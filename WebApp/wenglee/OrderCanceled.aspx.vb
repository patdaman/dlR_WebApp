
Partial Class Success
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            pnlRedirect.Visible = False
            pnlRefresh.Visible = False
        End If
    End Sub

    Protected Sub btnReceive_Click(sender As Object, e As System.EventArgs) Handles btnReceive.Click
        Response.Redirect("Default.aspx")
    End Sub

    Protected Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        If Request.UserAgent IsNot Nothing AndAlso Request.UserAgent.IndexOf("AppleWebKit", StringComparison.CurrentCultureIgnoreCase) > -1 Then
            Me.ClientTarget = "uplevel"
        End If
    End Sub

    Protected Sub btnOrder_Click(sender As Object, e As EventArgs) Handles btnOrder.Click
        Response.Redirect("OrderSearchListCustomer.aspx")
    End Sub
End Class
