<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CancelOrderConfirmation.aspx.vb" Inherits="CancelOrderConfirmation" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rapid POS Mobile WL - Order - Confirm Order Cancelation</title>
    <link href="mobile.css" rel="stylesheet" type="text/css" />
    <!-- Start iPhone -->
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-touch-fullscreen" content="yes" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <!-- End iPhone -->
    <script type="text/javascript" language="javascript">
        function submitChange() {
            __doPostBack();
        }

    </script>
    <style type="text/css">
        .auto-style1 {
            height: 20px;
        }
    </style>
</head>
<body bgcolor="#FFFFFF">
    <form id="form1" runat="server">
        <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" />

        <table border="0" cellpadding="1" cellspacing="0" bgcolor="#FFFFFF" align="center">
            <tr>
                <td align="center">
                    <asp:Image ID="imgLogo" runat="server"
                        ImageUrl="~/Images/logo_small.gif" /></td>
            </tr>
        </table>
        <table border="0" cellpadding="1" cellspacing="0" bgcolor="#FFFFFF" align="center">
            <tr>
                <td></td>
            </tr>
            <tr>
                <td align="center">
                    <h3>Confirm Order Cancellation</h3>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <h3>&nbsp;</h3>
                </td>
            </tr>
            <tr>
                <td class="td_text" style="height: 15px; text-align: center; color: red;">ARE YOU SURE YOU WISH TO CANCEL THIS ORDER?</td>
            </tr>
            <tr>
                <td style="height: 15px">&nbsp;</td>
            </tr>
            <tr>
                <td align="center">
                    <h3>
                        <asp:Button ID="btnYes" runat="server" Text="Yes" Width="290px" Height="45px" CssClass="td_text_larger" />
                    </h3>
                </td>
            </tr>
            <tr>
                <td style="height: 15px"></td>
            </tr>
            <tr>
                <td align="center" class="td_text">
                    <asp:Button ID="btnNo" runat="server" Text="No" Width="290px" Height="45px" CssClass="td_text_larger" />
                </td>
            </tr>
            <tr>
                <td style="height: 15px"></td>
                <asp:Label ID="lblPickingId" runat="server" Text="" Visible="false"></asp:Label>
            </tr>
        </table>
    </form>
</body>
</html>
