
Partial Class Success
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            lblMessage.Text = "Order #" & Request.QueryString("ord_no")
        End If
    End Sub

    Protected Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        If Request.UserAgent IsNot Nothing AndAlso Request.UserAgent.IndexOf("AppleWebKit", StringComparison.CurrentCultureIgnoreCase) > -1 Then
            Me.ClientTarget = "uplevel"
        End If
    End Sub

    Protected Sub btnOrder_Click(sender As Object, e As EventArgs) Handles btnOrder.Click

        Dim isEdit As New HttpCookie("isEdit", "N")
        isEdit.Expires = DateTime.Now.AddMonths(999)
        Response.Cookies.Add(isEdit)

        If Request.Browser.Cookies Then
            If (Not Request.Cookies("pickingId") Is Nothing) Then
                Dim hcMyCookie As HttpCookie
                hcMyCookie = New HttpCookie("pickingId")
                hcMyCookie.Expires = DateTime.Now.AddDays(-1D)
                Response.Cookies.Add(hcMyCookie)
            End If
        Else
            Response.Write("<br />ERROR: Please enable cookies<br />")
        End If

        Response.Redirect("OrderSearchListCustomer.aspx")
    End Sub

    Protected Sub btnReceive_Click(sender As Object, e As EventArgs) Handles btnReceive.Click

        If Request.Browser.Cookies Then
            If (Not Request.Cookies("pickingId") Is Nothing) Then
                Dim hcMyCookie As HttpCookie
                hcMyCookie = New HttpCookie("pickingId")
                hcMyCookie.Expires = DateTime.Now.AddDays(-1D)
                Response.Cookies.Add(hcMyCookie)
            End If
        Else
            Response.Write("<br />ERROR: Please enable cookies<br />")
        End If

        Response.Redirect("Default.aspx")

    End Sub
End Class
