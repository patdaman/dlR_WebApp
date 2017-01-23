Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.WebControls.Repeater

Partial Class CustomerConfirmation
    Inherits System.Web.UI.Page

    Dim myCommonLogic As New commonLogic

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            Try

                If Request.Browser.Cookies Then
                    lblCustomerDesc.Text = "Customer : " & Request.Cookies("pickingCustomer").Value & " - " & _
                    myCommonLogic.getCustomerDesc(Request.Cookies("pickingCustomer").Value)
                Else
                    Response.Write("<br />ERROR: Please enable cookies<br />")
                End If

            Catch ex As Exception

            End Try
        End If

        txtCustomerDesc.Attributes.Add("onFocus", "deselectDropDownList();")
        ddlCustomerList.Attributes.Add("onChange", "ddlCustomerList_SelectedIndexChanged();")

        'hide the submit buttom initially when the page is loaded
        btnSubmit.Visible = False

        'hide the customer selection list
        ddlCustomerList.Visible = False

        'hide the quick search textbox
        txtCustomerDesc.Visible = False

        lblAllCust.Visible = False

        lblQuickSearch.Visible = False


    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Request.UserAgent IsNot Nothing AndAlso Request.UserAgent.IndexOf("AppleWebKit", StringComparison.CurrentCultureIgnoreCase) > -1 Then
            Me.ClientTarget = "uplevel"
        End If
    End Sub

    Protected Sub BtnNo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles BtnNo.Click

        'change the diplay 
        lblConfirmCust.Text = "Please choose a new customer"

        lblAllCust.Visible = True

        lblQuickSearch.Visible = True

        'show the quick search tool box
        txtCustomerDesc.Visible = True

        'show the customer list
        ddlCustomerList.Visible = True

        'show the submit button
        btnSubmit.Visible = True

        'hide the yes button
        BtnNo.Visible = False

        'hide the no button
        btnYes.Visible = False

        'populate the customer list
        populateCustomerList()

    End Sub

    Protected Sub btnYes_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnYes.Click

        'submit the order
        submitOrder()

    End Sub

    Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSubmit.Click

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_SEARCH_CUSTOMER", con)

        Try

            con.Open()

            'get the customer order number from the description
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("CUST_NO_NAME", txtCustomerDesc.Text)

            Dim custNo As String = cmd.ExecuteScalar

            'save the current cookies as the current customer no
            Request.Cookies("pickingCustomer").Value = custNo

            'update the customer # on the order
            cmd = New SqlCommand("USER_SP_MOBILE_UPDATE_CUSTOMER_ON_ORDER", con)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@PICKING_NO", Data.SqlDbType.VarChar).Value = Request.Cookies("pickingId").Value
            cmd.Parameters.Add("@CUST_NO", Data.SqlDbType.VarChar).Value = custNo

            cmd.ExecuteScalar()

            'submit the order
            submitOrder()

        Catch ex As Exception
            lblError.Text = "Error: " & ex.Message.ToString

        Finally
            cmd.Dispose()
            con.Close()
        End Try

    End Sub

    Public Sub populateCustomerList()

        'get list of customers to choose from 
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_SEARCH_CUSTOMER", con)

        Try

            Dim drCustomers As SqlDataReader

            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("CUST_NO_NAME", txtCustomerDesc.Text)
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

    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As Array
        Dim customers As ArrayList = New ArrayList()

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_SEARCH_CUSTOMER", con)

        Try
            Dim drCustomers As SqlDataReader

            cmd.CommandType = Data.CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("CUST_NO_NAME", prefixText)

            cmd.Connection.Open()

            drCustomers = cmd.ExecuteReader()

            While drCustomers.Read
                customers.Add(drCustomers("NAM"))
            End While

        Catch ex As Exception

        Finally
            cmd.Dispose()
            con.Close()
        End Try

        Return customers.ToArray
    End Function

    Protected Sub txtCustomerDesc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCustomerDesc.TextChanged
        If txtCustomerDesc.Text.Length > 0 Then
            If ValidateCustomer(txtCustomerDesc.Text) Then
                ' Search for match in customer drop down list - regardless of case
                For Each customerListItem As ListItem In ddlCustomerList.Items
                    If customerListItem.Text.ToLower().Equals(txtCustomerDesc.Text.ToLower) Then
                        ddlCustomerList.SelectedIndex = -1
                        ddlCustomerList.Items.FindByText(customerListItem.Text).Selected = True
                        Exit For
                    End If
                Next
            Else
                valCustomer.IsValid = False
                valCustomer.Text = "Customer not found. Please retype or<br>use the selection list below instead."
                txtCustomerDesc.Text = ""
            End If
        End If
    End Sub

    Protected Function ValidateCustomer(ByVal customer As String) As Boolean
        'Validate that the customer exists in the database
        Dim returnValue As Boolean = False
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        Dim drCustomers As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand(
                                " SELECT COUNT(*)" & _
                                " FROM AR_CUST" & _
                                " WHERE NAM = '" & customer.Replace("'", "''") & "'", con)

        Try

            cmd.CommandType = CommandType.Text

            cmd.Connection.Open()

            drCustomers = cmd.ExecuteReader()

            drCustomers.Read()

            If drCustomers(0) > 0 Then
                returnValue = True
            Else
                returnValue = False
            End If

        Catch ex As Exception
            Response.Write("Error: " & ex.Message)
        Finally
            cmd.Dispose()
            con.Close()
        End Try

        Return returnValue
    End Function

    Public Sub submitOrder()

        'get the order number from the url field
        'Dim orderNo As String = Request.QueryString("ord_no")

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        con.Open()

        If (Request.Cookies("receiverId") IsNot Nothing) Then

            Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_COMPLETE_RECEIVER", con)

            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@RECEIVER_NO", SqlDbType.VarChar).Value = Request.Cookies("receiverId").Value

            Try

                cmd.ExecuteNonQuery()

            Catch ex As Exception
                Response.Write("Error: " & ex.Message)
            End Try

            cmd.Dispose()

            Dim receiverIdCookie As HttpCookie
            receiverIdCookie = New HttpCookie("receiverId", myCommonLogic.generateReceiverId)
            receiverIdCookie.Expires = DateTime.Now.AddDays(999)
            Response.Cookies.Add(receiverIdCookie)

        End If

        Dim orderNo As String
        Dim isEdit As String

        If (Not Request.Cookies("isEdit") Is Nothing) Then

            ' Set status of CARTON records to Unposted Order
            isEdit = Request.Cookies("isEdit").Value

            If isEdit = "Y" Then
                orderNo = Request.Cookies("orderNo").Value
            Else
            End If

            'Dim scriptKey As String = "scriptKey1"
            'Dim javaScript As String = "<script type='text/javascript'>alert('" & isEdit & "');</script>"
            'ClientScript.RegisterStartupScript(Me.GetType(), scriptKey, javaScript)

        End If

        Dim ord_no As String

        Dim com As New SqlCommand("USER_SP_MOBILE_COMPLETE_ORDER", con)

        com.CommandType = CommandType.StoredProcedure

        Try

            com.Parameters.Add("@PICKING_NO", Data.SqlDbType.VarChar).Value = Request.Cookies("pickingId").Value
            com.Parameters.Add("@CUST_NO", Data.SqlDbType.VarChar).Value = Request.Cookies("pickingCustomer").Value
            com.Parameters.Add("@IS_EDIT", Data.SqlDbType.VarChar).Value = isEdit
            com.Parameters.Add("@ORD_NO", Data.SqlDbType.VarChar).Value = orderNo
            com.Parameters.Add("@ORDER_NUMBER_SUBMITTED", Data.SqlDbType.VarChar, 15)
            com.Parameters("@ORDER_NUMBER_SUBMITTED").Direction = ParameterDirection.Output

            com.ExecuteScalar()
            ord_no = com.Parameters("@ORDER_NUMBER_SUBMITTED").Value.ToString
        Catch ex As Exception
            lblError.Text = "ERROR: " & ex.Message.ToString
            Exit Sub
        Finally
            com.Dispose()
            con.Close()
        End Try

        If (Not Request.Cookies("receiverId") Is Nothing) Then
            Dim receiverIdCookie As HttpCookie
            receiverIdCookie = New HttpCookie("receiverId")
            receiverIdCookie.Expires = DateTime.Now.AddDays(-1D)
            Response.Cookies.Add(receiverIdCookie)
        End If

        ' Take user to Success screen
        Response.Redirect("OrderSuccess.aspx?ord_no=" & ord_no)
    End Sub

End Class