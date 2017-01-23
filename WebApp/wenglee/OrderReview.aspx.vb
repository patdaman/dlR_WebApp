Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.WebControls.Repeater

Partial Class _Default
    Inherits System.Web.UI.Page

    Dim myCommonLogic As New commonLogic

    Dim isEdit As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.Browser.Cookies Then
                If (Not Request.Cookies("pickingCustomer") Is Nothing) Then
                    'lblPickingId.Text = Request.Cookies("pickingId").Value
                    lblCustomerNum.Text = Request.Cookies("pickingCustomer").Value
                    lblCustomerDesc.Text = myCommonLogic.getCustomerDesc(Request.Cookies("pickingCustomer").Value)

                    If (Not Request.Cookies("isEdit") Is Nothing) Then

                        If Request.Cookies("isEdit").Value = "Y" Then
                            lblPickingId.Text = Request.Cookies("orderNo").Value
                        Else
                            lblPickingId.Visible = False
                            lblOrderTxt.Visible = False
                        End If
                    End If
                End If
            Else
                Response.Write("<br />ERROR: Please enable cookies<br />")
            End If
        End If

        If (Request.Cookies("pickingId") IsNot Nothing) Then
            lblPickingNo.Text = Request.Cookies("pickingId").Value
        End If

        populateItemsScanned()

        If CInt(lblLineItem.Text) > 0 Then
            btnCancelOrder.Enabled = True
        Else
            btnCancelOrder.Enabled = False
        End If

    End Sub

    Protected Sub btnScanMore_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnScanMore.Click

        Response.Redirect("OrderScan.aspx")

    End Sub

    Protected Sub btnCompleteOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCompleteOrder.Click

        Dim custNoCookie As HttpCookie
        custNoCookie = New HttpCookie("custNo")
        custNoCookie.Expires = DateTime.Now.AddDays(999)
        Response.Cookies.Add(custNoCookie)

        Response.Redirect("CustomerConfirmation.aspx")

    End Sub

    Protected Sub btnCancelOrder_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelOrder.Click

        ' Take user to Cancel Success screen
        Response.Redirect("CancelOrderConfirmation.aspx")

    End Sub

    Protected Sub rptrItemsScanned_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptrItemsScanned.ItemCommand
        If e.CommandName = "Delete" And e.CommandArgument.ToString <> "" Then
            Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
            con.Open()

            Dim com As New SqlCommand("USER_SP_MOBILE_DELETE_ORDER_ITEM", con)

            com.CommandType = CommandType.StoredProcedure

            com.Parameters.Add("@SCAN_ORD_ID", Data.SqlDbType.Int).Value = e.CommandArgument

            Try
                com.ExecuteScalar()
            Catch ex As Exception
                lblError.Text = "ERROR: " & ex.Message.ToString
                Exit Sub
            Finally
                com.Dispose()
                con.Close()
            End Try

            com = Nothing
            con.Close()

            populateItemsScanned()
        ElseIf e.CommandName = "Edit" And e.CommandArgument.ToString <> "" Then
            Session("editOrderItemId") = e.CommandArgument
            Response.Redirect("OrderScan.aspx")
        End If

    End Sub

    Public Sub populateItemsScanned()

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_RETRIEVE_SCANNED_ITEMS_FOR_ORDER", con)

        Try
            cmd.CommandType = Data.CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("PICKING_NO", Request.Cookies("pickingId").Value)

            cmd.Connection.Open()

            rptrItemsScanned.DataSource = cmd.ExecuteReader
            rptrItemsScanned.DataBind()

            lblLineItem.Text = rptrItemsScanned.Items.Count

            If rptrItemsScanned.Items.Count > 0 Then
                btnCompleteOrder.Enabled = True
                btnCancelOrder.Enabled = True
            Else
                btnCompleteOrder.Enabled = False
                btnCancelOrder.Enabled = False
            End If

        Catch ex As Exception

        Finally
            cmd.Dispose()
            con.Close()
        End Try
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Request.UserAgent IsNot Nothing AndAlso Request.UserAgent.IndexOf("AppleWebKit", StringComparison.CurrentCultureIgnoreCase) > -1 Then
            Me.ClientTarget = "uplevel"
        End If
    End Sub

    Protected Sub btnMainMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMainMenu.Click

        Dim isEdit As String
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_UPDATE_SCAN_STATUS", con)

        If (Request.Cookies("isEdit") IsNot Nothing) Then
            isEdit = Request.Cookies("isEdit").Value
        End If

        If isEdit = "Y" Then
            con.Open()

            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@TO_STATUS", Data.SqlDbType.Int).Value = 10 'posted order in CP
            cmd.Parameters.Add("@SCAN_PICKING_NO", Data.SqlDbType.NVarChar).Value = lblPickingId.Text

            Try
                cmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write("ERROR - other: " & ex.Message.ToString)
            Finally
                cmd.Dispose()
                con.Close()
            End Try

        End If

        If (Request.Cookies("orderId") IsNot Nothing) Then
            Dim orderIdCookie As HttpCookie
            orderIdCookie = New HttpCookie("orderId")
            orderIdCookie.Expires = DateTime.Now.AddDays(-1D)
            Response.Cookies.Add(orderIdCookie)
        End If

        Response.Redirect("Default.aspx")

    End Sub

    Protected Sub btnSaveOrder_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveOrder.Click
        If Request.Browser.Cookies Then
            If (Not Request.Cookies("pickingCustomer") Is Nothing) Then
                Dim hcMyCookie As HttpCookie
                hcMyCookie = New HttpCookie("pickingCustomer")
                hcMyCookie.Expires = DateTime.Now.AddDays(-1D)
                Response.Cookies.Add(hcMyCookie)
            End If

            If (Request.Cookies("orderId") IsNot Nothing) Then
                Dim orderIdCookie As HttpCookie
                orderIdCookie = New HttpCookie("orderId")
                orderIdCookie.Expires = DateTime.Now.AddDays(-1D)
                Response.Cookies.Add(orderIdCookie)
            End If

            If (Not Request.Cookies("pickingId") Is Nothing) Then
                Dim hcMyCookie2 As HttpCookie
                hcMyCookie2 = New HttpCookie("pickingId")
                hcMyCookie2.Expires = DateTime.Now.AddDays(-1D)
                Response.Cookies.Add(hcMyCookie2)
            End If
        Else
            Response.Write("<br />ERROR: Please enable cookies<br />")
        End If

        Response.Write("<script type='text/javascript'>alert('Order has been saved for later. To retrieve and continue with this order," & _
                                           " simply click New Order then select this customer from the customer search screen.');" & _
                                           " window.location.href = 'Default.aspx';</script>")
    End Sub
End Class
