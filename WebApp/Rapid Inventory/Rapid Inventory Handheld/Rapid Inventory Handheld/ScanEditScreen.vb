Imports System.Net
Imports System.Data.SqlServerCe
Imports System.Data

Public Class ScanEditScreen

    Public dbConn As SqlCeConnection = LoginScreen.dbConn
    Public sqlCmd As SqlCeCommand = Nothing
    Public dataReader As SqlCeDataReader = Nothing

    Public username As String = LoginScreen.username
    Public companyName As String = LoginScreen.companyName
    Public locationId As String = LoginScreen.locationId
    Public sectionId As String = SectionNumberScreen.sectionId
    Public barcode As String

    Public countQty As String
    Public itemNo As String


    Public Sub New(ByVal barcode As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        'set barcode passed to this barcode
        Me.barcode = barcode

        ' Add any initialization after the InitializeComponent() call.
        fillForm()

    End Sub


    Private Sub fillForm()

        Try

            'set barcode to barcode number scanned
            lblBarcode.Text = barcode

            'open(connection)
            dbConn.Open()

            'get all the barcode data to display
            sqlCmd = New SqlCeCommand("SELECT B.BARCOD, B.ITEM_NO, I.DESCR, B.DIM_1_UPR, B.DIM_2_UPR, B.DIM_3_UPR " & _
                                      "FROM RAPID_IM_BARCOD B " & _
                                      " LEFT OUTER JOIN RAPID_IM_INV_CELL D ON B.ITEM_NO = D.ITEM_NO AND B.LOC_ID = D.LOC_ID AND B.COMPANY_NAM = D.COMPANY_NAM " & _
                                      " LEFT OUTER JOIN RAPID_IM_ITEM I ON B.ITEM_NO = I.ITEM_NO AND B.LOC_ID = I.LOC_ID AND B.COMPANY_NAM = I.COMPANY_NAM " & _
                                      "WHERE B.BARCOD = '" & barcode & "' " & _
                                      " AND B.LOC_ID = '" & locationId & "'" & _
                                      " AND B.COMPANY_NAM = '" & companyName & "'", dbConn)

            'create data reader
            dataReader = sqlCmd.ExecuteReader

            While dataReader.Read

                'fill txt boxes on form to be able to delete them
                txtboxItemNo.Text = dataReader(1).ToString
                txtboxScanQty.Text = "1"
                txtBoxScanDescr.Text = dataReader(2).ToString
                txtBoxGridDim1.Text = dataReader(3).ToString
                txtBoxGridDim2.Text = dataReader(4).ToString
                txtBoxGridDim3.Text = dataReader(5).ToString

            End While

            'clean up
            sqlCmd.Dispose()
            dataReader.Close()

            txtboxItemNo.Enabled = False
            txtBoxScanDescr.Enabled = False

        Catch ex As Exception

        Finally

            dbConn.Close()

        End Try

    End Sub

    Private Sub UpdateItem()

        countQty = txtboxScanQty.Text.ToString

        Dim itemNo As String = txtboxItemNo.Text
        Dim gridDim1 As String = txtBoxGridDim1.Text
        Dim gridDim2 As String = txtBoxGridDim1.Text
        Dim gridDim3 As String = txtBoxGridDim1.Text

        dbConn.Open()

        'sqlCmd.ExecuteNonQuery()

        'sqlCmd.Dispose()

        dbConn.Close()

    End Sub

    Private Sub btnUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdate.Click

        UpdateItem()

        Dim countMenu As New CountMenu(sectionId)
        countMenu.insertScan(companyName, locationId, sectionId, barcode, countQty, LoginScreen.handheldId)

        countMenu.Show()

        countMenu.txtBoxBarcode.Focus()

        Me.Close()

    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click

        Dim result As Integer = MsgBox("Are you sure you want to delete this record?", MsgBoxStyle.YesNo)

        If result = DialogResult.Yes Then

            Try

                dbConn.Open()

                sqlCmd = New SqlCeCommand("DELETE RAPID_RAW_SCAN_DATA " & _
                                          "WHERE BARCOD = '" & barcode & "'", dbConn)

                sqlCmd.ExecuteNonQuery()

                sqlCmd.Dispose()

                dbConn.Close()

                Dim countMenu As New CountMenu(sectionId)
                countMenu.insertScan(companyName, locationId, sectionId, barcode, countQty, LoginScreen.handheldId)

                countMenu.Show()

                'close form
                Me.Close()

            Catch ex As Exception

                MsgBox(ex.Message)

            End Try
        End If

    End Sub
End Class