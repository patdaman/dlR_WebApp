Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.WebControls.Repeater
Imports System.Web.Services

Imports System.Collections.Generic
Imports System.Text
Imports System.Web.UI.WebControls

Partial Class _Default
    Inherits System.Web.UI.Page

    Dim myCommonLogic As New commonLogic

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.Browser.Cookies Then
                If (Not Request.Cookies("pickingCustomer") Is Nothing) Then
                    lblCustomerNum.Text = Request.Cookies("pickingCustomer").Value
                    lblCustomerDesc.Text = myCommonLogic.getCustomerDesc(Request.Cookies("pickingCustomer").Value)
                End If

                If (Not Request.Cookies("orderId") Is Nothing) Then

                    Session("editOrderItemId") = Request.Cookies("orderId").Value

                End If

                'this cookie is use to determine if the back button was pressed
                If (Request.Cookies("prevUrl") IsNot Nothing) Then

                    Response.Cookies("prevUrl").Value = HttpContext.Current.Request.Url.AbsoluteUri.ToString

                End If

                Dim receiverIdCookie As HttpCookie
                receiverIdCookie = New HttpCookie("receiverId", myCommonLogic.generateReceiverId)
                receiverIdCookie.Expires = DateTime.Now.AddDays(999)
                Response.Cookies.Add(receiverIdCookie)

            Else
                Response.Write("<br />ERROR: Please enable cookies<br />")
            End If

            txtQtyLbs.Attributes.Add("type", "number")
            txtQtyLbs.Text = "1"

            If Request.Browser.Cookies Then
                lblPickingId.Text = Request.Cookies("pickingId").Value
            Else
                Response.Write("<br />ERROR: Please enable cookies<br />")
            End If

            webbarcode.Attributes.Add("onchange", "manualSubmitForm();")

            txtSerialNo.Attributes.Add("onchange", "serialSubmitForm();")

            If (Not Request.Cookies("isEdit") Is Nothing) Then

                Dim isEdit As String = Request.Cookies("isEdit").Value

                'cannot save an order edit
                If isEdit = "Y" Then

                    btnSaveOrder.Enabled = False
                    btnSaveOrder.Visible = False

                End If

            End If

        End If

        If webbarcode.Text > "" And btnSaveEdit.Visible = False Then
            submitForm()
        Else
            txtItemNo.Enabled = False
            'valItemNo.Enabled = False
            valReqItemNo.Enabled = False
            'txtLbs.Enabled = False
        End If

        If Len(lblCustomerNum.Text = Request.Cookies("pickingCustomer").Value) Then
            valCustomerNumber.IsValid = True
        Else
            valCustomerNumber.IsValid = False
        End If

        If Len(Session("editOrderItemId")) > 0 Then
            editOrderLine(Session("editOrderItemId"))
            Session("editOrderItemId") = ""
        Else
            btnCancelEdit.Visible = False
            btnSaveEdit.Visible = False
            lblCancelEditSpacer.Visible = False
            'btnNextScan.Visible = True
            webbarcode.Enabled = True
        End If

        populateItemsScanned()

        If CInt(lblLineItem.Text) > 0 Then
            btnCancelOrder.Enabled = True
        Else
            btnCancelOrder.Enabled = False
        End If

        If (Not String.IsNullOrEmpty(txtSerialNo.Text)) Then
            insertOrderItemBySerial()
            populateItemsScanned()
        End If

        'disable cancel edit button **cindy request**
        btnCancelEdit.Visible = False
    End Sub

    <WebMethod()> _
    Public Shared Function validatePickingBarcodeViaJSON(ByVal barcode As String, ByVal lbs As String, ByVal itemNo As String) As String
        Dim outputText As String = ""
        Dim needsQtyPcs As Integer = 0
        Dim needsLbs As Integer = 0
        Dim needsItemNo As Integer = 0
        Dim isOnExistingOrder As String = "0"

        If barcode.Length Then

            ' Check whether weight is required
            Dim con1 As New SqlConnection(ConfigurationManager.ConnectionStrings("MainconnectionString").ConnectionString)
            Dim cmd1 As SqlCommand = New SqlCommand("USER_SP_MOBILE_CHECK_WEIGHT_REQUIRED", con1)
            Dim dr As SqlDataReader

            Try
                Select Case commonLogic.getBarcodeType(barcode)
                    Case 0 ' 3rd party barcode
                        ' Require quantity or lbs depending on stocking. Also require Item Number entry - no lookup
                        needsItemNo = 1

                        cmd1.CommandType = Data.CommandType.StoredProcedure

                        cmd1.Parameters.AddWithValue("BARCOD", barcode)
                        cmd1.Parameters.AddWithValue("IS_WING_LEE_BARCOD", 0)

                        cmd1.Connection.Open()

                        dr = cmd1.ExecuteReader()

                        If dr.Read Then
                            If dr(0) = 1 Then
                                needsLbs = 1
                                needsQtyPcs = 0
                            Else
                                needsLbs = 0
                                needsQtyPcs = 1
                            End If
                        End If

                        cmd1.Dispose()
                        con1.Close()

                    Case 1 ' UPC
                        ' No need to enter weight or item number
                        needsQtyPcs = 1

                    Case 2 ' Wing Lee or Pitman barcode
                        ' No need to enter weight or item number
                        needsQtyPcs = 0
                        needsItemNo = 0

                    Case Else ' Invalid barcode
                        outputText = "ERROR: Invalid barcode. "

                End Select

                If Len(outputText) = 0 Then
                    ' CHECK WHETHER ITEM IS ON FILE IN COUNTERPOINT and CHECK WHETHER ITEN NUMBER AND BARCODE MATCH UP
                    Dim con2 As New SqlConnection(ConfigurationManager.ConnectionStrings("MainconnectionString").ConnectionString)
                    con2.Open()

                    Dim cmd2 As New SqlCommand("USER_SP_MOBILE_VERIFY_BARCODE_AND_ITEM_NO", con2)

                    cmd2.CommandType = Data.CommandType.StoredProcedure

                    cmd2.Parameters.AddWithValue("BARCOD", barcode)
                    cmd2.Parameters.AddWithValue("ITEM_NO", itemNo)

                    If commonLogic.iSWingLeeBarcode(barcode) Or commonLogic.iSPitmanBarcode(barcode) Then
                        cmd2.Parameters.AddWithValue("IS_WING_LEE_BARCOD", 1)
                    Else
                        cmd2.Parameters.AddWithValue("IS_WING_LEE_BARCOD", 0)
                    End If

                    cmd2.Parameters.AddWithValue("ITEM_NO_REQUIRED", needsItemNo)

                    Try
                        isOnExistingOrder = cmd2.ExecuteScalar()
                    Catch ex As Exception
                        outputText = "ERROR: " & ex.Message.ToString
                    Finally
                        cmd2.Dispose()
                        con2.Close()
                    End Try

                End If
            Catch ex As Exception
                outputText = "ERROR - other: " & ex.Message.ToString
            Finally
                cmd1.Dispose()
                con1.Close()
            End Try
        Else
            outputText = "ERROR: Barcode required. Please scan or type it."
        End If

        Return outputText & "|" & needsItemNo & "|" & needsLbs & "|" & needsQtyPcs & "|"

    End Function

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

    Public Sub populateItemsScanned()
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        Dim cmd1 As SqlCommand = New SqlCommand("USER_SP_MOBILE_RETRIEVE_SCANNED_ITEMS_FOR_ORDER", con)

        Try

            cmd1.CommandType = Data.CommandType.StoredProcedure

            cmd1.Parameters.AddWithValue("PICKING_NO", lblPickingId.Text)

            cmd1.Connection.Open()

            rptrItemsScanned.DataSource = cmd1.ExecuteReader
            rptrItemsScanned.DataBind()

            lblLineItem.Text = rptrItemsScanned.Items.Count

            If rptrItemsScanned.Items.Count > 0 Then
                btnDoneScanning.Enabled = True
                btnSaveOrder.Enabled = True
            Else
                btnDoneScanning.Enabled = False
                btnSaveOrder.Enabled = False
            End If

        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            cmd1.Dispose()
            con.Close()
        End Try
    End Sub

    Public Sub insertOrderItemBySerial()

        Dim barcode As String
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        Dim cmd As New SqlCommand("USER_SP_MOBILE_GET_BARCODE_FROM_SERIAL", con)

        Try
            con.Open()
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@SER_NO", SqlDbType.NVarChar).Value = txtSerialNo.Text

            barcode = cmd.ExecuteScalar()
        Catch ex As Exception
            Response.Write("Error:" & ex.Message)
        Finally
            cmd.Dispose()
            con.Close()
        End Try

        Dim com As New SqlCommand("USER_SP_MOBILE_INSERT_ORDER_ITEM", con)
        com.CommandType = CommandType.StoredProcedure
        con.Open()

        If commonLogic.iSWingLeeBarcode(barcode) Or commonLogic.iSPitmanBarcode(barcode) Then
            com.Parameters.Add("@IS_WING_LEE_BARCOD", Data.SqlDbType.Int).Value = 1
        Else
            com.Parameters.Add("@IS_WING_LEE_BARCOD", Data.SqlDbType.Int).Value = 0
            If txtItemNo.Enabled Then
                com.Parameters.Add("@ITEM_NO", Data.SqlDbType.VarChar).Value = txtItemNo.Text
            End If
        End If

        If txtQtyLbs.Text.Length Then
            com.Parameters.Add("@PIECES_WEIGHT", Data.SqlDbType.Float).Value = txtQtyLbs.Text
        Else
            com.Parameters.Add("@PIECES_WEIGHT", Data.SqlDbType.Float).Value = 1
        End If

        com.Parameters.Add("@CUST_NO", Data.SqlDbType.VarChar).Value = lblCustomerNum.Text
        com.Parameters.Add("@BARCOD", Data.SqlDbType.VarChar).Value = barcode
        com.Parameters.Add("@PICKING_NO", Data.SqlDbType.VarChar).Value = lblPickingId.Text

        If (Request.Cookies("receiverId") IsNot Nothing) Then
            com.Parameters.Add("@REC_ID", Data.SqlDbType.VarChar).Value = Request.Cookies("receiverId").Value
        End If

        Try
            Dim isOnExistingOrder = com.ExecuteScalar()

            If isOnExistingOrder = "Y" Then

                Dim barcodeCookie As HttpCookie
                barcodeCookie = New HttpCookie("barcode", barcode)
                barcodeCookie.Expires = DateTime.Now.AddDays(999)
                Response.Cookies.Add(barcodeCookie)

                Response.Redirect("CartonRescanConfirmation.aspx")
            End If
        Catch ex As Exception
            valUpcCustom.Text = "ERROR: " & ex.Message.ToString

            Dim scriptKey As String = "scriptKey1"
            Dim javaScript As String = "<script type='text/javascript'>alert('" & "ERROR: " & ex.Message.ToString & ". SCAN NOT SUCCESSFUL');</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), scriptKey, javaScript)

            valUpcCustom.IsValid = False
            webbarcode.BorderColor = Drawing.Color.Red
            txtQtyLbs.Text = "1"
            webbarcode.Text = ""
            txtItemNo.Text = ""
            txtSerialNo.Text = ""
            Exit Sub
        End Try

        com = Nothing
        con.Close()

        webbarcode.BorderColor = Drawing.Color.Green
        txtQtyLbs.Text = "1"
        webbarcode.Text = ""
        txtItemNo.Text = ""
        txtSerialNo.Text = ""
        'pnlThirdPartyUpc.Visible = False
        valReqItemNo.Enabled = False

        'valItemNo.Enabled = False
        'Else
        'webbarcode.BorderColor = Drawing.Color.Red
        'webbarcode.Text = ""
        'End If



    End Sub

    Public Sub submitForm()
        'Page.Validate("Scan")

        'If Page.IsValid Then
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        con.Open()

        Dim com As New SqlCommand("USER_SP_MOBILE_INSERT_ORDER_ITEM", con)

        com.CommandType = CommandType.StoredProcedure

        If commonLogic.iSWingLeeBarcode(webbarcode.Text) Or commonLogic.iSPitmanBarcode(webbarcode.Text) Then
            com.Parameters.Add("@IS_WING_LEE_BARCOD", Data.SqlDbType.Int).Value = 1
        Else
            com.Parameters.Add("@IS_WING_LEE_BARCOD", Data.SqlDbType.Int).Value = 0
            If txtItemNo.Enabled Then
                com.Parameters.Add("@ITEM_NO", Data.SqlDbType.VarChar).Value = txtItemNo.Text
            End If
        End If

        If txtQtyLbs.Text.Length Then
            com.Parameters.Add("@PIECES_WEIGHT", Data.SqlDbType.Float).Value = txtQtyLbs.Text
        Else
            com.Parameters.Add("@PIECES_WEIGHT", Data.SqlDbType.Float).Value = 1
        End If

        com.Parameters.Add("@CUST_NO", Data.SqlDbType.VarChar).Value = lblCustomerNum.Text
        com.Parameters.Add("@BARCOD", Data.SqlDbType.VarChar).Value = webbarcode.Text
        com.Parameters.Add("@PICKING_NO", Data.SqlDbType.VarChar).Value = lblPickingId.Text

        If (Request.Cookies("receiverId") IsNot Nothing) Then
            com.Parameters.Add("@REC_ID", Data.SqlDbType.VarChar).Value = Request.Cookies("receiverId").Value
        End If

        Try
            Dim isOnExistingOrder = com.ExecuteScalar()

            If isOnExistingOrder = "Y" Then

                Dim barcodeCookie As HttpCookie
                barcodeCookie = New HttpCookie("barcode", webbarcode.Text)
                barcodeCookie.Expires = DateTime.Now.AddDays(999)
                Response.Cookies.Add(barcodeCookie)

                Response.Redirect("CartonRescanConfirmation.aspx")
            End If
        Catch ex As Exception
            valUpcCustom.Text = "ERROR: " & ex.Message.ToString

            Dim scriptKey As String = "scriptKey1"
            Dim javaScript As String = "<script type='text/javascript'>alert('" & "ERROR: " & ex.Message.ToString & ". SCAN NOT SUCCESSFUL');</script>"
            ClientScript.RegisterStartupScript(Me.GetType(), scriptKey, javaScript)

            valUpcCustom.IsValid = False
            webbarcode.BorderColor = Drawing.Color.Red
            txtQtyLbs.Text = "1"
            webbarcode.Text = ""
            txtItemNo.Text = ""
            Exit Sub
        End Try

        com = Nothing
        con.Close()

        webbarcode.BorderColor = Drawing.Color.Green
        txtQtyLbs.Text = "1"
        webbarcode.Text = ""
        txtItemNo.Text = ""
        'pnlThirdPartyUpc.Visible = False
        valReqItemNo.Enabled = False
        'valItemNo.Enabled = False
        'Else
        'webbarcode.BorderColor = Drawing.Color.Red
        'webbarcode.Text = ""
        'End If

    End Sub

    Protected Sub rptrItemsScanned_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptrItemsScanned.ItemCommand

        'txtSerialNo.Enabled = False

        If e.CommandName = "Delete" And e.CommandArgument.ToString <> "" Then
            Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
            con.Open()

            Dim com As New SqlCommand("USER_SP_MOBILE_DELETE_ORDER_ITEM", con)

            com.CommandType = CommandType.StoredProcedure

            com.Parameters.Add("@SCAN_ORD_ID", Data.SqlDbType.Int).Value = e.CommandArgument

            Try
                com.ExecuteScalar()
            Catch ex As Exception
                valUpcCustom.Text = "ERROR: " & ex.Message.ToString
                valUpcCustom.IsValid = False
                webbarcode.BorderColor = Drawing.Color.Red
                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "failure", "errorSound.play();", True)
                webbarcode.Text = ""
                Exit Sub
            Finally
                com.Dispose()
                con.Close()
            End Try

            populateItemsScanned()

            If rptrItemsScanned.Items.Count = 0 Then
                btnCancelOrder.Enabled = False
            End If
        ElseIf e.CommandName = "Edit" And e.CommandArgument.ToString <> "" Then
            editOrderLine(e.CommandArgument)

            If rptrItemsScanned.Items.Count > 0 Then

                For i As Integer = 0 To rptrItemsScanned.Items.Count - 1

                    Dim lblEdit As LinkButton = rptrItemsScanned.Items.Item(i).FindControl("lbtnEdit")
                    lblEdit.Visible = False

                    Dim lblDelete As LinkButton = rptrItemsScanned.Items.Item(i).FindControl("lbtnDelete")
                    lblDelete.Visible = False

                Next
            End If

        End If

    End Sub

    Protected Sub btnDoneScanning_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDoneScanning.Click

        If (Request.Cookies("orderId") IsNot Nothing) Then
            Dim orderIdCookie As HttpCookie
            orderIdCookie = New HttpCookie("orderId")
            orderIdCookie.Expires = DateTime.Now.AddDays(-1D)
            Response.Cookies.Add(orderIdCookie)
        End If

        Response.Redirect("OrderReview.aspx")
    End Sub

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Request.UserAgent IsNot Nothing AndAlso Request.UserAgent.IndexOf("AppleWebKit", StringComparison.CurrentCultureIgnoreCase) > -1 Then
            Me.ClientTarget = "uplevel"
        End If
    End Sub

    Public Sub editOrderLine(ByVal orderLine As Integer)

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        con.Open()

        Dim com As New SqlCommand("USER_SP_MOBILE_RETRIEVE_ORDER_LINE", con)

        Try

            Dim dr As SqlDataReader

            com.CommandType = CommandType.StoredProcedure

            com.Parameters.Add("@ORD_ID", Data.SqlDbType.Int).Value = orderLine

            dr = com.ExecuteReader

            If dr.Read Then
                txtOrderId.Text = dr("SCAN_ORD_ID")
                txtQtyLbs.Text = dr("SCAN_ORD_PIECES_WEIGHT")
                webbarcode.Text = dr("SCAN_ORD_BARCOD")
                txtItemNo.Text = dr("SCAN_ORD_ITEM_NO")
                webbarcode.Enabled = False
                txtItemNo.Enabled = False

                btnSaveEdit.Visible = True
                btnCancelEdit.Visible = True
                lblCancelEditSpacer.Visible = True
                btnNextScan.Visible = False
            End If

            'disable cancel edit button **cindy request**
            btnCancelEdit.Visible = False

            'hide page buttons when editing an item
            btnDoneScanning.Visible = False
            btnSaveOrder.Visible = False
            btnCancelOrder.Visible = False
            btnMainMenu.Visible = False

        Catch ex As Exception
            Response.Write(ex.Message)
        Finally
            com.Dispose()
            con.Close()
        End Try
    End Sub

    Protected Sub btnSaveEdit_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveEdit.Click

        btnDoneScanning.Visible = True
        btnSaveOrder.Visible = True
        'btnCancelOrder.Visible = True
        btnMainMenu.Visible = True

        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        con.Open()

        Dim com As New SqlCommand("USER_SP_MOBILE_UPDATE_ORDER_LINE", con)

        com.CommandType = CommandType.StoredProcedure

        com.Parameters.Add("@ORD_ID", Data.SqlDbType.Int).Value = txtOrderId.Text
        com.Parameters.Add("@PIECES_WEIGHT", Data.SqlDbType.Float).Value = txtQtyLbs.Text

        Try
            com.ExecuteScalar()
        Catch ex As Exception
            valUpcCustom.Text = "ERROR: " & ex.Message.ToString
            valUpcCustom.IsValid = False
            webbarcode.BorderColor = Drawing.Color.Red
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "failure", "errorSound.play();", True)
            webbarcode.Text = ""
            Exit Sub
        Finally
            com.Dispose()
            con.Close()
        End Try

        txtQtyLbs.Text = "1"
        webbarcode.Text = ""
        txtItemNo.Text = ""
        webbarcode.Enabled = True
        'txtSerialNo.Enabled = True
        'txtItemNo.Enabled = True

        populateItemsScanned()


    End Sub

    Protected Sub btnCancelOrder_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelOrder.Click
        ' Set status of CARTON records to Deleted

        If rptrItemsScanned.Items.Count Then

            Response.Redirect("CancelOrderConfirmation.aspx")

        Else
            Response.Write("No order items to cancel.")

        End If
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
