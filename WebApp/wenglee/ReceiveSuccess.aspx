<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReceiveSuccess.aspx.vb" Inherits="Success" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rapid POS Mobile WL - Receiver Saved to CounterPoint</title>
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

<script language="javascript">
    var updateLayout = function () {
        if (window.innerWidth != currentWidth) {
            currentWidth = window.innerWidth;
            var orient = (currentWidth == 290) ? "profile" : "landscape"; document.body.setAttribute("orient", orient); window.scrollTo(0, 1);
        }
    };

    iPhone.DomLoad(updateLayout); setInterval(updateLayout, 500); 
</script>
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
        <td colspan="3" align="center"Receiver saved to CounterPoint</h3></td>
    </tr>
    <tr>
        <td align="center" class="td_text"></td>
    </tr>  
    <tr>
        <td align="center">
   
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/success.jpg" />
   
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
        <td align="center" height="3"></td>
    </tr>       
    <tr>
        <td align="center">
            <asp:Button ID="btnMainMenu" runat="server" Text="Main Menu" Width="290px" 
                Height="45px" CssClass="td_text_larger" />
            </td>
    </tr>  
    <tr>
        <td align="center" height="3"></td>
    </tr>       
    <tr>
        <td align="center">
            <asp:Button ID="btnSearchVendor" runat="server" Text="New Receiver" Width="290px" 
                Height="45px" CssClass="td_text_larger" />
            </td>
    </tr>  
    </table>
    </form>
</body>
</html>
