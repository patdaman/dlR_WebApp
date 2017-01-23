<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReceiveSearchListVendor.aspx.vb" Inherits="_Default" %>

<%@ Register TagPrefix="asp" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rapid POS Mobile WL - Receive - Search Vendor</title>
    <link href="mobile.css" rel="stylesheet" type="text/css" />
    <!-- Start iPhone -->
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-touch-fullscreen" content="yes" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <!-- End iPhone --> 
    <script language="javascript">
        function deselectDropDownList() {
            document.getElementById('ddlVendorList').selectedIndex = -1;
        }

        function ddlVendorList_SelectedIndexChanged() {
            document.getElementById('txtVendorDesc').value = document.getElementById('ddlVendorList').options[document.getElementById('ddlVendorList').selectedIndex].text;
        }

    </script>    

</head>
<body bgcolor="#FFFFFF">    
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="Server" EnablePageMethods="true" />
   
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
        <td align="center"><h3>Receive - Search Vendor</h3></td>
    </tr>
    <tr>
        <td style="height:5px"></td>
    </tr>  
    <tr>
        <td class="td_text"><strong>Quick Search for Vendor</strong></td>
    </tr>
    <tr>
        <td class="td_text_larger">
            <asp:TextBox ID="txtVendorDesc" runat="server" MaxLength="40" Width="235px" OnTextChanged="vendorSelected" Enabled="true"
            AutoPostBack="true" Height="30px"></asp:TextBox>&nbsp;<asp:Button ID="btnOk" runat="server" Text="OK" Width="50px" CssClass="td_text_larger" 
                Height="30px" />
            <div id="divVendorDescSuggestions"></div>
            <asp:AutoCompleteExtender   
                ID="AutoCompleteExtender1" 
                CompletionListElementID="divVendorDescSuggestions" 
                TargetControlID="txtVendorDesc"   
                runat="server" ServiceMethod="GetCompletionList" UseContextKey="True" CompletionInterval="1" 
                CompletionListItemCssClass="td_text_larger"
                EnableCaching="true" 
                MinimumPrefixLength="2"
                FirstRowSelected="true" />  
        </td>
    </tr>
    <tr>
        <td>            
            <asp:CustomValidator ID="valVendor" runat="server" Display="Dynamic"
                ErrorMessage="CustomValidator" ForeColor="Red" CssClass="td_text"></asp:CustomValidator>            
        </td>
    </tr> 
    <tr>
        <td class="td_text"><strong>All Vendors</strong></td>
    </tr>
    <tr>
        <td>
            <asp:listbox ID="ddlVendorList" runat="server" Width="290px" Height="50px" AutoPostBack="false"
             OnSelectedIndexChanged="ddlVendorList_SelectedIndexChanged">
            </asp:listbox>

        </td>
    </tr>
    <tr>
        <td style="height:15px"></td>
    </tr> 
    <tr>
        <td align="center" class="td_text">
            <asp:Button ID="btnUseWingLee" runat="server" Text="Use Weng Lee" CssClass="td_text_larger"
                Width="250px" Height="45px" />
        </td>
    </tr>
    <tr>
        <td  style="height:15px"></td>
    </tr> 
    <tr>
        <td align="center" class="td_text">
            <asp:Button ID="btnMainMenu" runat="server" Text="Main Menu" Width="250px" CssClass="td_text_larger" 
                Height="45px" />
        </td>
    </tr>
    <tr>
        <td class="td_text_small_subdued">Receiver ID: 
            <asp:Label ID="lblReceiverId" runat="server" Text=""></asp:Label>
        </td>
    </tr> 
    </table>

    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
