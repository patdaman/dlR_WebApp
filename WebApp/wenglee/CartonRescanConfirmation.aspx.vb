Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.WebControls.Repeater
Imports System.Web.Services

Imports System.Collections.Generic
Imports System.Text
Imports System.Web.UI.WebControls

Partial Class CartonRescanConfirmation
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            lblBarcode.Text = Request.Cookies("barcode").Value

            
            If (Request.Cookies("orderId") IsNot Nothing) Then
                Dim orderIdCookie As HttpCookie
                orderIdCookie = New HttpCookie("orderId")
                orderIdCookie.Expires = DateTime.Now.AddDays(-1D)
                Response.Cookies.Add(orderIdCookie)
            End If

            populateCartonData()
        End If
    End Sub

    Public Sub populateCartonData()

        Try

            Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
            con.Open()

            Dim cmd As New SqlCommand("USER_SP_MOBILE_GET_SCAN_ORDER_ID", con)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("@BARCODE", Data.SqlDbType.NVarChar).Value = lblBarcode.Text
            cmd.Parameters.Add("@PICKING_NO", Data.SqlDbType.NVarChar).Value = Request.Cookies("pickingId").Value

            Dim orderId As String = cmd.ExecuteScalar()

            cmd = New SqlCommand("USER_SP_MOBILE_RETRIEVE_ORDER_LINE", con)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("@ORD_ID", Data.SqlDbType.NVarChar).Value = orderId

            Dim dr As SqlDataReader = cmd.ExecuteReader

            While dr.Read
                lblItemNo.Text = "Item No: " & dr(3)
                lblSerialNo.Text = "Ser: " & dr(4)
                lblItemDesc.Text = dr(5)
                lblQty.Text = "Qty/Lbs: " & dr(6)
            End While

        Catch ex As Exception
            Response.Write("ERROR: " & ex.Message.ToString)
        End Try

    End Sub

    Protected Sub btnKeep_Click(sender As Object, e As EventArgs) Handles btnKeep.Click

        Response.Redirect("OrderScan.aspx")

    End Sub

    Protected Sub btnRemove_Click(sender As Object, e As EventArgs) Handles btnRemove.Click

        Try

            Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
            con.Open()

            Dim cmd As New SqlCommand("USER_SP_MOBILE_GET_SCAN_ORDER_ID", con)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("@BARCODE", Data.SqlDbType.NVarChar).Value = lblBarcode.Text
            cmd.Parameters.Add("@PICKING_NO", Data.SqlDbType.NVarChar).Value = Request.Cookies("pickingId").Value

            Dim orderId As String = cmd.ExecuteScalar()

            cmd = New SqlCommand("USER_SP_MOBILE_DELETE_ORDER_ITEM", con)
            cmd.CommandType = CommandType.StoredProcedure
            cmd.Parameters.Add("@SCAN_ORD_ID", Data.SqlDbType.NVarChar).Value = orderId

            cmd.ExecuteScalar()

            cmd.Dispose()
            con.Close()

            Response.Redirect("OrderScan.aspx")

        Catch ex As Exception
            Response.Write("ERROR: " & ex.Message.ToString)
        End Try

    End Sub

    Protected Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Try

            Dim con As New SqlConnection(ConfigurationManager.ConnectionStrings("MainConnectionString").ConnectionString)
            con.Open()

            Dim cmd As New SqlCommand("USER_SP_MOBILE_GET_SCAN_ORDER_ID", con)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("@BARCODE", Data.SqlDbType.NVarChar).Value = lblBarcode.Text
            cmd.Parameters.Add("@PICKING_NO", Data.SqlDbType.NVarChar).Value = Request.Cookies("pickingId").Value

            Dim orderId As String = cmd.ExecuteScalar()

            Dim orderIdCookie As HttpCookie
            orderIdCookie = New HttpCookie("orderId", orderId)
            orderIdCookie.Expires = DateTime.Now.AddDays(999)
            Response.Cookies.Add(orderIdCookie)

            Response.Redirect("OrderScan.aspx")

        Catch ex As Exception
            Response.Write("ERROR: " & ex.Message.ToString)
        End Try
    End Sub
End Class
