Imports System.IO
Imports System.Configuration
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net

Public Class DeleteCount
    Inherits System.Web.UI.Page

    Dim constring As String = ConfigurationManager.ConnectionStrings("DbConn").ConnectionString
    Dim dbConn As SqlConnection = New SqlConnection(constring)
    Dim sqlCmd As SqlCommand

    Dim companyName As String
    Dim location As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then

            companyName = Request.QueryString("field1")
            location = Request.QueryString("field2")

            populateCountGrid(companyName, location, "rapidadmin")

        End If

        companyName = Request.QueryString("field1")
        location = Request.QueryString("field2")

        Session("companyName") = companyName
        Session("location") = location

    End Sub

    Public Sub populateCountGrid(ByVal company As String, ByVal location As String, ByVal username As String)

        dbConn.Open()

        Try

            sqlCmd = New SqlCommand("RAPID_SP_GET_COUNT_LIST", dbConn)

            sqlCmd.CommandType = CommandType.StoredProcedure

            sqlCmd.Parameters.Add("@COMPANY_NAM", SqlDbType.VarChar).Value = company

            sqlCmd.Parameters.Add("@LOC_ID", SqlDbType.VarChar).Value = location

            sqlCmd.Parameters.Add("@USR_NAM", SqlDbType.VarChar).Value = username

            Dim sqlDataReader As SqlDataReader = sqlCmd.ExecuteReader

            gridDeleteCount.DataSource = sqlDataReader
            gridDeleteCount.DataBind()

            sqlDataReader.Close()

        Catch ex As Exception
            MsgBox(ex.Message)

        Finally

            sqlCmd.Dispose()

            dbConn.Close()

        End Try

    End Sub

    Public Sub deleteCount(ByVal companyName As String, ByVal location As String)

        dbConn.Open()

        Try

            sqlCmd = New SqlCommand("RAPID_SP_DELETE_COUNT", dbConn)

            sqlCmd.CommandType = CommandType.StoredProcedure

            sqlCmd.Parameters.Add("@COMPANY_NAM", SqlDbType.VarChar).Value = companyName

            sqlCmd.Parameters.Add("@LOC_ID", SqlDbType.VarChar).Value = location

            sqlCmd.ExecuteNonQuery()

        Catch ex As Exception

            MsgBox(ex.Message)

        Finally

            sqlCmd.Dispose()

            dbConn.Close()

        End Try


    End Sub

    'function called when delete button is pressed
    Protected Sub gridDeleteCount_RowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs) Handles gridDeleteCount.RowDeleting

        Dim result As Integer = MsgBox("Are you sure you wish to delete the counts for this location?", MsgBoxStyle.YesNo)

        If result = MsgBoxResult.Yes Then

            'get the company and location of the row selected
            Dim rowIndex As Integer = e.RowIndex
            Dim deletecompany As String = gridDeleteCount.Rows(rowIndex).Cells(1).Text
            Dim deleteloc As String = gridDeleteCount.Rows(rowIndex).Cells(2).Text

            'delete the section 
            deleteCount(deletecompany, deleteloc)

            'bind the data to the grid
            populateCountGrid(companyName, location, "rapidadmin")

        End If

    End Sub
End Class