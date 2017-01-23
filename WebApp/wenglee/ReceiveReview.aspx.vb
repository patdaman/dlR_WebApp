Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.WebControls.Repeater

Partial Class _Default
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.Browser.Cookies Then
                lblReceiverId.Text = Request.Cookies("receiverId").Value
            Else
                Response.Write("<br />ERROR: Please enable cookies<br />")
            End If

            populateItemsScanned()
        End If
    End Sub

    Protected Sub btnScanMore_Click(sender As Object, e As System.EventArgs) Handles btnScanMore.Click
        Response.Redirect("ReceiveScan.aspx")
    End Sub

    Protected Sub btnCompleteReceiver_Click(sender As Object, e As System.EventArgs) Handles btnCompleteReceiver.Click
        ' Set status of CARTON records to Unposted Receiver
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        con.Open()

        Dim com As New SqlCommand("USER_SP_MOBILE_COMPLETE_RECEIVER", con)

        com.CommandType = CommandType.StoredProcedure

        com.Parameters.Add("@RECEIVER_NO", Data.SqlDbType.VarChar).Value = lblReceiverId.Text

        Try
            com.ExecuteScalar()
        Catch ex As Exception
            lblError.Text = "ERROR: " & ex.Message.ToString
            Exit Sub
        End Try

        com = Nothing
        con.Close()

        If Request.Browser.Cookies Then
            If (Not Request.Cookies("receiverId") Is Nothing) Then
                Dim hcMyCookie As HttpCookie
                hcMyCookie = New HttpCookie("receiverId")
                hcMyCookie.Expires = DateTime.Now.AddDays(-1D)
                Response.Cookies.Add(hcMyCookie)
            End If
        Else
            Response.Write("<br />ERROR: Please enable cookies<br />")
        End If

        ' Take user to Success screen
        Response.Redirect("ReceiveSuccess.aspx")
    End Sub

    Protected Sub btnCancelReceiver_Click(sender As Object, e As System.EventArgs) Handles btnCancelReceiver.Click
        ' Set status of CARTON records to Deleted
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        con.Open()

        Dim com As New SqlCommand("USER_SP_MOBILE_CANCEL_RECEIVER", con)

        com.CommandType = CommandType.StoredProcedure

        com.Parameters.Add("@RECEIVER_NO", Data.SqlDbType.VarChar).Value = lblReceiverId.Text

        Try
            com.ExecuteScalar()
        Catch ex As Exception
            lblError.Text = "ERROR: " & ex.Message.ToString
            Exit Sub
        End Try

        com = Nothing
        con.Close()

        If Request.Browser.Cookies Then
            If (Not Request.Cookies("receiverId") Is Nothing) Then
                Dim hcMyCookie As HttpCookie
                hcMyCookie = New HttpCookie("receiverId")
                hcMyCookie.Expires = DateTime.Now.AddDays(-1D)
                Response.Cookies.Add(hcMyCookie)
            End If
        Else
            Response.Write("<br />ERROR: Please enable cookies<br />")
        End If

        ' Take user to Cancel Success screen
        Response.Redirect("ReceiveCanceled.aspx")
    End Sub

    Protected Sub rptrItemsScanned_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptrItemsScanned.ItemCommand
        If e.CommandName = "Delete" And e.CommandArgument.ToString <> "" Then
            Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
            con.Open()

            Dim com As New SqlCommand("USER_SP_MOBILE_DELETE_CARTON", con)

            com.CommandType = CommandType.StoredProcedure

            com.Parameters.Add("@CARTON_NO", Data.SqlDbType.Int).Value = e.CommandArgument

            Try
                com.ExecuteScalar()
            Catch ex As Exception
                lblError.Text = "ERROR: " & ex.Message.ToString
                Exit Sub
            End Try

            com = Nothing
            con.Close()

            populateItemsScanned()
        ElseIf e.CommandName = "Edit" And e.CommandArgument.ToString <> "" Then
            Session("editCartonNo") = e.CommandArgument
            Response.Redirect("ReceiveScan.aspx")
        End If

        If CInt(lblLineItem.Text) > 0 Then
            btnCancelReceiver.Enabled = True
        Else
            btnCancelReceiver.Enabled = False
        End If

    End Sub

    Public Sub populateItemsScanned()
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_RETRIEVE_SCANNED_ITEMS_FOR_RECEIVER", con)

        cmd.CommandType = Data.CommandType.StoredProcedure

        cmd.Parameters.AddWithValue("RECEIVER_NO", lblReceiverId.Text)

        cmd.Connection.Open()

        rptrItemsScanned.DataSource = cmd.ExecuteReader
        rptrItemsScanned.DataBind()

        lblLineItem.Text = rptrItemsScanned.Items.Count

        cmd.Dispose()
        con.Close()

        If rptrItemsScanned.Items.Count > 0 Then
            btnCompleteReceiver.Enabled = True
            btnCancelReceiver.Enabled = True
        Else
            btnCompleteReceiver.Enabled = False
            btnCancelReceiver.Enabled = False
        End If
    End Sub

    Protected Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        If Request.UserAgent IsNot Nothing AndAlso Request.UserAgent.IndexOf("AppleWebKit", StringComparison.CurrentCultureIgnoreCase) > -1 Then
            Me.ClientTarget = "uplevel"
        End If
    End Sub
End Class
