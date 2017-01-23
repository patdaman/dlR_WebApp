<%@ Page Language="VB" AutoEventWireup="false" CodeFile="OrderCanceled.aspx.vb" Inherits="Success" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rapid POS Mobile WL - Order Canceled</title>
    <link href="mobile.css" rel="stylesheet" type="text/css" />
    <!-- Start iPhone -->
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-touch-fullscreen" content="YES" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <!-- End iPhone --> 
<asp:Panel ID="pnlRefresh" runat="server">
    <meta http-equiv="refresh" content="100.5;URL='zzz.aspx'">
</asp:Panel>

</head>
<body bgcolor="#FFFFFF">
    <form id="form1" runat="server">
    <table border="0" cellpadding="1" cellspacing="0" bgcolor="#FFFFFF" align="center">
    <tr>
        <td colspan="3" align="center">
            <asp:ImageButton ID="ImageButton1" runat="server" 
                ImageUrl="~/Images/logo_small.gif" />
        </td>
    </tr>
    <tr>
        <td colspan=3></td>
    </tr>    
    <tr>
        <td colspan="3" align="center"><h3>Rapid POS Mobile WL<br />Order Canceled</h3></td>
    </tr>
    <tr>
        <td align="center" class="td_text"></td>
    </tr>  
    <tr>
        <td align="center">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/trashcan_orange.jpg" />
        </td>
    </tr>
    <tr>
        <td align="center" height="3"></td>
    </tr>  
    <asp:Panel ID="pnlRedirect" runat="server">        
    <tr>
        <td colspan="3" align="center" class="td_text">In 1.5 seconds, you will be redirected to the "Receive - Search Vendor" screen. Click the button below to go there immediately.</td>
    </tr>
    </asp:Panel>
    <tr>
        <td align="center" height="3" class="td_text">This order has been CANCELED. If you did not intend to cancel, you will need to
         scan the items again in a new order.</td>
    </tr>    
    <tr>
        <td colspan="3" style="height:15px"></td>
    </tr>      
    <tr>
        <td align="center">
            <asp:Button ID="btnReceive" runat="server" Text="Main Menu" Width="290px" 
                Height="45px" CssClass="td_text_larger" />
            </td>
    </tr>  
    <tr>
        <td style="height:15px"></td>
    </tr>    
    <tr>
        <td align="center">
            <asp:Button ID="btnOrder" runat="server" Text="New Order"  Width="290px" 
                Height="45px" CssClass="td_text_larger"  />
            </td>
    </tr>  
    </table>
    </form>
</body>
</html>
