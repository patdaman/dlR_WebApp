Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.WebControls.Repeater
Imports System.Web.Services

Imports System.Collections.Generic
Imports System.Text
Imports System.Web.UI.WebControls

Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared


Partial Class _Default
    Inherits System.Web.UI.Page

    Dim myCommonLogic As New commonLogic

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.Browser.Cookies Then
                If (Not Request.Cookies("receiverVendor") Is Nothing) Then
                    lblVendorNum.Text = Request.Cookies("receiverVendor").Value
                    lblVendorDesc.Text = myCommonLogic.getVendorDesc(Request.Cookies("receiverVendor").Value)
                End If
            Else
                Response.Write("<br />ERROR: Please enable cookies<br />")
            End If

            txtQtyPcs.Attributes.Add("type", "number")
            txtQtyPcs.Text = "1"

            If Request.Browser.Cookies Then
                lblReceiverId.Text = Request.Cookies("receiverId").Value
            Else
                Response.Write("<br />ERROR: Please enable cookies<br />")
            End If

            webbarcode.Attributes.Add("onchange", "manualSubmitForm();")

        ElseIf webbarcode.Text > "" And btnSaveEdit.Visible = False Then
            submitForm()
        End If

        btnDoneScanning.Attributes.Add("onclick", "javascript:confirm('Are you sure you want to navigate away from this page and go to the Vendor Search?')")

        If Len(Session("editCartonNo")) > 0 Then
            editCarton(Session("editCartonNo"))
            Session("editCartonNo") = ""
        Else
            btnCancelEdit.Visible = False
            btnSaveEdit.Visible = False
            lblCancelEditSpacer.Visible = False
            webbarcode.Enabled = True
        End If

        populateItemsScanned()

        If CInt(lblLineItem.Text) > 0 Then
            btnCancelReceiver.Enabled = True
        Else
            btnCancelReceiver.Enabled = False
        End If
    End Sub

    Protected Sub btnMainMenu_Click(sender As Object, e As System.EventArgs) Handles btnMainMenu.Click
        Response.Redirect("Default.aspx")
    End Sub


    Public Sub populateItemsScanned()
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_RETRIEVE_SCANNED_ITEMS_FOR_RECEIVER", con)

        cmd.CommandType = Data.CommandType.StoredProcedure

        cmd.Parameters.AddWithValue("RECEIVER_NO", lblReceiverId.Text)

        cmd.Connection.Open()

        rptrItemsScanned.DataSource = cmd.ExecuteReader
        rptrItemsScanned.DataBind()

        lblLineItem.Text = rptrItemsScanned.Items.Count

        cmd.Dispose()
        con.Close()

        If rptrItemsScanned.Items.Count > 0 Then
            btnDoneScanning.Enabled = True
        Else
            btnDoneScanning.Enabled = False
        End If
    End Sub

    Public Sub submitForm()
        Page.Validate("Scan")

        If Page.IsValid And (commonLogic.iSWingLeeBarcode(webbarcode.Text) Or commonLogic.iSPitmanBarcode(webbarcode.Text)) Then
            Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
            con.Open()

            'Assuming all received cartons/barcodes are Wing Lee for now
            Dim com As New SqlCommand("USER_SP_MOBILE_INSERT_CARTON", con)

            com.CommandType = CommandType.StoredProcedure

            If commonLogic.iSWingLeeBarcode(webbarcode.Text) Then
                com.Parameters.Add("@VEND_ITEM_NO", Data.SqlDbType.VarChar).Value = commonLogic.getItemCodeFromWingLeeBarcode(webbarcode.Text)
            Else
                com.Parameters.Add("@VEND_ITEM_NO", Data.SqlDbType.VarChar).Value = commonLogic.getItemCodeFromPitmanBarcode(webbarcode.Text)
            End If

            com.Parameters.Add("@VEND_NO", Data.SqlDbType.VarChar).Value = lblVendorNum.Text
            com.Parameters.Add("@BARCOD", Data.SqlDbType.VarChar).Value = webbarcode.Text
            com.Parameters.Add("@PIECES_WEIGHT", Data.SqlDbType.Float).Value = txtQtyPcs.Text
            com.Parameters.Add("@RECEIVER_NO", Data.SqlDbType.VarChar).Value = lblReceiverId.Text

            Try
                com.ExecuteScalar()
            Catch ex As Exception
                valUpcCustom.Text = "ERROR: " & ex.Message.ToString

                Dim scriptKey As String = "scriptKey1"
                Dim javaScript As String = "<script type='text/javascript'>alert('" & "ERROR: " & ex.Message.ToString & ". SCAN NOT SUCCESSFUL');</script>"
                ClientScript.RegisterStartupScript(Me.GetType(), scriptKey, javaScript)

                valUpcCustom.IsValid = False
                webbarcode.BorderColor = Drawing.Color.Red
                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "failure", "errorSound.play();", True)
                webbarcode.Text = ""
                Exit Sub
            End Try

            com = Nothing
            con.Close()

            webbarcode.BorderColor = Drawing.Color.Green
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "success", "successSound.play();", True)
            txtQtyPcs.Text = "1" 'mid(webbarcode.Text, 23, 4)
            webbarcode.Text = ""
        Else
            webbarcode.BorderColor = Drawing.Color.Red
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "failure", "errorSound.play();", True)
            webbarcode.Text = ""
        End If

    End Sub

    <WebMethod()> _
    Public Shared Function validateReceiverBarcodeViaJSON(barcode As String, vendor As String) As String
        Dim outputText As String = ""
        Dim setWeightValueTo As Decimal = 0

        If barcode.Length Then
            Select Case commonLogic.getBarcodeType(barcode)
                Case 1 ' UPC
                    outputText = "ERROR: UPC. Receiving enabled for Wing Lee and Pitman barcodes only."
                Case 2 ' Wing Lee or Pitman barcode
                    ' Check whether carton has already been received
                    Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
                    Dim cmd As SqlCommand = New SqlCommand("USER_SP_MOBILE_CHECK_CARTON_ALREADY_RECEIVED", con)
                    Dim drReceived As SqlDataReader

                    cmd.CommandType = Data.CommandType.StoredProcedure

                    cmd.Parameters.AddWithValue("BARCOD", barcode)

                    cmd.Connection.Open()

                    drReceived = cmd.ExecuteReader()

                    If drReceived.Read Then
                        If drReceived(0) <> 0 Then
                            outputText = "ERROR: This carton has already been received."
                        End If
                    End If

                    cmd.Connection.Close()
                    cmd.Dispose()

                    ' Check whether carton is on file in CounterPoint
                    If Len(vendor) Then
                        cmd = New SqlCommand("USER_SP_MOBILE_CHECK_CARTON_ON_FILE", con)
                        Dim drOnFileInCp As SqlDataReader

                        cmd.CommandType = Data.CommandType.StoredProcedure

                        cmd.Parameters.AddWithValue("VEND_NO", vendor)

                        If commonLogic.iSWingLeeBarcode(barcode) Then
                            cmd.Parameters.AddWithValue("VEND_ITEM_NO", commonLogic.getItemCodeFromWingLeeBarcode(barcode))
                        Else
                            cmd.Parameters.AddWithValue("VEND_ITEM_NO", commonLogic.getItemCodeFromPitmanBarcode(barcode))
                        End If

                        cmd.Connection.Open()

                        drOnFileInCp = cmd.ExecuteReader()

                        If drOnFileInCp.Read Then
                            If drOnFileInCp(0) = 0 Then
                                outputText = "ERROR: 'This Vendor Item # is not on file in CounterPoint for Vendor # ( " & _
                                    vendor & " ). Select correct vendor, or add the item to CounterPoint then try again."
                            End If
                        End If
                        cmd.Dispose()
                        con.Close()
                    Else
                        outputText = "ERROR: Vendor must be specified."
                    End If

                    If Len(outputText) = 0 Then
                        ' If item is weighed, set qty to weight
                        Dim con1 As New SqlConnection(ConfigurationManager.ConnectionStrings("MainconnectionString").ConnectionString)
                        Dim cmd1 As SqlCommand = New SqlCommand("USER_SP_MOBILE_ITEM_IS_WEIGHED", con1)
                        Dim dr As SqlDataReader

                        cmd1.CommandType = Data.CommandType.StoredProcedure

                        If commonLogic.iSWingLeeBarcode(barcode) Then
                            cmd1.Parameters.AddWithValue("ITEM_NO", commonLogic.getItemCodeFromWingLeeBarcode(barcode))
                        Else
                            cmd1.Parameters.AddWithValue("ITEM_NO", commonLogic.getItemCodeFromPitmanBarcode(barcode))
                        End If

                        cmd1.Connection.Open()

                        dr = cmd1.ExecuteReader()

                        ' If dr.Read Then
                        'If dr(0) = 1 Then
                        setWeightValueTo = commonLogic.getWeightFromWingLeePitmanBarcode(barcode)
                        'End If
                        'End If

                        cmd1.Dispose()
                        con1.Close()
                    End If

                Case Else
                    outputText = "ERROR: Uknown barcode. Receiving enabled for Wing Lee and Pitman barcodes only."
            End Select
        Else
            outputText = "ERROR: Barcode required. Please scan or type it."
        End If

        Return outputText & "|" & setWeightValueTo
    End Function

    Protected Sub rptrItemsScanned_ItemCommand(source As Object, e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptrItemsScanned.ItemCommand
        If e.CommandName = "Delete" And e.CommandArgument.ToString <> "" Then
            Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
            con.Open()

            Dim com As New SqlCommand("USER_SP_MOBILE_DELETE_CARTON", con)

            com.CommandType = CommandType.StoredProcedure

            com.Parameters.Add("@CARTON_NO", Data.SqlDbType.Int).Value = e.CommandArgument

            Try
                com.ExecuteScalar()
            Catch ex As Exception
                valUpcCustom.Text = "ERROR: " & ex.Message.ToString
                valUpcCustom.IsValid = False
                webbarcode.BorderColor = Drawing.Color.Red
                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "failure", "errorSound.play();", True)
                webbarcode.Text = ""
                Exit Sub
            End Try

            com = Nothing
            con.Close()

            populateItemsScanned()
        ElseIf e.CommandName = "Edit" And e.CommandArgument.ToString <> "" Then
            editCarton(e.CommandArgument)
        End If

    End Sub

    Public Sub editCarton(ByVal cartonNo As Integer)
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        con.Open()

        Dim com As New SqlCommand("USER_SP_MOBILE_RETRIEVE_CARTON", con)
        Dim dr As SqlDataReader

        com.CommandType = CommandType.StoredProcedure

        com.Parameters.Add("@CARTON_NO", Data.SqlDbType.Int).Value = cartonNo

        dr = com.ExecuteReader

        If dr.Read Then
            txtCartonNo.Text = dr("CARTON_NO")
            txtQtyPcs.Text = dr("CARTON_PIECES_WEIGHT")
            webbarcode.Text = dr("CARTON_FULL_BARCODE")
            webbarcode.Enabled = False
            btnSaveEdit.Visible = True
            btnCancelEdit.Visible = True
            lblCancelEditSpacer.Visible = True
        End If

        com = Nothing
        con.Close()
    End Sub

    Protected Sub btnDoneScanning_Click(sender As Object, e As System.EventArgs) Handles btnDoneScanning.Click
        Response.Redirect("ReceiveReview.aspx")
    End Sub

    Protected Sub Page_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit
        If Request.UserAgent IsNot Nothing AndAlso Request.UserAgent.IndexOf("AppleWebKit", StringComparison.CurrentCultureIgnoreCase) > -1 Then
            Me.ClientTarget = "uplevel"
        End If
    End Sub

    Protected Sub btnSaveEdit_Click(sender As Object, e As EventArgs) Handles btnSaveEdit.Click
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        con.Open()

        Dim com As New SqlCommand("USER_SP_MOBILE_UPDATE_CARTON", con)

        com.CommandType = CommandType.StoredProcedure

        com.Parameters.Add("@CARTON_NO", Data.SqlDbType.Int).Value = txtCartonNo.Text
        com.Parameters.Add("@PIECES_WEIGHT", Data.SqlDbType.Float).Value = txtQtyPcs.Text

        Try
            com.ExecuteScalar()
        Catch ex As Exception
            valUpcCustom.Text = "ERROR: " & ex.Message.ToString
            valUpcCustom.IsValid = False
            webbarcode.BorderColor = Drawing.Color.Red
            'Page.ClientScript.RegisterStartupScript(Me.GetType(), "failure", "errorSound.play();", True)
            webbarcode.Text = ""
            Exit Sub
        End Try

        com = Nothing
        con.Close()

        txtQtyPcs.Text = "1"
        webbarcode.Text = ""
        webbarcode.Enabled = True

        populateItemsScanned()
    End Sub

    Protected Sub btnPrintRcvrReport_Click(sender As Object, e As EventArgs)
        Dim cryRpt As New ReportDocument()
        cryRpt.Load("c:\Program Files (x86)\Radiant Systems\Counterpoint\CPSQL.1\TopLevel\WENGLEE\Configuration\Reports\USER_CASE_WEIGHT_REPORT_NO_PARAM.rpt")

        Dim crtableLogoninfos As New TableLogOnInfos()
        Dim crtableLogoninfo As New TableLogOnInfo()
        Dim crConnectionInfo As New ConnectionInfo()
        Dim CrTables As Tables

        crConnectionInfo.ServerName = "WLFSERVER"
        crConnectionInfo.DatabaseName = "CPPractice"
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

    Protected Sub btnCancelReceiver_Click(sender As Object, e As EventArgs) Handles btnCancelReceiver.Click
        ' Set status of CARTON records to Deleted
        Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
        con.Open()

        Dim com As New SqlCommand("USER_SP_MOBILE_CANCEL_RECEIVER", con)

        com.CommandType = CommandType.StoredProcedure

        com.Parameters.Add("@RECEIVER_NO", Data.SqlDbType.VarChar).Value = lblReceiverId.Text

        Try
            com.ExecuteScalar()
        Catch ex As Exception
            Response.Write("ERROR: " & ex.Message.ToString)
            Exit Sub
        End Try

        com = Nothing
        con.Close()

        If Request.Browser.Cookies Then
            If (Not Request.Cookies("receiverId") Is Nothing) Then
                Dim hcMyCookie As HttpCookie
                hcMyCookie = New HttpCookie("receiverId")
                hcMyCookie.Expires = DateTime.Now.AddDays(-1D)
                Response.Cookies.Add(hcMyCookie)
            End If
        Else
            Response.Write("<br />ERROR: Please enable cookies<br />")
        End If

        ' Take user to Cancel Success screen
        Response.Redirect("ReceiveCanceled.aspx")
    End Sub
End Class
