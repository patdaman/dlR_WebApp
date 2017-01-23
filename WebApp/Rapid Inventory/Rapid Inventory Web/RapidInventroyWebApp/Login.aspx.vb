Imports System.Configuration
Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net
Imports System.Web.Security
Imports System.Web.Optimization


Public Class Login

    Inherits System.Web.UI.Page

    Dim constring As String = ConfigurationManager.ConnectionStrings("DbConn").ConnectionString
    Dim dbConn As SqlConnection = New SqlConnection(constring)

    Dim companyName As String
    Dim location As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        RegisterHyperLink.NavigateUrl = "Register"
        OpenAuthLogin.ReturnUrl = Request.QueryString("ReturnUrl")
        fillDropDownList()

        Dim returnUrl = HttpUtility.UrlEncode(Request.QueryString("ReturnUrl"))
        If Not String.IsNullOrEmpty(returnUrl) Then
            RegisterHyperLink.NavigateUrl &= "?ReturnUrl=" & returnUrl
        End If
    End Sub

    Public Sub fillDropDownList()

        Try

            dbConn.Open()

            Dim cmd As String = "SELECT DISTINCT COMPANY_NAM + '-' + LOC_ID AS COMP_LOC FROM RAPID_USR_ACCT"

            Dim sqlCmd As SqlCommand = New SqlCommand(cmd, dbConn)

            Dim dataReader As SqlDataReader = sqlCmd.ExecuteReader()

            While dataReader.Read

                compLocDropDown.Items.Add(dataReader("COMP_LOC").ToString)

            End While

            'clean up
            dataReader.Close()
            sqlCmd.Dispose()

        Catch ex As Exception

            MsgBox(ex.Message)

        Finally

            dbConn.Close()

        End Try

    End Sub

    Public Function IsAlphaNumeric(ByVal text As String) As Boolean

        Return Regex.IsMatch(text, "^[a-zA-Z0-9]+$")

    End Function

    Private Function ValidateCredentials(ByVal userName As String, ByVal password As String, ByVal companyLoc As String) As Boolean

        Dim returnValue As Boolean = False

        'Split the company and location string
        Dim companyLocArray() = Split(companyLoc, "-", -1)

        'assign the company name from the array
        companyName = Trim(companyLocArray(0).ToString)

        'assign the locationId from the array
        location = Trim(companyLocArray(1).ToString)

        'Try


        Dim cmd As String = "RAPID_SP_USR_LOGIN"
        Dim sqlCmd As SqlCommand = New SqlCommand(cmd, dbConn)

        sqlCmd.CommandType = Data.CommandType.StoredProcedure

        sqlCmd.Parameters.Add("@USR_NAM", Data.SqlDbType.VarChar).Value = userName

        sqlCmd.Parameters.Add("@PWD", Data.SqlDbType.VarChar).Value = password

        sqlCmd.Parameters.Add("@COMPANY_NAM", Data.SqlDbType.VarChar).Value = companyName

        sqlCmd.Parameters.Add("@LOC_ID", Data.SqlDbType.VarChar).Value = location

        dbConn.Open()

        Dim result As Integer = Convert.ToInt32(sqlCmd.ExecuteScalar())

        If (result = 0) Then

            'log the error

            sqlCmd = New SqlCommand("RAPID_SP_UPDATE_FAILED_LOGIN", dbConn)

            sqlCmd.CommandType = Data.CommandType.StoredProcedure

            sqlCmd.Parameters.Add("@USR_NAM", Data.SqlDbType.VarChar).Value = userName

            sqlCmd.Parameters.Add("@COMPANY_NAM", Data.SqlDbType.VarChar).Value = companyName

            sqlCmd.Parameters.Add("@LOC_ID", Data.SqlDbType.VarChar).Value = location

            result = Convert.ToInt32(sqlCmd.ExecuteScalar())

            'clean up
            sqlCmd.Dispose()

            'if failed login count is more than 6
            If result > 6 Then

                'send email to rapid
                sendNotification()

            End If

            'clean up
            sqlCmd.Dispose()
            dbConn.Close()

            Return False

        End If

        dbConn.Close()

        'Catch ex As Exception

        'MsgBox(ex.Message)

        'End Try

        Return True

    End Function

    Private Sub sendNotification()

        Dim Message = New MailMessage()
        Message.From = New MailAddress("superman@gmail.com")
        Message.To.Add(New MailAddress("superman@gmail.com"))
        Message.Subject = "Krypton"
        Message.Body = "I can fly"

        ' Replace SmtpMail.SmtpServer = server with the following:
        Dim smtp As New SmtpClient("smtp.hellyeah.com")
        smtp.Port = 587
        smtp.EnableSsl = True
        smtp.Credentials = New System.Net.NetworkCredential("uname", "pass")

        Try

            smtp.Send(Message)

        Catch ex As Exception

            MsgBox(ex.Message)

        End Try

    End Sub

    Protected Sub LoginButton_ServerClick(sender As Object, e As EventArgs) Handles LoginButton.Click

        If ValidateCredentials(UserName.Text, Password.Text, compLocDropDown.Text) = True Then

            ' Create user name based cookie and allow entry
            FormsAuthentication.RedirectFromLoginPage(UserName.Text, _chkPersistCookie.Checked)

            'go to the main menu
            Response.Redirect("DashBoard.aspx?field1=" & companyName & "&field2=" & location)
        Else

            Literal1.Text = "Login credentials are not correct, please try again. If you forgot your password, please contact Rapid to reset your password"
            Literal1.Visible = True
        End If

    End Sub

End Class