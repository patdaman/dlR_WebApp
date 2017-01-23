Imports System.IO
Imports System.Configuration
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net
Imports System.Windows

Public Class UploadItemData
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

            populateDataExportQuery(companyName, location)

        End If


    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click

        If ItemFileUpload.HasFile Then

            If ItemFileUpload.PostedFile.ContentLength > 20971520 Then

                lblErrMsg.ForeColor = Drawing.Color.Red
                lblErrMsg.Text = "File size must be under 20 MB"

            Else

                'check to see if directory exists
                If (Not System.IO.Directory.Exists("C:\RapidInventory\" & companyName)) Then

                    System.IO.Directory.CreateDirectory("C:\RapidInventory\" & companyName)

                End If

                ItemFileUpload.SaveAs("C:\RapidInventory\" & companyName & "\" + companyName & "_" & location & "_" & Year(Date.Now) & Month(Date.Now) & Day(Date.Now) & Hour(Date.Now) & Second(Date.Now) & ".csv")

                Dim filePath As String = "C:\RapidInventory\" & companyName & "\" & companyName & "_" & location & "_" & Year(Date.Now) & Month(Date.Now) & Day(Date.Now) & Hour(Date.Now) & Second(Date.Now) & ".csv"

                insertItemData(filePath)

            End If
        End If

    End Sub

    Public Sub populateDataExportQuery(ByVal location As String, ByVal company As String)

        Try

            dbConn.Open()

            sqlCmd = New SqlCommand("RAPID_QUERY_FOR_ITEM_UPLOAD", dbConn)

            sqlCmd.CommandType = CommandType.StoredProcedure

            sqlCmd.Parameters.Add("@COMPANY_NAM", SqlDbType.VarChar).Value = company

            sqlCmd.Parameters.Add("@LOC_ID", SqlDbType.VarChar).Value = location

            Dim result As String = sqlCmd.ExecuteScalar

            lblQueryforExport.Text = result

        Catch ex As Exception
            MsgBox(ex.Message)

        Finally

            dbConn.Close()

        End Try

    End Sub

    Public Sub uploadItemData(ByVal fileName As String)

        'Dim file As FileIO.TextFieldParser = New FileIO.TextFieldParser(fileName)

        'Dim currentRow As String()

        'file.TextFieldType = FileIO.FieldType.Delimited

        'file.Delimiters = New String() {"~/\~"}

        'While Not file.EndOfData

        'currentRow = File.ReadFields

        'Dim tableName = currentRow(0)

        'If tableName = "RAPID_IM_BARCOD" Then

        'insertIntoBarcodeTable(currentRow(1), currentRow(2), currentRow(3), currentRow(4), currentRow(5), currentRow(6))

        'End If

        'End While


    End Sub

    Public Sub insertItemData(ByVal filePath)

        dbConn.Open()

        Dim sqlCmd2 As SqlCommand = New SqlCommand("ALTER DATABASE RAPIDINVENTORY SET RECOVERY BULK_LOGGED", dbConn)

        sqlCmd2.ExecuteNonQuery()

        sqlCmd = dbConn.CreateCommand

        Dim tran As SqlTransaction

        tran = dbConn.BeginTransaction

        sqlCmd.Connection = dbConn

        sqlCmd.Transaction = tran

        Try

            sqlCmd.CommandTimeout = 240

            sqlCmd.CommandText = "RAPID_SP_UPLOAD_ITEM_FILE"

            sqlCmd.CommandType = CommandType.StoredProcedure

            sqlCmd.Parameters.Add("@FILE_PATH", SqlDbType.VarChar).Value = filePath

            sqlCmd.ExecuteNonQuery()

            tran.Commit()

            MsgBox("Upload successfull")

        Catch ex As Exception
            MsgBox(ex.Message)

            tran.Rollback()

        Finally
            Dim sqlCmd3 As SqlCommand = New SqlCommand("ALTER DATABASE RAPIDINVENTORY SET RECOVERY FULL", dbConn)

            sqlCmd3.ExecuteNonQuery()

            dbConn.Close()
        End Try

        dbConn.Close()


    End Sub

    'Public Sub insertIntoBarcodeTable(ByVal companyName As String, ByVal location As String, ByVal itemNo As String, ByVal Dimension1 As String, ByVal dimension2 As String, ByVal dimension3 As String)

    '   dbConn.Open()

    '    sqlCmd = New SqlCommand("INSERT INTO RAPID_IM_BARCOD")

    'End Sub

End Class