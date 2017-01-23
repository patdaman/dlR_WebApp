<%@ Page Language="VB" AutoEventWireup="false" CodeFile="OrderSuccess.aspx.vb" Inherits="Success" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rapid POS Mobile WL - Order Saved to CounterPoint</title>
    <link href="mobile.css" rel="stylesheet" type="text/css" />
    <!-- Start iPhone -->
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-touch-fullscreen" content="YES" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <!-- End iPhone --> 

    <script type="text/javascript" language="javascript">

    </script>
</head>
<body bgcolor="#FFFFFF">
    <form id="form1" runat="server">
    <table border="0" cellpadding="1" cellspacing="0" bgcolor="#FFFFFF" align="center">
    <tr>
        <td align="center">
            <asp:ImageButton ID="ImageButton1" runat="server" 
                ImageUrl="~/Images/logo_small.gif" />
        </td>
    </tr>
    <tr>
        <td></td>
    </tr>    
    <tr>
        <td align="center">
            <h3>Rapid POS Mobile WL<br />
                Order saved to CounterPoint</h3>
        </td>
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
        <td align="center" height="3"><asp:Label ID="lblMessage" runat="server" Text="Label" CssClass="td_text"></asp:Label>
        </td>
    </tr>  
    <tr>
        <td align="center" height="3"></td>
    </tr>          
    <tr>
        <td align="center">
            <asp:Button ID="btnPrintInvoice" runat="server" Text="Print Invoice"  Width="290px"  Enabled="false"
                Height="45px" CssClass="td_text_larger"  />
            </td>
    </tr>  
    <tr>
        <td align="center" height="3"></td>
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
