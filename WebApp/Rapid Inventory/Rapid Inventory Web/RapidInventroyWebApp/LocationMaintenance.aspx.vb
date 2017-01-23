Imports System.Configuration
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net

Public Class LocationMaintenance
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

            populateMenuBar()

        End If

        companyName = Request.QueryString("field1")
        location = Request.QueryString("field2")

        Session("companyName") = companyName
        Session("location") = location

    End Sub

    Public Sub populateMenuBar()

    End Sub

    Protected Sub btnEdit_Click(sender As Object, e As GridViewCommandEventArgs) Handles gridLocation.RowCommand

        If e.CommandName = "Insert" Then

            Dim company = DirectCast(gridLocation.FooterRow.FindControl("txtBoxCompanyName"), TextBox).Text
            Dim location = DirectCast(gridLocation.FooterRow.FindControl("txtBoxLocation"), TextBox).Text
            Dim username = DirectCast(gridLocation.FooterRow.FindControl("txtBoxUsername"), TextBox).Text
            Dim password = DirectCast(gridLocation.FooterRow.FindControl("txtBoxPassword"), TextBox).Text
            Dim manualItemEntry = DirectCast(gridLocation.FooterRow.FindControl("txtBoxManualItemEntry"), TextBox).Text

            dbConn.Open()

            sqlCmd = New SqlCommand("RAPID_SP_CREATE_USR", dbConn)

            sqlCmd.CommandType = Data.CommandType.StoredProcedure

            sqlCmd.Parameters.Add("@COMPANY_NAM", Data.SqlDbType.VarChar).Value = company

            sqlCmd.Parameters.Add("@LOC_ID", Data.SqlDbType.VarChar).Value = location

            sqlCmd.Parameters.Add("@USR_NAM", Data.SqlDbType.VarChar).Value = username

            sqlCmd.Parameters.Add("@PWD", Data.SqlDbType.VarChar).Value = password

            sqlCmd.Parameters.Add("@MANUAL_ITEM_ENTRY", Data.SqlDbType.VarChar).Value = manualItemEntry

            Dim sqlResult As Integer = sqlCmd.ExecuteNonQuery()

            sqlCmd.Dispose()

            dbConn.Close()

            gridLocation.DataBind()

        End If

    End Sub

End Class