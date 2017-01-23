Imports System.Configuration
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net

Public Class DashBoard
    Inherits System.Web.UI.Page

    Dim constring As String = ConfigurationManager.ConnectionStrings("DbConn").ConnectionString
    Dim dbConn As SqlConnection = New SqlConnection(constring)
    Dim sqlCmd As SqlCommand

    Dim companyName As String
    Dim location As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            populateMenuBar()
            populateDashboard(companyName, location)

        End If

        companyName = Request.QueryString("field1")
        location = Request.QueryString("field2")

        Session("companyName") = companyName
        Session("location") = location

    End Sub

    Public Sub populateMenuBar()

        Dim menu As MenuItem = New MenuItem
        menu.Text = "Cool"

        Dim menu2 As MenuItem = New MenuItem
        menu2.Text = "NOW"

        'menuBar.Items.Add(menu)

        'menuBar.Items.Add(menu2)
        companyName = Request.QueryString("field1")
        location = Request.QueryString("field2")

    End Sub

    Public Sub populateDashboard(ByVal companyName As String, ByVal location As String)

        'set the resreshed label to the current time
        lblLastRefreshed.Text = "Last Refreshed " & Date.Now

        If dbConn.State = ConnectionState.Closed Then

            dbConn.Open()

        End If

        'get counts of section scanned
        sqlCmd = New SqlCommand("RAPID_SP_GET_SECTION_COUNTS", dbConn)

        sqlCmd.CommandType = Data.CommandType.StoredProcedure

        sqlCmd.Parameters.Add("@COMPANY_NAM", Data.SqlDbType.VarChar).Value = companyName

        sqlCmd.Parameters.Add("@LOC_ID", Data.SqlDbType.VarChar).Value = location

        lblCounts.Text = sqlCmd.ExecuteScalar.ToString

        'get list of non validated counts
        sqlCmd = New SqlCommand("RAPID_SP_LIST_OF_NON_VALID_COUNTS", dbConn)

        sqlCmd.CommandType = Data.CommandType.StoredProcedure

        sqlCmd.Parameters.Add("@COMPANY_NAM", Data.SqlDbType.VarChar).Value = companyName

        sqlCmd.Parameters.Add("@LOC_ID", Data.SqlDbType.VarChar).Value = location

        Dim sqlDataAdapter = New SqlDataAdapter(sqlCmd)

        Dim sqlDataSet = New DataSet

        sqlDataAdapter.Fill(sqlDataSet)

        If sqlDataSet.Tables.Count > 0 Then

            gridNonValidCounts.DataSource = sqlDataSet.Tables(0)
            gridNonValidCounts.AllowPaging = True
            gridNonValidCounts.DataBind()

        End If

        'get list of sections counted 
        sqlCmd = New SqlCommand("RAPID_SP_LIST_OF_SECTIONS_COUNTED", dbConn)

        sqlCmd.CommandType = Data.CommandType.StoredProcedure

        sqlCmd.Parameters.Add("@COMPANY_NAM", Data.SqlDbType.VarChar).Value = companyName

        sqlCmd.Parameters.Add("@LOC_ID", Data.SqlDbType.VarChar).Value = location

        Dim sqlDataReader As SqlDataReader = sqlCmd.ExecuteReader

        gridSectionsCounted.DataSource = sqlDataReader
        gridSectionsCounted.DataBind()

        sqlDataReader.Close()

        sqlCmd.Dispose()

        dbConn.Close()

    End Sub

    Public Sub deleteSectionCount(ByVal sectionId As String, ByVal companyName As String, ByVal location As String)

        dbConn.Open()

        sqlCmd = New SqlCommand("RAPID_SP_DELETE_SECTION_COUNT", dbConn)

        sqlCmd.CommandType = Data.CommandType.StoredProcedure

        sqlCmd.Parameters.Add("@SECTION_ID", Data.SqlDbType.VarChar).Value = sectionId

        sqlCmd.Parameters.Add("@COMPANY_NAM", Data.SqlDbType.VarChar).Value = companyName

        sqlCmd.Parameters.Add("@LOC_ID", Data.SqlDbType.VarChar).Value = location

        Dim sqlResult As Integer = sqlCmd.ExecuteNonQuery()

        If sqlResult > 1 Then

            MsgBox("Section " & sectionId & " count deleted")

            populateDashboard(companyName, location)

            sqlCmd.Dispose()

            dbConn.Close()

        End If

        sqlCmd.Dispose()

        dbConn.Close()

    End Sub

    Protected Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click

        'refresh page
        Response.Redirect("DashBoard.aspx?field1=" & companyName & "&field2=" & location)

    End Sub

    Protected Sub btnEdit_Click(sender As Object, e As GridViewCommandEventArgs) Handles gridSectionsCounted.RowCommand

        If e.CommandName = "ChangeSectionNumber" Then

            'get the index of the row selected 
            Dim rowIndex As Integer = Convert.ToInt32(e.CommandArgument.ToString)

            'set the edit index to the row selected
            gridSectionsCounted.EditIndex = rowIndex

            companyName = Session("companyName")
            location = Session("location")

            'populate the grid 
            populateDashboard(companyName, location)

        End If

    End Sub

    'funtion called when edit button is pressed
    Protected Sub gridSectionsCounted_RowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs) Handles gridSectionsCounted.RowEditing

        'Set the edit index.
        gridSectionsCounted.EditIndex = e.NewEditIndex
        Dim rowIndex As Integer = e.NewEditIndex

        'get the sectionId of the row selected
        Dim sectionId As String = gridSectionsCounted.Rows(rowIndex).Cells(3).Text

        MsgBox(sectionId)

        'go to the section edit screen
        Response.Redirect("EditSection.aspx?field1=" & companyName & "&field2=" & location & "&field3=" & sectionId)

    End Sub


    'function called when delete button is pressed
    Protected Sub gridSectionsCounted_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs) Handles gridSectionsCounted.RowDeleting

        Dim result As Integer = MsgBox("Are you sure you wish to delete the count for this section?", MsgBoxStyle.YesNo)

        If result = MsgBoxResult.Yes Then

            'get the section id of the row selected
            Dim rowIndex As Integer = e.RowIndex
            Dim sectionId As String = gridSectionsCounted.Rows(rowIndex).Cells(5).Text

            companyName = Session("companyName")
            location = Session("location")

            'delete the section 
            deleteSectionCount(sectionId, companyName, location)

            'bind the data to the grid
            populateDashboard(companyName, location)

        End If

    End Sub

    'function is called when data is bound to the data grid
    Protected Overridable Sub OnRowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gridSectionsCounted.RowDataBound

        'set the proper fields to read only 
        Dim bndFieldScanDate As BoundField = DirectCast(DirectCast(e.Row.Cells(4), DataControlFieldCell).ContainingField, BoundField)
        Dim bndFieldSumQty As BoundField = DirectCast(DirectCast(e.Row.Cells(5), DataControlFieldCell).ContainingField, BoundField)

        bndFieldScanDate.ReadOnly = True
        bndFieldSumQty.ReadOnly = True

    End Sub


    Private Sub OnUpdateDataBound(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs) Handles gridSectionsCounted.RowUpdating

        companyName = Session("companyName")
        location = Session("location")

        'get the index of the row selected
        Dim rowIndex As Integer = e.RowIndex

        Dim oldSectionId = e.OldValues.Item(5)

        Dim newSectionId = e.NewValues.Item(5)

        MsgBox(oldSectionId)

        MsgBox(newSectionId)

        dbConn.Open()

        sqlCmd = New SqlCommand("RAPID_SP_UPDATE_SECTION_NUMBER", dbConn)

        sqlCmd.CommandType = Data.CommandType.StoredProcedure

        sqlCmd.Parameters.Add("@OLD_SECTION_ID", Data.SqlDbType.VarChar).Value = newSectionId

        sqlCmd.Parameters.Add("@NEW_SECTION_ID", Data.SqlDbType.VarChar).Value = newSectionId

        sqlCmd.Parameters.Add("@COMPANY_NAM", Data.SqlDbType.VarChar).Value = companyName

        sqlCmd.Parameters.Add("@LOC_ID", Data.SqlDbType.VarChar).Value = location

        Dim sqlResult As Integer = sqlCmd.ExecuteNonQuery()

        If sqlResult > 1 Then

            MsgBox("Section updated")

            populateDashboard(companyName, location)

            sqlCmd.Dispose()

            dbConn.Close()

        End If

        sqlCmd.Dispose()

        dbConn.Close()

    End Sub

    Private Sub OnPageIndexChange(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gridNonValidCounts.PageIndexChanging

        gridNonValidCounts.PageIndex = e.NewPageIndex

        companyName = Session("companyName")
        location = Session("location")

        populateDashboard(companyName, location)

    End Sub

End Class