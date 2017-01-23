<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReceiveReview.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rapid POS Mobile WL - Recieve - Review</title>
    <link href="mobile.css" rel="stylesheet" type="text/css" />
    <!-- Start iPhone -->
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-touch-fullscreen" content="YES" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <!-- End iPhone --> 
    <meta name="format-detection" content="telephone=no" />

    <script type="text/javascript" language="javascript">
        function completeReceiverClick() {
            var completeYN;
            completeYN = confirm('Are you sure you want COMPLETE this receiver?');

            if (completeYN == true) {
                document.getElementById("btnCompleteReceiver").value = 'Please wait...';
                document.getElementById("btnScanMore").disabled = true;
            }

            return completeYN;
        }
    </script>
</head>
<body bgcolor="#FFFFFF">
    <form id="form1" runat="server">
    <table border="0" cellpadding="1" cellspacing="0" bgcolor="#FFFFFF" align="center">
    <tr>
        <td colspan="3" align="center"><asp:Image ID="imgLogo" runat="server" 
                ImageUrl="~/Images/logo_small.gif" /></td>
    </tr>
    <tr>
        <td></td>
    </tr>    
    <tr>
        <td colspan="3" align="center"><h3>Receive - Review</h3></td>
    </tr>
    <tr>
        <td style="height:5px"></td>
    </tr>  
    <tr>
        <td colspan="3" align="right">
            <asp:Button ID="btnCancelReceiver" runat="server" Text="Cancel Receiver" CssClass="td_text" 
                Width="125px" Height="20px" 
                OnClientClick="return confirm('Are you sure you want to CANCEL this receiver?')" />
        </td>
    </tr>
    <tr>
        <td style="height:5px"></td>
    </tr>  
    <tr>
        <td colspan="3" style="height:5px" class="td_text"><strong>Receiver:</strong> <asp:Label ID="lblReceiverId" runat="server" Text=""></asp:Label></td>
    </tr>
    <tr>
        <td style="height:5px"></td>
    </tr>  
    <tr>
        <td colspan="3" class="td_text" align="center"><strong>Items</strong></td>
    </tr> 
    <tr>
        <td colspan="3" style="height:5px"></td>
    </tr>
    <tr>
        <td colspan="3" align="center" class="td_text">
            Line Item: <asp:Label id="lblLineItem" runat="server"></asp:Label>
             <asp:Repeater ID="rptrItemsScanned" runat="server">
                <HeaderTemplate>
                    <table cellpadding="2" cellspacing="1">
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td colspan="7"><strong><%# DataBinder.Eval(Container.DataItem, "DESCR")%></strong></td>
                    </tr>
                    <tr>
                        <td>Item #: <%# DataBinder.Eval(Container.DataItem,
                            "CARTON_ITEM_NO")%></td>
                        <td style="pointer-events: none; text-decoration:none; color:inherit;">Ser: <%# DataBinder.Eval(Container.DataItem,
                            "WING_LEE_CARTON_SERIAL")%></td>
                        <td>Qty/Lbs: <%# DataBinder.Eval(Container.DataItem,
                            "CARTON_PIECES_WEIGHT")%></td>
                    </tr>
                    <tr>
                        <td><asp:LinkButton ID="lbtnDelete" CommandName="Delete"
                            OnClientClick='<%# "return confirm(""Are you sure you want to remove item #" & DataBinder.Eval(Container.DataItem,
                            "CARTON_ITEM_NO") & "?"")" %>' CommandArgument='<%# Eval("CARTON_NO") %>' 
                            runat="server" Text="Remove" CausesValidation="false" />
                        </td>
                        <td></td>
                        <td align="right"><asp:LinkButton ID="lbtnEdit" CommandName="Edit"
                            CommandArgument='<%# Eval("CARTON_NO") %>' 
                            runat="server" Text="Edit" CausesValidation="false" />
                    </tr>
                    <tr>
                        <td colspan="7" style="border-style: none none dotted none; border-width: thin">
                            Vendor: <%# DataBinder.Eval(Container.DataItem, "VEND_NO")%></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>                    
            </asp:Repeater>             
        </td>
    </tr>    
    <tr>
        <td colspan="3" align="center" style="height:5px"></td>
    </tr> 
    <tr>
        <td colspan="3"><strong><asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label></strong>
        </td>
    </tr> 
    <tr>
        <td colspan="3" align="center">
            <asp:Button ID="btnCompleteReceiver" runat="server" Text="Receiver Complete" CssClass="td_text_larger"
                OnClientClick="return completeReceiverClick();"
                Width="290px" Height="45px" />
        </td>
    </tr>
    <tr>
        <td colspan="3" align="center" style="height:5px"></td>
    </tr>
    <tr>
        <td colspan="3" align="center">
            <asp:Button ID="btnScanMore" runat="server" Text="Scan More" CssClass="td_text_larger"  
                Width="290px" Height="45px" />
        </td>
    </tr>
    </table>

    </form>
</body>
</html>
