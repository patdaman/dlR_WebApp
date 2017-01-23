<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<script runat="server">


</script>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rapid POS Mobile WL - Main Menu</title>
    <link href="mobile.css" rel="stylesheet" type="text/css" />

    <!-- Start iPhone -->
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-touch-fullscreen" content="yes" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <!-- End iPhone -->
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="expires" content="-1" />

    <script language="javascript">
        var updateLayout = function () {
            if (window.innerWidth != currentWidth) {
                currentWidth = window.innerWidth;
                var orient = (currentWidth == 290) ? "profile" : "landscape"; document.body.setAttribute("orient", orient); window.scrollTo(0, 1);
            }
        };

        iPhone.DomLoad(updateLayout); setInterval(updateLayout, 500); 

    </script>
     

    <style type="text/css">
        .auto-style2 {
            height: 15px;
        }
        .auto-style4 {
            height: 57px;
        }
        .auto-style3 {
            height: 23px;
        }
    </style>
     

</head>
<body bgcolor="#FFFFFF">
    <form id="form1" runat="server">
    <table border="0" cellpadding="1" cellspacing="0" bgcolor="#FFFFFF" align="center">
    <tr>
        <td align="center"><asp:Image ID="imgLogo" runat="server" 
                ImageUrl="~/Images/logo_small.gif" /></td>
    </tr>
    <tr>
        <td></td>
    </tr>    
    <tr>
        <td align="center"><h3>Rapid POS Mobile WL - Home</h3></td>
    </tr>
    <tr>
        <td align="center" class="td_text">&nbsp;</td>
    </tr>  
    <tr>
        <td align="center"> 
            <asp:Button ID="btnReceive" runat="server" Text="Receive" Width="200px" 
                Height="55px" CssClass="td_text_larger" />
        </td>
    </tr>  
    <tr>
        <td height="10"></td>
    </tr>    
    <tr>
        <td align="center" >
            <asp:Button ID="btnNewOrder" runat="server" Text="New Order" Width="200px" 
                Height="55px" CssClass="td_text_larger" />
        </td>
    </tr>  
    <tr>
        <td height="10"></td>
    </tr>   
    <tr>
        <td align="center">
            <asp:Button ID="btnSavedOrder" runat="server" Text="Saved Orders" Width="200px" 
                Enabled="False" Height="55px" CssClass="td_text_larger" />
        </td>
    </tr>  
    <tr>
        <td height="10"></td>
    </tr>   
        <tr>
        <td align="center" class="auto-style4">
            <asp:Button ID="btnEditSubmittedOrder" runat="server" Text="Edit Order" 
                Width="200px" Height="55px" CssClass="td_text_larger" />
        </td>
        </tr>
    <tr>
        <td height="10"></td>
    </tr>
        <tr>
        <td align="center">
            <asp:Button ID="btnPrintReport" runat="server" Text="Print Receivings" 
                Width="200px" Height="55px" CssClass="td_text_larger" OnClick="btnItemLookup_Click" />
        </td>
        </tr>
    <tr>
        <td class="auto-style2"></td>
    </tr> 
    <tr> 
        <td class="td_text" align="center"><strong><asp:Label ID="lblUnsubmittedReceiver" runat="server" 
        Text="" ForeColor="DarkOrange">Note: Receiver has <a href="ReceiveScan.aspx">uncommited scans</a> pending.</asp:Label></strong><br /><strong><asp:Label ID="lblUnsubmittedOrder" runat="server" 
        Text="" ForeColor="DarkOrange">Note: You have an <a href="OrderScan.aspx">unsubmitted order</a> pending.</asp:Label></strong>
        </td>
    </tr> 
    </table>

    </form>
</body>
</html>
