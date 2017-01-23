

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rapid POS Mobile WL - Error</title>
<link href="rapid_style.css" rel="stylesheet" type="text/css" />
<link href="style.css" rel="stylesheet" type="text/css" />
<!-- Start iPhone -->
<meta name="viewport" content="width=device-width; initial-scale=1.0; maximum-scale=1.0; user-scalable=0;" />
<meta name="apple-mobile-web-app-capable" content="yes" />
<meta name="apple-mobile-web-app-status-bar-style" content="black" />
<link rel="apple-touch-icon" href="/iphone.png" />
<!-- End iPhone --> 

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
        <td colspan=3 align="center">
            <asp:ImageButton ID="ImageButton1" runat="server" 
                ImageUrl="~/Images/logo_small.gif" />
        </td>
    </tr>
    <tr>
        <td colspan=3></td>
    </tr>    
    <tr>
        <td colspan="3" align="center"><h3>Rapid POS Mobile WL - Error</h3></td>
    </tr>
    <tr>
        <td colspan="3" align="center" class="td_text"></td>
    </tr>  
    <tr>
        <td colspan="3" align="center">
   
            <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/error.gif" />
   
        </td>
    </tr>
    <tr>
        <td colspan="3" align="center" height="3"></td>
    </tr>  
    <tr>
        <td colspan="3" align="center" class="td_text"><asp:Label ID="lblErrorMessage" 
                runat="server" Font-Bold="True" ForeColor="Red"></asp:Label></td>
    </tr>
    <tr>
        <td colspan="3" align="center" height="3"></td>
    </tr>       
    <tr>
        <td>
            <asp:Button ID="btnNewScan" runat="server" Text="New Scan" />
        </td>
        <td></td>
        <td align="right">
           <input id="btnGoBack" type="button" value="Go Back" onclick="javascript:history.go(-1);" />
        </td>
    </tr>  
    </table>
    </form>
</html>
