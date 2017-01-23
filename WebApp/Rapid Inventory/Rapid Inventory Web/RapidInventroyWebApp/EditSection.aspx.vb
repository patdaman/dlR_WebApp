Imports System.Configuration
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net

Public Class EditSection1
    Inherits System.Web.UI.Page

    Dim constring As String = ConfigurationManager.ConnectionStrings("DbConn").ConnectionString
    Dim dbConn As SqlConnection = New SqlConnection(constring)
    Dim sqlCmd As SqlCommand

    Dim companyName As String
    Dim location As String
    Dim sectionId As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            companyName = Request.QueryString("field1")
            location = Request.QueryString("field2")
            sectionId = Request.QueryString("field3")

            populateCountGrid(companyName, location, sectionId)

            Session("companyName") = companyName
            Session("location") = location
            Session("sectionId") = sectionId

        End If

    End Sub

    Public Sub populateCountGrid(ByVal companyn As String, ByVal loc As String, ByVal section As String)

        dbConn.Open()

        'get list of non validated counts
        sqlCmd = New SqlCommand("RAPID_SP_GET_RAW_SCAN_DATA", dbConn)

        sqlCmd.CommandType = Data.CommandType.StoredProcedure

        sqlCmd.Parameters.Add("@SECTION_ID", Data.SqlDbType.VarChar).Value = sectionId

        sqlCmd.Parameters.Add("@COMPANY_NAM", Data.SqlDbType.VarChar).Value = companyName

        sqlCmd.Parameters.Add("@LOC_ID", Data.SqlDbType.VarChar).Value = location

        Dim sqlDataReader As SqlDataReader = sqlCmd.ExecuteReader

        gridSectionScans.DataSource = sqlDataReader
        gridSectionScans.DataBind()

        sqlDataReader.Close()

        dbConn.Close()

    End Sub

    Public Sub deleteScanData(ByVal scanId As String, ByVal companyName As String, ByVal locationId As String)

        dbConn.Open()

        sqlCmd = New SqlCommand("RAPID_SP_DELETE_SCAN_DATA", dbConn)

        sqlCmd.CommandType = Data.CommandType.StoredProcedure

        sqlCmd.Parameters.Add("@SCAN_ID", Data.SqlDbType.VarChar).Value = scanId

        sqlCmd.Parameters.Add("@COMPANY_NAM", Data.SqlDbType.VarChar).Value = companyName

        sqlCmd.Parameters.Add("@LOC_ID", Data.SqlDbType.VarChar).Value = location

        sqlCmd.ExecuteScalar()

        dbConn.Close()

    End Sub

    'funtion called when edit button is pressed
    Protected Sub gridSectionScans_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles gridSectionScans.RowEditing

        'Set the edit index.
        gridSectionScans.EditIndex = e.NewEditIndex
        Dim rowIndex As Integer = e.NewEditIndex

        'get the location and comapny name from the current session
        location = Session("location")
        companyName = Session("companyName")
        sectionId = Session("sectionId")

        'bind data to grid
        populateCountGrid(companyName, location, sectionId)

    End Sub

    'function called when delete button is pressed
    Protected Sub gridSectionScans_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs) Handles gridSectionScans.RowDeleting

        Dim result As Integer = MsgBox("Are you sure you wish to delete this scan data?", MsgBoxStyle.YesNo)

        If result = MsgBoxResult.Yes Then

            'get the section id of the row selected
            Dim rowIndex As Integer = e.RowIndex
            Dim scanId As String = gridSectionScans.Rows(rowIndex).Cells(1).Text

            'get the location and comapny name from the current session
            location = Session("location")
            companyName = Session("companyName")
            sectionId = Session("sectionId")

            'delete the scan data 
            deleteScanData(scanId, companyName, location)

            'bind the data to the grid
            populateCountGrid(companyName, location, sectionId)

        End If

    End Sub

    Private Sub gridSectionScans_RowCancel(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs) Handles gridSectionScans.RowCancelingEdit

        gridSectionScans.EditIndex = -1

        'get the location and comapny name from the current session
        location = Session("location")
        companyName = Session("companyName")
        sectionId = Session("sectionId")

        populateCountGrid(companyName, location, sectionId)

    End Sub

    Private Sub gridSectionScans_RowUpdate(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs) Handles gridSectionScans.RowUpdating

        'get vnew values from textbox
        Dim scanId = gridSectionScans.Rows(e.RowIndex).Cells(1).Text
        Dim scanDate = e.NewValues.Item(0)
        'Dim barcode = e.NewValues.Item(1)
        Dim Descr = e.NewValues.Item(1)
        Dim Qty As Integer = e.NewValues.Item(2)

        'get the location and comapny name from the current session
        location = Session("location")
        companyName = Session("companyName")
        sectionId = Session("sectionId")

        dbConn.Open()

        sqlCmd = New SqlCommand("RAPID_SP_UPDATE_RAW_SCAN_DATA", dbConn)

        sqlCmd.CommandType = Data.CommandType.StoredProcedure

        sqlCmd.Parameters.Add("@SCAN_ID", Data.SqlDbType.Int).Value = scanId

        sqlCmd.Parameters.Add("@SCAN_DAT", Data.SqlDbType.DateTime).Value = scanDate

        'sqlCmd.Parameters.Add("@BARCOD", Data.SqlDbType.VarChar).Value = barcode

        sqlCmd.Parameters.Add("@QTY", Data.SqlDbType.Int).Value = Qty

        sqlCmd.Parameters.Add("@DESCR", Data.SqlDbType.VarChar).Value = Descr

        sqlCmd.Parameters.Add("@COMPANY_NAM", Data.SqlDbType.VarChar).Value = companyName

        sqlCmd.Parameters.Add("@LOC_ID", Data.SqlDbType.VarChar).Value = location

        sqlCmd.Parameters.Add("@SECTION_ID", Data.SqlDbType.VarChar).Value = sectionId

        sqlCmd.ExecuteScalar()

        dbConn.Close()

        'remove the edit mode from table
        gridSectionScans.EditIndex = -1

        'refresh grid
        populateCountGrid(companyName, location, sectionId)


    End Sub

    'function is called when data is bound to the data grid
    Protected Overridable Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gridSectionScans.RowDataBound

        'set the proper fields to read only 
        Dim bndFieldScanId As BoundField = DirectCast(DirectCast(e.Row.Cells(1), DataControlFieldCell).ContainingField, BoundField)
        Dim bndFieldBarcode As BoundField = DirectCast(DirectCast(e.Row.Cells(3), DataControlFieldCell).ContainingField, BoundField)
        'Dim bndFieldCount As BoundField = DirectCast(DirectCast(e.Row.Cells(3), DataControlFieldCell).ContainingField, BoundField)
        Dim bndFieldHandheldId As BoundField = DirectCast(DirectCast(e.Row.Cells(6), DataControlFieldCell).ContainingField, BoundField)

        bndFieldScanId.ReadOnly = True
        bndFieldBarcode.ReadOnly = True
        'bndFieldCount.ReadOnly = True
        bndFieldHandheldId.ReadOnly = True

    End Sub

End Class