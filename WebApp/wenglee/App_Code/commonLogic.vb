Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.SqlClient


Public Class commonLogic
    Public Shared Function getBarcodeType(ByVal barcode As String) As Integer
        ' Determines barcode type
        '   0 = Other / 3rd party
        '   1 = UPC
        '   2 = Wing Lee or Pitman
        '   3 = Invalid / non-numeric

        Dim barcodeType As Integer = 0

        If Not IsNumeric(barcode) Then
            barcodeType = 3
        ElseIf iSWingLeeBarcode(barcode) Or iSPitmanBarcode(barcode) Then
            barcodeType = 2
        ElseIf isUpc(barcode) Then
            barcodeType = 1
        Else
            barcodeType = 0
        End If

        Return barcodeType
    End Function


    Public Shared Function isUpc(ByVal UPC As String) As Boolean
        ' Check to see if inputted UPC Code is valid according to the Check Digit Verification Method
        Try
            Dim tempUpc As Int64
            tempUpc = UPC
        Catch ex As Exception
            Return False
        End Try

        If Len(UPC) > 12 Then
            Return False
        End If

        Dim tempData As String = UPC.ToString()
        Dim UPCData(11) As Integer, i As Integer, result As Integer
        ' Convert UPC Code into an Array to access specific digits easily
        For i = 0 To tempData.Length - 1
            UPCData(i) = Convert.ToInt32(tempData.Substring(i, 1))
        Next
        ' Check validity of UPC Code
        result = ((UPCData(0) + UPCData(2) + UPCData(4) + UPCData(6) + UPCData(8) + UPCData(10)) * 3 _
       + (UPCData(1) + UPCData(3) + UPCData(5) + UPCData(7) + UPCData(9))) + UPCData(11)
        If result Mod 10 = 0 Then
            ' Valid UPC Code
            Return True
        Else
            ' Invalid UPC Code
            Return False
        End If
    End Function

    Public Shared Function iSWingLeeBarcode(ByVal barcode As String) As Boolean
        ' Check to see if barcode belongs to Wing Lee

        If Left(barcode, 8) = System.Configuration.ConfigurationManager.AppSettings("wingLeeBarcodeFirstEightDigits") And
          Len(barcode) = System.Configuration.ConfigurationManager.AppSettings("wingLeeBarcodeLength") Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Shared Function iSPitmanBarcode(ByVal barcode As String) As Boolean
        ' Check to see if barcode belongs to Wing Lee

        If Left(barcode, 6) = System.Configuration.ConfigurationManager.AppSettings("pitmanBarcodeFirstSixDigits") And
          Len(barcode) = System.Configuration.ConfigurationManager.AppSettings("pitmanBarcodeLength") Then
            Return True
        Else
            Return False
        End If

    End Function

    Public Shared Function getItemCodeFromWingLeeBarcode(ByVal barcode As String) As String
        Dim myNumStr As String
        myNumStr = Mid(barcode, System.Configuration.ConfigurationManager.AppSettings("wingLeeBarcodeItemIdBeginsAtPosition"),
                   System.Configuration.ConfigurationManager.AppSettings("wingLeeBarcodeItemIdEndsAtPosition") -
                   System.Configuration.ConfigurationManager.AppSettings("wingLeeBarcodeItemIdBeginsAtPosition") + 1)
        Return myNumStr.TrimStart("0"c)
    End Function

    Public Shared Function getItemCodeFromPitmanBarcode(ByVal barcode As String) As String
        Dim myNumStr As String
        Return Mid(barcode, System.Configuration.ConfigurationManager.AppSettings("pitmanBarcodeItemIdBeginsAtPosition"),
                   System.Configuration.ConfigurationManager.AppSettings("pitmanBarcodeItemIdEndsAtPosition") -
                   System.Configuration.ConfigurationManager.AppSettings("pitmanBarcodeItemIdBeginsAtPosition") + 1)
        Return myNumStr.TrimStart("0"c)
    End Function

    Public Shared Function getWeightFromWingLeePitmanBarcode(ByVal barcode As String) As Decimal
        Dim myNumStr As String
        Return Mid(barcode, System.Configuration.ConfigurationManager.AppSettings("wingLeePitmanBarcodeWeightBeginsAtPosition"),
                   System.Configuration.ConfigurationManager.AppSettings("wingLeePitmanBarcodeWeightEndsAtPosition") -
                   System.Configuration.ConfigurationManager.AppSettings("wingLeePitmanBarcodeWeightBeginsAtPosition") + 1) / 100
        Return myNumStr.TrimStart("0"c)
    End Function

    Public Function generateReceiverId() As String
        ' This function generates a receiver ID using the current time and the client's IP address
        Dim time As DateTime = DateTime.Now
        Dim format As String = "REC-yyyyMMddHHmmss.fff-" & HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Return time.ToString(format)
    End Function

    Public Function generatePickingId() As String
        ' This function generates a picking ID using the current time and the client's IP address
        Dim time As DateTime = DateTime.Now
        Dim format As String = "PICK-yyyyMMddHHmmss.fff-" & HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")

        Return time.ToString(format)
    End Function

    Public Function receiverHasUncommittedScans(ByVal receiverId As String) As Boolean
        If Len(receiverId) Then
            Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
            Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_RETRIEVE_SCANNED_ITEMS_FOR_RECEIVER", con)
            Dim latestRecordId As Integer

            cmd.CommandType = Data.CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("RECEIVER_NO", receiverId)

            cmd.Connection.Open()

            latestRecordId = cmd.ExecuteScalar

            If latestRecordId > 0 Then
                Return True
            Else
                Return False
            End If

            cmd.Dispose()
            con.Close()
        Else
            Return False
        End If
    End Function

    Public Function orderHasUncommittedScans(ByVal pickingId As String) As Boolean
        If Len(pickingId) Then
            Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
            Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_RETRIEVE_SCANNED_ITEMS_FOR_ORDER", con)
            Dim latestRecordId As Integer

            cmd.CommandType = Data.CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("PICKING_NO", pickingId)

            cmd.Connection.Open()

            latestRecordId = cmd.ExecuteScalar

            If latestRecordId > 0 Then
                Return True
            Else
                Return False
            End If

            cmd.Dispose()
            con.Close()
        Else
            Return False
        End If
    End Function

    Public Function getVendorDesc(ByVal vendorNumber As String) As String
        If Len(vendorNumber) Then
            Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
            Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_GET_VENDOR_DESC", con)
            Dim dr As SqlDataReader

            cmd.CommandType = Data.CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("VEND_NO", vendorNumber)

            cmd.Connection.Open()

            dr = cmd.ExecuteReader()

            If dr.Read Then
                Return dr("NAM")
            Else
                Return ""
            End If

            cmd.Dispose()
            con.Close()
        Else
            Return ""
        End If
    End Function

    Public Function getCustomerDesc(ByVal customerNumber As String) As String
        If Len(customerNumber) Then
            Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
            Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_GET_CUSTOMER_DESC", con)
            Dim dr As SqlDataReader

            cmd.CommandType = Data.CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("CUST_NO", customerNumber)

            cmd.Connection.Open()

            dr = cmd.ExecuteReader()

            If dr.Read Then
                Return dr("NAM")
            Else
                Return ""
            End If

            cmd.Dispose()
            con.Close()
        Else
            Return ""
        End If
    End Function




End Class


