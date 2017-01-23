Imports System.Data
Imports System.Data.SqlClient

Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared

Partial Class _Default
    Inherits System.Web.UI.Page
    Dim myCommonLogic As New commonLogic

    Protected Sub btnReceive_Click(sender As Object, e As System.EventArgs) Handles btnReceive.Click
        Response.Redirect("ReceiveSearchListVendor.aspx")
    End Sub

    Protected Sub btnNewOrder_Click(sender As Object, e As System.EventArgs) Handles btnNewOrder.Click
        Response.Redirect("OrderSearchListCustomer.aspx")
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            lblUnsubmittedReceiver.Visible = False
            lblUnsubmittedOrder.Visible = False
            btnSavedOrder.Enabled = False

            If Request.Browser.Cookies Then
                If (Not Request.Cookies("receiverId") Is Nothing) Then
                    If myCommonLogic.receiverHasUncommittedScans(Request.Cookies("receiverId").Value) Then
                        lblUnsubmittedReceiver.Visible = True
                    Else
                        lblUnsubmittedReceiver.Visible = False

                        Dim hcMyCookie As HttpCookie
                        hcMyCookie = New HttpCookie("receiverId")
                        hcMyCookie.Expires = DateTime.Now.AddDays(-1D)
                        Response.Cookies.Add(hcMyCookie)
                    End If
                End If

                If (Not Request.Cookies("pickingId") Is Nothing) Then
                    If myCommonLogic.orderHasUncommittedScans(Request.Cookies("pickingId").Value) Then
                        lblUnsubmittedOrder.Visible = True
                    Else
                        lblUnsubmittedOrder.Visible = False

                        Dim hcMyCookie2 As HttpCookie
                        hcMyCookie2 = New HttpCookie("pickingId")
                        hcMyCookie2.Expires = DateTime.Now.AddDays(-1D)
                        Response.Cookies.Add(hcMyCookie2)
                    End If
                End If

                'this cookie is used to determine if and order is being edited
                If (Request.Cookies("isEdit") IsNot Nothing) Then
                    Dim isEdit As New HttpCookie("isEdit", "N")
                    isEdit.Expires = DateTime.Now.AddMonths(999)
                    Response.Cookies.Add(isEdit)
                End If

                Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
                Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_SAVED_ORDERS_EXIST", con)
                Dim drSavedOrdersExist As SqlDataReader

                cmd.CommandType = Data.CommandType.StoredProcedure

                cmd.Connection.Open()

                drSavedOrdersExist = cmd.ExecuteReader()

                If drSavedOrdersExist.Read Then
                    If drSavedOrdersExist(0) <> 0 Then
                        btnSavedOrder.Enabled = True
                    End If
                End If

                cmd.Connection.Close()
                cmd.Dispose()
                con.Close()
            Else
                Response.Write("<br />ERROR: Please enable cookies<br />")
            End If

            'If Not System.Web.HttpContext.Current.Session("receiverId") Is Nothing Then
            '    If myCommonLogic.receiverHasUncommittedScans(System.Web.HttpContext.Current.Session("receiverId")) Then
            '        lblUnsubmittedScans.Visible = True
            '    Else
            '        lblUnsubmittedScans.Visible = False

            '        ' Clear Receiver Number session variable
            '        System.Web.HttpContext.Current.Session("receiverId") = Nothing
            '    End If
            'End If
        End If

        'this cookie is used to determine if the back button is pressed
        If (Request.Cookies("prevUrl") IsNot Nothing) Then

            Response.Cookies("prevUrl").Value = HttpContext.Current.Request.Url.AbsoluteUri.ToString

        Else

            Dim pageUrlCookie As New HttpCookie("prevUrl", HttpContext.Current.Request.Url.AbsoluteUri.ToString)
            pageUrlCookie.Expires = DateTime.Now.AddMonths(999)
            Response.Cookies.Add(pageUrlCookie)

        End If

    End Sub

    Protected Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        If Request.UserAgent IsNot Nothing AndAlso Request.UserAgent.IndexOf("AppleWebKit", StringComparison.CurrentCultureIgnoreCase) > -1 Then
            Me.ClientTarget = "uplevel"
        End If
    End Sub

    Protected Sub btnSavedOrder_Click(sender As Object, e As EventArgs) Handles btnSavedOrder.Click
        Response.Redirect("OrderSearchListOpenOrder.aspx")
    End Sub

    Protected Sub btnEditSubmittedOrder_Click(sender As Object, e As System.EventArgs) Handles btnEditSubmittedOrder.Click
        Response.Redirect("EditSubmittedOrderSearch.aspx")
    End Sub

    Protected Sub btnItemLookup_Click(sender As Object, e As EventArgs)
        Dim cryRpt As New ReportDocument()
        cryRpt.Load("c:\Program Files (x86)\Radiant Systems\Counterpoint\CPSQL.1\TopLevel\WENGLEE\Configuration\Reports\USER_CASE_WEIGHT_REPORT_NO_PARAM.rpt")

        Dim crtableLogoninfos As New TableLogOnInfos()
        Dim crtableLogoninfo As New TableLogOnInfo()
        Dim crConnectionInfo As New ConnectionInfo()
        Dim CrTables As Tables

        crConnectionInfo.ServerName = "WLFSERVER"
        crConnectionInfo.DatabaseName = "WENGLEE"
        crConnectionInfo.UserID = "Reports"
        crConnectionInfo.Password = "WlfAdmin!"

        CrTables = cryRpt.Database.Tables
        For Each CrTable As CrystalDecisions.CrystalReports.Engine.Table In CrTables
            crtableLogoninfo = CrTable.LogOnInfo
            crtableLogoninfo.ConnectionInfo = crConnectionInfo
            CrTable.ApplyLogOnInfo(crtableLogoninfo)
        Next

        cryRpt.Refresh()
        cryRpt.PrintToPrinter(1, False, 0, 0)
    End Sub

End Class
