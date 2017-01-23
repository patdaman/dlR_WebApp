<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="RapidInventoryWebApp.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rapid Physical Inventory</title>
    <link href="CSS/Styles.css" rel="stylesheet" />
    <style type="text/css">
        .auto-style1 {
            height: 30px;
        }
    </style>
</head>
<body>
    <form id="login" runat="server" class="pure-form" >
        <h1 align="center">Rapid Physical Inventory</h1>
    <table>
        <tr>
            <td align="center">Company - Location</td>
            <td align="center">
                <asp:DropDownList ID="compLocDropDown" runat="server" Width="150px" CssClass="field"></asp:DropDownList>
                <asp:requiredfieldvalidator id="Requiredfieldvalidator1" runat="server" ControlToValidate="compLocDropDown" Text="*"></asp:requiredfieldvalidator>
            </td>
        </tr>
        <tr>
            <td align="center" class="auto-style1">Username</td>
            <td align="center" class="auto-style1">
                <asp:TextBox id="UserName" runat="server" Width="150px" CssClass="field"></asp:TextBox>
                <asp:requiredfieldvalidator id="UserNameRequired" runat="server" ControlToValidate="UserName" Text="*"></asp:requiredfieldvalidator>
            </td>
        </tr>
        <tr>
            <td align="center">Password</td>
            <td align="center">
                <asp:TextBox id="Password" runat="server" textMode="Password" Width="150px" CssClass="field"></asp:TextBox>
                <asp:requiredfieldvalidator id="PasswordRequired" runat="server" ControlToValidate="Password" Text="*" SetFocusOnError="True"></asp:requiredfieldvalidator>
            </td>
        </tr>
        <tr>
            <td>Persistent Cookie:</td>
            <td><ASP:CheckBox id="chkPersistCookie" runat="server" autopostback="false" /></td>
            <td></td>
        </tr>
         <tr class="submit" >
            <td align="center" colspan="2"> <asp:Button ID="LoginButton" runat="server" Text="Login" /></td>
        </tr>
        <tr>
            <td colspan="2" class="back">
            <asp:Literal Text="Login unsuccessful" runat="server" Visible="false" ID="Literal1" /> 
            </td>  
        </tr>
        </table>
    </form>
</body>
</html>
