<%@ Page Language="VB" AutoEventWireup="false" CodeFile="OrderSearchListOpenOrder.aspx.vb" Inherits="_Default" %>

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
    <script language="javascript">
        function deselectDropDownList()
        {
            document.getElementById('ddlCustomerList').selectedIndex = -1;
        }

        function ddlCustomerList_SelectedIndexChanged()
        {
            document.getElementById('txtCustomerDesc').value = document.getElementById('ddlCustomerList').options[document.getElementById('ddlCustomerList').selectedIndex].text;
        }

    </script>
</head>
<body bgcolor="#FFFFFF">
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" />
    
    <table border="0" cellpadding="1" cellspacing="0" bgcolor="#FFFFFF" align="center">
    <tr>
        <td align="center"><asp:Image ID="imgLogo" runat="server" 
                ImageUrl="~/Images/logo_small.gif" /></td>
    </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server"> 
    <ContentTemplate> 
    <table border="0" cellpadding="1" cellspacing="0" bgcolor="#FFFFFF" align="center">
    <tr>
        <td></td>
    </tr>    
    <tr>
        <td align="center"><h3>Order - Search Customer</h3></td>
    </tr>
    <tr>
        <td style="height:5px"></td>
    </tr>  
    <tr>
        <td class="td_text"><strong>Customers With Open Orders</strong></td>
    </tr>
    <tr>
        <td>
            <asp:listbox ID="ddlCustomerList" runat="server" Width="290px" Height="50px" AutoPostBack="false">
            </asp:listbox>

        </td>
    </tr>
    <tr>
        <td style="height:15px"></td>
    </tr> 
    <tr>
        <td class="td_text"><strong>Quick Search for Customer</strong></td>
    </tr>
    <tr>
        <td class="td_text_larger" align="left" valign="top">
            <asp:TextBox ID="txtCustomerDesc" runat="server" MaxLength="40" Width="280px" Height="30px" OnTextChanged="customerSelected"
            AutoPostBack="true"></asp:TextBox>
            <div id="divCustomerDescSuggestions"></div>
            <asp:AutoCompleteExtender   
                ID="AutoCompleteExtender1" 
                CompletionListElementID="divCustomerDescSuggestions" 
                TargetControlID="txtCustomerDesc"   
                runat="server" ServiceMethod="GetCompletionList" UseContextKey="True" CompletionInterval="1" 
                CompletionListItemCssClass="td_text_larger"
                EnableCaching="true" 
                MinimumPrefixLength="2"
                FirstRowSelected="true" />  
        </td>
    </tr>
    <tr>
        <td>            
            <asp:CustomValidator ID="valCustomer" runat="server" Display="Dynamic"
                ErrorMessage="CustomValidator" ForeColor="Red" CssClass="td_text"></asp:CustomValidator>            
        </td>
    </tr> 
    <tr>
        <td style="height:10px"></td>
    </tr> 
    <tr>
        <td align="center" class="td_text">
            <asp:Button ID="btnOk" runat="server" Text="OK" Width="250px" CssClass="td_text_larger" 
                Height="45px" />
        </td>
    </tr>
    <tr>
        <td style="height:15px"></td>
    </tr> 
    <tr>
        <td align="center" class="td_text">
            <asp:Button ID="btnMainMenu" runat="server" Text="Main Menu" Width="250px" CssClass="td_text_larger" 
                Height="45px" />
        </td>
    </tr>
    </table>
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
