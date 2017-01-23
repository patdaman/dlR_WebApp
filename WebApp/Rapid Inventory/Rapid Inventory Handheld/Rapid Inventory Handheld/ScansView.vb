Imports System.Net
Imports System.Data.SqlServerCe
Imports System.Data
Imports System.Windows.Forms

Public Class ScansView

    Public dbConn As SqlCeConnection = LoginScreen.dbConn
    Public sqlCmd As SqlCeCommand = Nothing
    Public dataReader As SqlCeDataReader = Nothing

    'get values from login
    Public username As String = LoginScreen.username
    Public companyName As String = LoginScreen.companyName
    Public locationId As String = LoginScreen.locationId
    Public sectionId As String


    Public Shared barcode As String
    Public countQty As String
    Public itemNo As String
    Public gridDim As String

    Public Sub New(ByVal sectionId As String)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        Me.sectionId = sectionId

        'lblSectionNumber.Text = "Section #" & sectionId

        'fill datagrid with scan item data
        fillDataGrid()

    End Sub

    'function to refresh scan data based on section
    Private Sub fillDataGrid()

        Try

            'open connection
            dbConn.Open()

            'get all the barcode data to display
            sqlCmd = New SqlCeCommand("SELECT BARCOD AS Barcode, CNT_QTY AS Qty, SCAN_DAT AS [Scan Date] FROM RAPID_RAW_SCAN_DATA ORDER BY SCAN_DAT DESC", dbConn)

            'create data set
            Dim sqlDataSet = New DataSet()

            'create data adapter
            Dim sqlDataAdapter = New SqlCeDataAdapter(sqlCmd)

            'fill data adapter with results from query
            sqlDataAdapter.Fill(sqlDataSet)

            'fill grid with result from dataset
            dataGridScans.DataSource = sqlDataSet.Tables(0)

            'clean up
            sqlCmd.Dispose()

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            'Close database connection
            dbConn.Close()
        End Try

    End Sub

    Private Sub btnEditScan_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditScan.Click

        'Get current row selected
        Dim selectedRow As Integer = dataGridScans.CurrentRowIndex

        'get the barcode from the current row
        barcode = dataGridScans.Item(selectedRow, 0).ToString

        'show scan dialog edit screen
        Dim scanEditScreen As Form = New ScanEditScreen(barcode)
        scanEditScreen.Show()

        'hide current form
        Me.Hide()

    End Sub

    Private Sub btnDone_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDone.Click

        Dim countMenu As Form = New CountMenu(sectionId)
        countMenu.Show()

        Me.Close()

    End Sub

    'Private Sub dataGridScans_CurrentCellChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dataGridScans.CurrentCellChanged
    '   MsgBox(dataGridScans.CurrentRowIndex.ToString())

    '   MsgBox(dataGridScans.Item(dataGridScans.CurrentRowIndex, 0).ToString)
    'End Sub
End Class