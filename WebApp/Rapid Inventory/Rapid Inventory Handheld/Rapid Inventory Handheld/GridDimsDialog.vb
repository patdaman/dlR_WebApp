Imports System.Data.SqlServerCe

Public Class GridDimsDialog

    Public dbConn As SqlCeConnection = LoginScreen.dbConn
    Public sqlCmd As SqlCeCommand = Nothing
    Public dataReader As SqlCeDataReader = Nothing

    Public username As String
    Public companyName As String
    Public locationId As String
    Public sectionId As String
    Public barcode As String

    Private gridDim1 As String
    Private gridDim2 As String
    Private gridDim3 As String

    Public Sub New(ByVal sectionId As String, ByVal barcode As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.sectionId = sectionId
        Me.barcode = barcode

        ' Add any initialization after the InitializeComponent() call.
        fillGridComboBoxes()

        'set the barcode lable to the barcode entered on scan
        lblBarcode.Text = CountMenu.barcode

    End Sub

    'function to fill combo boxes with Grid dimen tag
    Public Sub fillGridComboBoxes()
        Try

            'open cennection
            dbConn.Open()

            'get the grid tag for the item scanned 
            sqlCmd = New SqlCeCommand("SELECT GRID_DIM_1_TAG, GRID_DIM_1_TAG, GRID_DIM_1_TAG FROM RAPID_IM_ITEM I " & _
                                      " INNER JOIN RAPID_IM_BARCOD B ON B.ITEM_NO = I.ITEM_NO AND B.LOC_ID = I.LOC_ID AND B.COMPANY_NAM = I.COMPANY_NAM " & _
                                      "WHERE B.BARCOD = '" & barcode & "'", dbConn)

            dataReader = sqlCmd.ExecuteReader

            While dataReader.Read
                'fill dimension combo boxes
                cmbBoxGridDim1.Items.Add(dataReader(0).ToString)

                cmbBoxGridDim2.Items.Add(dataReader(1).ToString)

                cmbBoxGridDim3.Items.Add(dataReader(2).ToString)

            End While

            'clean up
            dataReader.Close()
            sqlCmd.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

            'close connection
            dbConn.Close()

        End Try
    End Sub

    Public Function forceGridItemEntry()

        Try

            'open connection
            dbConn.Open()

            'get the grid dimension entered in dialog
            gridDim1 = txtboxGridDim1.Text.ToString

            'get the grid dimension entered in dialog
            gridDim2 = txtboxGridDim2.Text.ToString

            'get the grid dimension entered in dialog
            gridDim3 = txtboxGridDim3.Text.ToString

            'if a value is entered into the grid dimension
            If String.IsNullOrEmpty(gridDim1) = False Then

                'get the item no to insert the dimension
                sqlCmd = New SqlCeCommand("SELECT ITEM_NO FROM RAPID_IM_BARCOD WHERE BARCOD = '" & barcode & "'", dbConn)

                Dim itemNo As String = sqlCmd.ExecuteScalar.ToString

                'insert it into RAPID_IM_GRID_DIMS
                sqlCmd = New SqlCeCommand("INSERT INTO RAPID_IM_GRID_DIMS (COMPANY_NAM, LOC_ID, ITEM_NO, DIM_1_UPR, DIM_1_SEQ_NO, DIM_2_UPR" & _
                                          " , DIM_2_SEQ_NO, DIM_3_UPR, DIM_3_SEQ_NO) " & _
                                          "SELECT '" & companyName & "','" & locationId & "','" & itemNo & "','" & gridDim1 & "', '1'" & _
                                          " , '*', 1, '*', 1", dbConn)

                'execute the insert statement
                sqlCmd.ExecuteNonQuery()

                'if second dimension exists
                If String.IsNullOrEmpty(gridDim2) = False Then

                    'insert it into RAPID_IM_GRID_DIMS
                    sqlCmd = New SqlCeCommand("UPDATE RAPID_IM_GRID_DIMS " & _
                                              "SET DIM_2_UPR = '" & gridDim2 & "'" & _
                                              " , DIM_2_SEQ_NO = 1" & _
                                              "WHERE COMPANY_NAME =  '" & companyName & "'" & _
                                              " AND LOC_ID = '" & locationId & "'" & _
                                              " AND ITEM_NO = '" & itemNo & "'" & _
                                              " AND DIM_1_UPR = '" & gridDim1 & "'", dbConn)

                    'execute the insert statement
                    sqlCmd.ExecuteNonQuery()
                End If

                'if 3rd dimension exists
                If String.IsNullOrEmpty(gridDim3) = False Then

                    'insert it into RAPID_IM_GRID_DIMS
                    sqlCmd = New SqlCeCommand("UPDATE RAPID_IM_GRID_DIMS " & _
                                              "SET DIM_3_UPR = '" & gridDim2 & "'" & _
                                              " , DIM_3_SEQ_NO = 1" & _
                                              "WHERE COMPANY_NAME =  '" & companyName & "'" & _
                                              " AND LOC_ID = '" & locationId & "'" & _
                                              " AND ITEM_NO = '" & itemNo & "'" & _
                                              " AND DIM_1_UPR = '" & gridDim1 & "'" & _
                                              " AND DIM_2_UPR = '" & gridDim2 & ",", dbConn)

                    'execute the insert statement
                    sqlCmd.ExecuteNonQuery()
                End If

                'if no value was entered
            Else
                MsgBox("You must enter a value for at least one grid dimension")

                Return False

            End If


        Catch ex As Exception
            MsgBox(ex.Message)

            sqlCmd.Dispose()
            dbConn.Close()

        End Try

        sqlCmd.Dispose()
        dbConn.Close()

        Return True

    End Function

    Private Sub btnForce_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForce.Click

        Dim result = forceGridItemEntry()

        If result = True Then

            Dim count = New CountMenu(sectionId)
            count.Show()

            Me.Close()

        End If

    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        'close current window
        Me.Close()
    End Sub
End Class