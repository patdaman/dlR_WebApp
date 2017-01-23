Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.WebControls.Repeater
Imports System.Web.Services

Imports System.Collections.Generic
Imports System.Text
Imports System.Web.UI.WebControls

Partial Class CancelOrderConfirmation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            If Request.Browser.Cookies Then
                lblPickingId.Text = Request.Cookies("pickingId").Value
            Else
                Response.Write("<br />ERROR: Please enable cookies<br />")
            End If

        End If
    End Sub

    Protected Sub btnYes_Click(sender As Object, e As EventArgs) Handles btnYes.Click

        ' Set status of CARTON records to Deleted
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        con.Open()

        Dim com As New SqlCommand("USER_SP_MOBILE_CANCEL_ORDER", con)

        com.CommandType = CommandType.StoredProcedure

        com.Parameters.Add("@PICKING_NO", Data.SqlDbType.VarChar).Value = Request.Cookies("pickingId").Value

        Try
            com.ExecuteScalar()
        Catch ex As Exception
            Response.Write("ERROR: " & ex.Message.ToString)
            Exit Sub
        Finally
            com.Dispose()
            con.Close()
        End Try

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

        ' Take user to Cancel Success screen
        Response.Redirect("OrderCanceled.aspx")

    End Sub

    Protected Sub btnNo_Click(sender As Object, e As EventArgs) Handles btnNo.Click
        ' Take user to Cancel Success screen
        Response.Redirect("OrderReview.aspx")
    End Sub
End Class
