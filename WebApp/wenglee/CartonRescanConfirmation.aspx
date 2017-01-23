<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CartonRescanConfirmation.aspx.vb" Inherits="CartonRescanConfirmation" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rapid POS Mobile WL - Order - Search Open Orders</title>
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
                    <h3>Confirm Scan</h3>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <h3>&nbsp;</h3>
                </td>
            </tr>
            <tr>
                <td class="td_text" style="height: 15px; text-align: center; color: red;">ALREADY ON CURRENT ORDER.
                    <br />
                    DO YOU WANT TO..?</td>
            </tr>
            <tr>
                <td align="center">
                    <h3>&nbsp;</h3>
                </td>
            </tr>
            <tr>
                <td style="font: bold; font-size: large; text-align: center">Barcode:
                    <br />
                    <asp:Label ID="lblBarcode" runat="server" Font-Size="10px"></asp:Label>

                </td>
            </tr>
            <tr>
                <td style="height: 15px">&nbsp;</td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:Label ID="lblItemDesc" runat="server" CssClass="td_text_larger"></asp:Label>

                </td>
            </tr>
            <tr>
                <td style="height: 15px; text-align: center">
                    <asp:Label ID="lblItemNo" runat="server" Text="Item #:" CssClass="td_text"></asp:Label>&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblSerialNo" runat="server" Text="Ser :" CssClass="td_text"></asp:Label>&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="lblQty" runat="server" Text="Qty/Lbs:" CssClass="td_text"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 15px"></td>
            </tr>
            <tr>
                <td align="center">
                    <h3>
                        <asp:Button ID="btnKeep" runat="server" Text="Keep"  Width="290px" Height="45px" CssClass="td_text_larger" />
                    </h3>
                </td>
            </tr>
            <tr>
                <td style="height: 10px">
                    <asp:TextBox ID="txtCustomerOrderNo" runat="server" type="hidden"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="center" class="td_text">
                    <asp:Button ID="btnRemove" runat="server" Text="Return"  Width="290px" Height="45px" CssClass="td_text_larger" />
                </td>
            </tr>
            <tr>
                <td style="height: 15px"></td>
            </tr>
            <tr>
                <td align="center" class="td_text">
                    <asp:Button ID="btnEdit" runat="server" Text="Edit"  Width="290px" Height="45px" CssClass="td_text_larger" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
