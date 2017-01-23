<%@ Page Language="VB" AutoEventWireup="false" CodeFile="CustomerConfirmation.aspx.vb" Inherits="CustomerConfirmation" %>


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

    </script>
    <script type="text/javascript" language="javascript">
        function doYes(btnYes) {
            if (typeof (Page_ClientValidate) == 'function' && Page_ClientValidate() == false) {
                return false;
            }
            btnYes.disabled = 'disabled';
            btnYes.value = 'Please Wait...';
            <%= ClientScript.GetPostBackEventReference(btnYes, String.Empty)%>;
        }

        function doSubmit(btnSubmit) {
            if (typeof (Page_ClientValidate) == 'function' && Page_ClientValidate() == false) {
                return false;
            }
            btnSubmit.disabled = 'disabled';
            btnSubmit.value = 'Please Wait...';
            <%= ClientScript.GetPostBackEventReference(btnSubmit, String.Empty)%>;
                }

    </script>
    <style type="text/css">
        .auto-style2 {
            font-family: arial;
            font-size: 14px;
            font-weight: normal;
            height: 25px;
        }

        .auto-style3 {
            height: 20px;
        }

        .auto-style4 {
            height: 5px;
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
                <td align="center" class="auto-style3">
                    <h3>Customer Confirmation</h3>
                </td>
            </tr>
            <tr>
                <td style="height: 5px"></td>
            </tr>
            <tr>
                <td style="height: 5px; text-align: center; font-weight: 600;">
                    <asp:Label ID="lblConfirmCust" runat="server" Text="Are you sure this is the correct customer?"></asp:Label></td>
            </tr>
            <tr>
                <td style="height: 5px; font-weight: 700;">&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style4" style="text-align: center">
                    <asp:Label ID="lblCustomerDesc" runat="server" Text="Customer : " Style="height: 5px; font-weight: 700;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 5px"></td>
            </tr>
            <tr>
                <td class="td_text"><strong>
                    <asp:Label ID="lblAllCust" runat="server" Text="All Customers"></asp:Label></strong></td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:ListBox ID="ddlCustomerList" runat="server" Width="290px" Height="50px" AutoPostBack="false"></asp:ListBox>
                </td>
            </tr>
            <tr>
                <td class="td_text">&nbsp;</td>
            </tr>
            <tr>
                <td class="td_text"><strong>
                    <asp:Label ID="lblQuickSearch" runat="server" Text="Quick Search for Customer"></asp:Label></strong></td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <asp:TextBox ID="txtCustomerDesc" runat="server" Width="290px" Height="20px"></asp:TextBox>
                    <asp:AutoCompleteExtender ID="AutoCompleteExtender1"
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
                <td class="auto-style2">
                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
                    <br />
                    <asp:CustomValidator ID="valCustomer" runat="server" CssClass="td_text" Display="Dynamic" ErrorMessage="CustomValidator" ForeColor="Red"></asp:CustomValidator>
                </td>
            </tr>
            <tr>
                <td align="center" class="td_text">
                    <asp:Button ID="btnYes" runat="server" Text="Yes" Width="290px" Height="45px" CssClass="td_text_larger" OnClick="btnYes_Click" OnClientClick="doYes(this)" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td style="height: 10px"></td>
            </tr>
            <tr>
                <td align="center" class="td_text">
                    <asp:Button ID="BtnNo" runat="server" Width="290px" Height="45px" CssClass="td_text_larger" Text="NO" />
                </td>
            </tr>
            <tr>
                <td style="height: 15px"></td>
            </tr>
            <tr>
                <td style="height: 15px; text-align: center" class="td_text">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" Width="290px" Height="45px" CssClass="td_text_larger" OnClick="btnSubmit_Click" OnClientClick="doSubmit(this)" AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td style="height: 15px"></td>
            </tr>
        </table>
    </form>
</body>
</html>
