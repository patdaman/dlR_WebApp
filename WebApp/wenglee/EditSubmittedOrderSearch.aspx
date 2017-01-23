<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EditSubmittedOrderSearch.aspx.vb" Inherits="_Default" %>

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
        function deselectDropDownList() {
            document.getElementById('ddlCustomerList').selectedIndex = -1;
        }

        function ddlCustomerList_SelectedIndexChanged() {
            document.getElementById('txtCustomerDesc').value = document.getElementById('ddlCustomerList').options[document.getElementById('ddlCustomerList').selectedIndex].text;
        }

        function ddlCustomerOrderList_SelectedIndexChanged() {
            document.getElementById('txtCustomerOrderNo').value = document.getElementById('ddlCustomerOrderList').options[document.getElementById('ddlCustomerOrderList').selectedIndex].text;
        }

        function submitChange() {
            __doPostBack();
        }

        function icodyDidScanBarcode( value, typeID, typeValue, date ) {
            // your implementation here
            document.getElementById('webbarcode').value = value;
            //document.getElementById('webbarcode').hidden = true;

            submitChange();

        }

    </script>
     <script type="text/javascript" language="javascript">
         function searchOrderClick() {

             document.getElementById("btnSearch").value = 'Please wait...';
             document.getElementById("btnSearch").disabled = true;
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
    <table border="0" cellpadding="1" cellspacing="0" bgcolor="#FFFFFF" align="center">
    <tr>
        <td></td>
    </tr>    
    <tr>
        <td align="center"><h3>Edit Order</h3></td>
    </tr>
    <tr>
        <td style="height:5px"></td>
    </tr>  
        <tr>
            <td style="height:5px; font-weight: 700;">Carton / Order Number:&nbsp;
                <asp:TextBox ID="webbarcode" runat="server" AutoPostBack="true" MaxLength="52" Text="" Width="110px" ></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="height:5px">
                <asp:CustomValidator ID="valOrder" runat="server" CssClass="td_text" Display="Dynamic" ErrorMessage="CustomValidator" ForeColor="Red"></asp:CustomValidator>
            </td>
        </tr>
    <tr>
        <td class="td_text"><strong>Customer List</strong></td>
    </tr>
    <tr>
        <td style="text-align:center">
            <asp:listbox ID="ddlCustomerList" runat="server" Width="290px" Height="50px" AutoPostBack="false"></asp:listbox>
        </td>
    </tr>
    <tr>
        <td style="height:15px"></td>
    </tr> 
    <tr>
        <td class="td_text"><strong>Open Orders</strong></td>
    </tr>
    <tr>
        <td class="td_text_larger" align="left" valign="top" style="text-align:center">
            <asp:ListBox ID="ddlCustomerOrderList" runat="server" AutoPostBack="false" Height="50px" Width="290px"></asp:ListBox>
        </td>
    </tr>
    <tr>
        <td style="height:10px;text-align:center">
            <asp:TextBox ID="txtCustomerOrderNo" runat="server" type="hidden"></asp:TextBox>
            <asp:Label ID="lblErrMsg" runat="server" Text="" Font-Size="Large" ForeColor="Red"></asp:Label>
        </td>
    </tr> 
    <tr>
        <td align="center" class="td_text">
            <asp:Button ID="btnOk" runat="server" Text="OK" Width="250px" CssClass="td_text_larger" Height="45px" />
        </td>
    </tr>
    <tr>
        <td style="height:15px"></td>
    </tr> 
    <tr>
        <td align="center" class="td_text">
            <asp:Button ID="btnMainMenu" runat="server" Text="Main Menu" Width="250px" CssClass="td_text_larger" Height="45px"/>
        </td>
    </tr>
    </table>
    </form>
</body>
</html>
