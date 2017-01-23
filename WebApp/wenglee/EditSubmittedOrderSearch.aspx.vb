Imports System.Data
Imports System.Data.SqlClient

Partial Class _Default
    Inherits System.Web.UI.Page

    Dim myCommonLogic As New commonLogic

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            PopulateCustomerList()

            ddlCustomerList.Attributes.Add("onChange", "ddlCustomerList_SelectedIndexChanged();")
            ddlCustomerOrderList.Attributes.Add("onChange", "ddlCustomerOrderList_SelectedIndexChanged();")
            ddlCustomerList.Attributes.Add("onChange", "submitChange()")
            webbarcode.Attributes.Add("onChange", "submitChange()")

            webbarcode.Focus()

        Else

            PopulateOrderList()

        End If

        lblErrMsg.Text = ""

        'If (Request.Cookies("pickingId") IsNot Nothing) Then
        'Response.Write(Request.Cookies("pickingId").Value)
        'End If

        'If (Request.Cookies("prevUrl") IsNot Nothing) Then
        'Response.Write(Request.Cookies("prevUrl").Value)
        'End If

        If Request.Browser.Cookies Then

            If (Request.Cookies("prevUrl") IsNot Nothing) Then

                'if the back button was pressed
                If Request.Cookies("prevUrl").Value = "http://localhost/wenglee/OrderScan.aspx" Then

                    'update the status back to 10 on selected order
                    updateOrder()

                End If

            End If

        Else

            Response.Write("<br />ERROR: Please enable cookies<br />")
            Exit Sub
        End If

    End Sub

    Public Sub updateOrder()

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        Dim cmd As SqlCommand = New SqlCommand("UPDATE USER_SCANNED_ORDER SET SCAN_ORD_STATUS_CODE = 10 WHERE SCAN_ORD_PICKING_NO = @PICKING_NO", con)

        Try

            con.Open()

            cmd.CommandType = Data.CommandType.Text
            cmd.Parameters.Add("@PICKING_NO", SqlDbType.NVarChar).Value = Request.Cookies("pickingId").Value

            cmd.ExecuteScalar()

        Catch ex As Exception

            Response.Write("Error: " & ex.Message)

        Finally
            cmd.Dispose()
            con.Close()
        End Try

    End Sub

    Private Sub PopulateCustomerList()

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_SEARCH_CUSTOMER", con)

        Try

            Dim drCustomers As SqlDataReader

            cmd.CommandType = Data.CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("CUST_NO_NAME", "")

            cmd.Connection.Open()

            drCustomers = cmd.ExecuteReader()

            With ddlCustomerList
                '.Items.Clear()
                .DataValueField = "CUST_NO"
                .DataTextField = "NAM"
                .DataSource = drCustomers
                .DataBind()
            End With
        Catch ex As Exception
            Response.Write("Error: " & ex.Message)
        Finally
            cmd.Dispose()
            con.Close()
        End Try
    End Sub

    Private Sub PopulateOrderList()

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        con.Open()
        Dim cmd As New SqlCommand("USER_SP_MOBILE_CHECK_FOR_UNPOSTED_ORDER_LIST", con)

        Try
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.Parameters.Add("CUST_NO", Data.SqlDbType.VarChar).Value = ddlCustomerList.SelectedValue
            cmd.Parameters.Add("@TKT_CARTON_NO", Data.SqlDbType.VarChar).Value = webbarcode.Text

            Dim drTKT As New SqlDataAdapter(cmd)
            Dim dt As New DataTable("ORDERS")
            drTKT.Fill(dt)
            Dim rowcount As Integer = dt.Rows.Count()

            With ddlCustomerOrderList
                '.Items.Clear()
                .DataValueField = "SCAN_ORD_NO"
                .DataTextField = "SCAN_ORD_NO"
                .DataSource = dt
                .DataBind()
            End With

            If rowcount = 0 Then
                valOrder.IsValid = False
                valOrder.Text = "Order not found. Please retype /<br>rescan or use the selection<br>list."
            ElseIf rowcount > 1 Then
                valOrder.IsValid = False
                valOrder.Text = rowcount.ToString + " Orders found. Please retype /<br>or use the selection<br>list."
            End If

        Catch ex As Exception
            Response.Write("Error:" & ex.Message)
        Finally
            cmd.Dispose()
            con.Close()
        End Try
    End Sub

    Protected Sub btnMainMenu_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMainMenu.Click
        Response.Redirect("Default.aspx")
    End Sub

    Protected Sub orderSelected()
        If ddlCustomerOrderList.SelectedIndex >= 0 Then
            valOrder.IsValid = True
        Else
            valOrder.IsValid = False
            valOrder.Text = "Customer not found. Please retype or<br>use the selection list below instead."
        End If
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Request.UserAgent IsNot Nothing AndAlso Request.UserAgent.IndexOf("AppleWebKit", StringComparison.CurrentCultureIgnoreCase) > -1 Then
            Me.ClientTarget = "uplevel"
        End If
    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOk.Click

        If txtCustomerOrderNo.Text <> "" Then

            Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
            con.Open()

            Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_GET_PICKING_NO", con)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@ORD_NO", Data.SqlDbType.VarChar).Value = txtCustomerOrderNo.Text
            cmd.Parameters.Add("@CUST_NO", Data.SqlDbType.VarChar).Value = ddlCustomerList.SelectedValue

            Dim pickingNo As String
            Dim dr As SqlDataReader = Nothing

            Try
                dr = cmd.ExecuteReader

                While dr.Read
                    pickingNo = dr("SCAN_ORD_PICKING_NO")
                    txtCustomerOrderNo.Text = dr("SCAN_ORD_NO")
                End While

            Catch ex As Exception
                Response.Write("Error: " & ex.Message)
            End Try

            If (dr IsNot Nothing) Then
                dr.Close()
            End If
            cmd.Dispose()
            cmd = Nothing

            cmd = New SqlCommand("USER_SP_MOBILE_EDIT_SUBMITTED_ORDER", con)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@PICKING_NO", Data.SqlDbType.VarChar).Value = pickingNo

            Try

                cmd.ExecuteScalar()

            Catch ex As Exception

                Response.Write("ERROR: " & ex.Message.ToString)
                'Response.Write("Order # or Barcode is invalid, please enter a valid value")
                Exit Sub

            End Try

            cmd = New SqlCommand("SELECT SCAN_ORD_CUST_NO FROM USER_SCANNED_ORDER WHERE SCAN_ORD_PICKING_NO = @PICKING_NO", con)
            cmd.CommandType = CommandType.Text
            cmd.Parameters.Add("@PICKING_NO", SqlDbType.NVarChar).Value = pickingNo


            Dim custNo As String
            'Dim custNo As String = ddlCustomerList.SelectedValue

            Try

                custNo = cmd.ExecuteScalar

            Catch ex As Exception

                Response.Write("ERROR: " & ex.Message.ToString)
                Exit Sub
            Finally
                cmd.Dispose()
                con.Close()
            End Try

            If Request.Browser.Cookies Then

                Dim hcMyCookie As New HttpCookie("pickingId", pickingNo)
                hcMyCookie.Expires = DateTime.Now.AddMonths(999)
                Response.Cookies.Add(hcMyCookie)

                Dim custNoCookie As New HttpCookie("pickingCustomer", custNo)
                custNoCookie.Expires = DateTime.Now.AddMonths(999)
                Response.Cookies.Add(custNoCookie)

                Dim orderNoCookie As New HttpCookie("orderNo", txtCustomerOrderNo.Text)
                orderNoCookie.Expires = DateTime.Now.AddMonths(999)
                Response.Cookies.Add(orderNoCookie)

                Dim isEdit As New HttpCookie("isEdit", "Y")
                isEdit.Expires = DateTime.Now.AddMonths(999)
                Response.Cookies.Add(isEdit)

            Else

                Response.Write("<br />ERROR: Please enable cookies<br />")
                Exit Sub

            End If

            Response.Redirect("OrderScan.aspx")
        Else
            'lblErrMsg.Text = "You must select from the list of orders to continue"
            If webbarcode.Text <> "" Or webbarcode.Text IsNot Nothing Then

                txtCustomerOrderNo.Text = webbarcode.Text
                btnOk_Click(sender, e)

            End If
        End If

    End Sub

End Class
