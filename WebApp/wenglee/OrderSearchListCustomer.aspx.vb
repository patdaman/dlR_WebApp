Imports System.Data
Imports System.Data.SqlClient

Partial Class _Default
    Inherits System.Web.UI.Page

    Dim myCommonLogic As New commonLogic

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            PopulateCustomerList()

            txtCustomerDesc.Attributes.Add("onFocus", "deselectDropDownList();")
            ddlCustomerList.Attributes.Add("onChange", "ddlCustomerList_SelectedIndexChanged();")
        End If
    End Sub

    Private Sub PopulateCustomerList()
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_SEARCH_CUSTOMER", con)
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

        cmd.Dispose()
        con.Close()
    End Sub

    Protected Sub txtCustomerDesc_TextChanged(sender As Object, e As System.EventArgs) Handles txtCustomerDesc.TextChanged
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


    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As Array
        Dim customers As ArrayList = New ArrayList()
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_SEARCH_CUSTOMER", con)
        Dim drCustomers As SqlDataReader

        cmd.CommandType = Data.CommandType.StoredProcedure

        cmd.Parameters.AddWithValue("CUST_NO_NAME", prefixText)

        cmd.Connection.Open()

        drCustomers = cmd.ExecuteReader()

        While drCustomers.Read
            customers.Add(drCustomers("NAM"))
        End While

        cmd.Dispose()
        con.Close()

        Return customers.ToArray
    End Function

    Protected Sub btnMainMenu_Click(sender As Object, e As System.EventArgs) Handles btnMainMenu.Click
        Response.Redirect("Default.aspx")
    End Sub


    Protected Sub customerSelected()
        If ddlCustomerList.SelectedIndex >= 0 Then
            valCustomer.IsValid = True
        Else
            valCustomer.IsValid = False
            valCustomer.Text = "Customer not found. Please retype or<br>use the selection list below instead."
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
        cmd.CommandType = CommandType.Text

        cmd.Connection.Open()

        drCustomers = cmd.ExecuteReader()

        drCustomers.Read()

        If drCustomers(0) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If

        cmd.Dispose()
        con.Close()

        Return returnValue
    End Function

    'Protected Sub ddlCustomerList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlCustomerList.SelectedIndexChanged
    '    txtCustomerDesc.Text = ddlCustomerList.SelectedItem.Text
    'End Sub

    Protected Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        If Request.UserAgent IsNot Nothing AndAlso Request.UserAgent.IndexOf("AppleWebKit", StringComparison.CurrentCultureIgnoreCase) > -1 Then
            Me.ClientTarget = "uplevel"
        End If
    End Sub

    Protected Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        If ddlCustomerList.SelectedIndex >= 0 Then
            valCustomer.IsValid = True

            If Request.Browser.Cookies Then
                ' Create a new cookie
                Dim hcMyCookie As New HttpCookie("pickingCustomer", ddlCustomerList.SelectedValue)
                ' set expiration date
                hcMyCookie.Expires = DateTime.Now.AddMonths(999)
                Response.Cookies.Add(hcMyCookie)
            Else
                Response.Write("<br />ERROR: Please enable cookies<br />")
                Exit Sub
            End If

            'Check whether there is an unsubmitted (saved) picking for specified customer. If so, grab the existing picking number. Otherwise, create a new picking number.
            Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
            Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_CHECK_FOR_UNSUBMITTED_ORDER", con)
            Dim dr As SqlDataReader

            cmd.CommandType = Data.CommandType.StoredProcedure

            cmd.Parameters.AddWithValue("CUST_NO", ddlCustomerList.SelectedValue)

            cmd.Connection.Open()

            dr = cmd.ExecuteReader()

            If dr.Read Then
                If Len(dr("PICKING_NO")) > 0 Then
                    Dim hcMyCookie As New HttpCookie("pickingId", dr("PICKING_NO"))
                    hcMyCookie.Expires = DateTime.Now.AddMonths(999)
                    Response.Cookies.Add(hcMyCookie)
                Else
                    Dim hcMyCookie As New HttpCookie("pickingId", myCommonLogic.generatePickingId)
                    hcMyCookie.Expires = DateTime.Now.AddMonths(999)
                    Response.Cookies.Add(hcMyCookie)
                End If

            End If

            cmd.Dispose()
            con.Close()

            Response.Redirect("OrderScan.aspx")
        Else
            valCustomer.IsValid = False
            valCustomer.Text = "Customer not found. Please retype or<br>use the selection list below instead."
        End If
    End Sub

End Class
