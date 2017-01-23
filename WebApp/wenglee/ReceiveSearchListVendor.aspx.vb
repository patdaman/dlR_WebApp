Imports System.Data
Imports System.Data.SqlClient
Imports ServiceReference1
Imports System.Web.Services

Partial Class _Default
    Inherits System.Web.UI.Page

    Dim myCommonLogic As New commonLogic

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            PopulateVendorList()

            txtVendorDesc.Attributes.Add("onFocus", "deselectDropDownList();")
            ddlVendorList.Attributes.Add("onChange", "ddlVendorList_SelectedIndexChanged();")

            If Request.Browser.Cookies Then
                If Request.Cookies("receiverId") Is Nothing Then
                    ' Create a new cookie
                    Dim hcMyCookie As New HttpCookie("receiverId", myCommonLogic.generateReceiverId)

                    ' set expiration date
                    hcMyCookie.Expires = DateTime.Now.AddMonths(999)
                    Response.Cookies.Add(hcMyCookie)
                End If

                lblReceiverId.Text = Request.Cookies("receiverId").Value
            Else
                Response.Write("<br />ERROR: Please enable cookies<br />")
            End If
        End If
    End Sub

    Private Sub PopulateVendorList()
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_SEARCH_VENDOR", con)
        Dim drVendors As SqlDataReader

        cmd.CommandType = Data.CommandType.StoredProcedure

        cmd.Parameters.AddWithValue("VEND_NO_NAME", txtVendorDesc.Text)
        cmd.Parameters.AddWithValue("@SHOW_WING_LEE_FIRST", "1")

        cmd.Connection.Open()

        drVendors = cmd.ExecuteReader()

        With ddlVendorList
            '.Items.Clear()
            .DataValueField = "VEND_NO"
            .DataTextField = "NAM"
            .DataSource = drVendors
            .DataBind()
        End With

        cmd.Dispose()
        con.Close()
    End Sub

    Protected Sub txtVendorDesc_TextChanged(sender As Object, e As System.EventArgs) Handles txtVendorDesc.TextChanged
        If txtVendorDesc.Text.Length > 0 Then
            If ValidateVendor(txtVendorDesc.Text) Then
                ' Search for match in vendor drop down list - regardless of case
                For Each vendorListItem As ListItem In ddlVendorList.Items
                    If vendorListItem.Text.ToLower().Equals(txtVendorDesc.Text.ToLower) Then
                        ddlVendorList.SelectedIndex = -1
                        ddlVendorList.Items.FindByText(vendorListItem.Text).Selected = True
                        Exit For
                    End If
                Next
            Else
                valVendor.IsValid = False
                valVendor.Text = "Vendor not found. Please retype or<br>use the selection list below instead."
                txtVendorDesc.Text = ""
            End If
        End If
    End Sub


    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()>
    Public Shared Function GetCompletionList(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As Array
        Dim vendors As ArrayList = New ArrayList()
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_SEARCH_VENDOR", con)
        Dim drVendors As SqlDataReader

        cmd.CommandType = Data.CommandType.StoredProcedure

        cmd.Parameters.AddWithValue("VEND_NO_NAME", prefixText)

        cmd.Connection.Open()

        drVendors = cmd.ExecuteReader()

        While drVendors.Read
            vendors.Add(drVendors("NAM"))
        End While

        cmd.Dispose()
        con.Close()

        Return vendors.ToArray
    End Function

    Protected Sub btnMainMenu_Click(sender As Object, e As System.EventArgs) Handles btnMainMenu.Click
        Response.Redirect("Default.aspx")
    End Sub


    Protected Sub vendorSelected()
        If ddlVendorList.SelectedIndex >= 0 Then
            valVendor.IsValid = True
        Else
            valVendor.IsValid = False
            valVendor.Text = "Vendor not found. Please retype or<br>use the selection list below instead."
        End If
    End Sub

    Protected Function ValidateVendor(ByVal vendor As String) As Boolean
        'Validate that the vendor exists in the database
        Dim returnValue As Boolean = False
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        Dim drVendors As SqlDataReader
        Dim cmd As SqlCommand = New SqlCommand(
                                " SELECT COUNT(*)" & _
                                " FROM PO_VEND" & _
                                " WHERE NAM = '" & vendor & "'", con)
        cmd.CommandType = CommandType.Text

        cmd.Connection.Open()

        drVendors = cmd.ExecuteReader()

        drVendors.Read()

        If drVendors(0) > 0 Then
            returnValue = True
        Else
            returnValue = False
        End If

        cmd.Dispose()
        con.Close()

        Return returnValue
    End Function

    Protected Sub ddlVendorList_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles ddlVendorList.SelectedIndexChanged
        txtVendorDesc.Text = ""
        vendorSelected()
    End Sub

    Protected Sub btnUseWingLee_Click(sender As Object, e As System.EventArgs) Handles btnUseWingLee.Click
        ddlVendorList.SelectedValue = System.Configuration.ConfigurationManager.AppSettings("wingLeeIdInVendorTable")
        txtVendorDesc.Text = ddlVendorList.SelectedItem.Text
        If Request.Browser.Cookies Then
            ' Create a new cookie
            Dim hcMyCookie As New HttpCookie("receiverVendor", ddlVendorList.SelectedValue)
            ' set expiration date
            hcMyCookie.Expires = DateTime.Now.AddMonths(999)
            Response.Cookies.Add(hcMyCookie)
        Else
            Response.Write("<br />ERROR: Please enable cookies<br />")
        End If

        Response.Redirect("ReceiveScan.aspx")
    End Sub

    Protected Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        If Request.UserAgent IsNot Nothing AndAlso Request.UserAgent.IndexOf("AppleWebKit", StringComparison.CurrentCultureIgnoreCase) > -1 Then
            Me.ClientTarget = "uplevel"
        End If
    End Sub


    Protected Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        If ddlVendorList.SelectedIndex >= 0 Then
            valVendor.IsValid = True

            If Request.Browser.Cookies Then
                ' Create a new cookie
                Dim hcMyCookie As New HttpCookie("receiverVendor", ddlVendorList.SelectedValue)
                ' set expiration date
                hcMyCookie.Expires = DateTime.Now.AddMonths(999)
                Response.Cookies.Add(hcMyCookie)
            Else
                Response.Write("<br />ERROR: Please enable cookies<br />")
            End If

            Response.Redirect("ReceiveScan.aspx")
        Else
            valVendor.IsValid = False
            valVendor.Text = "Vendor not found. Please retype or<br>use the selection list below instead."
        End If
    End Sub
End Class

