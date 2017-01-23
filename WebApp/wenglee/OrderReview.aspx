<%@ Page Language="VB" AutoEventWireup="false" CodeFile="OrderReview.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rapid POS Mobile WL - Order - Review</title>
    <link href="mobile.css" rel="stylesheet" type="text/css" />
    <!-- Start iPhone -->
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-touch-fullscreen" content="YES" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <!-- End iPhone -->
    <meta name="format-detection" content="telephone=no" />

    <script type="text/javascript" language="javascript">
        function completeOrderClick() {
            var completeYN;
            completeYN = confirm('Are you sure you want COMPLETE this order?');

            if (completeYN == true) {
                document.getElementById("btnCompleteOrder").value = 'Please wait...';
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
                <td colspan="3" align="center">
                    <asp:Image ID="imgLogo" runat="server"
                        ImageUrl="~/Images/logo_small.gif" /></td>
            </tr>
            <tr>
                <td></td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <h3>Order - Review</h3>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center" style="height: 5px"></td>
            </tr>
            <tr>
                <td colspan="3" align="right">&nbsp;</td>
            </tr>
            <tr>
                <td style="height: 5px"></td>
            </tr>
            <tr>
                <td colspan="3" class="td_text" style="height: 5px; font-size: 18px"><strong>
                    <asp:Label ID="lblOrderTxt" runat="server" Text="Order:"></asp:Label></strong></td>
            </tr>
            <tr>
                <td colspan="3" class="td_text" style="height: 5px; font-size: 18px">
                    <asp:Label ID="lblPickingId" runat="server" Text=""></asp:Label></td>
            </tr>
            <tr>
                <td style="height: 5px"></td>
            </tr>
            <tr>
                <td colspan="3" style="height: 5px; font-size: 18px" class="td_text"><strong>Customer:</strong> #<asp:Label ID="lblCustomerNum" runat="server" Font-Size="18px"></asp:Label>
                    -
                    <asp:Label ID="lblCustomerDesc" runat="server" Text="" Font-Size="18px"></asp:Label></td>
            </tr>
            <tr>
                <td style="height: 5px"></td>
            </tr>
            <tr>
                <td colspan="3" class="td_text" align="center"><strong>Items</strong></td>
            </tr>
            <tr>
                <td colspan="3" style="height: 5px"></td>
            </tr>
            <tr>
                <td colspan="3" align="center" class="td_text">Line Item:
                    <asp:Label ID="lblLineItem" runat="server"></asp:Label>
                    <asp:Repeater ID="rptrItemsScanned" runat="server">
                        <HeaderTemplate>
                            <table cellpadding="2" cellspacing="1">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td colspan="3"><strong><%# DataBinder.Eval(Container.DataItem, "DESCR")%></strong></td>
                            </tr>
                            <tr>
                                <td>Item #: <%# DataBinder.Eval(Container.DataItem,
                            "SCAN_ORD_ITEM_NO")%></td>
                                <td style="pointer-events: none; text-decoration: none; color: inherit;">Ser: <%# DataBinder.Eval(Container.DataItem,
                            "WING_LEE_CARTON_SERIAL")%></td>
                                <td>Qty/Lbs: <%# DataBinder.Eval(Container.DataItem,
                            "SCAN_ORD_PIECES_WEIGHT")%></td>
                            </tr>
                            <tr>
                                <td style="border-style: none none dotted none; border-width: thin">
                                    <asp:LinkButton ID="lbtnDelete" CommandName="Delete"
                                        OnClientClick='<%# "return confirm(""Are you sure you want to remove item #" & DataBinder.Eval(Container.DataItem,
                            "SCAN_ORD_ITEM_NO") & "?"");"%>'
                                        CommandArgument='<%# Eval("SCAN_ORD_ID") %>'
                                        runat="server" Text="Delete" CausesValidation="false" /></td>
                                <td style="border-style: none none dotted none; border-width: thin"></td>
                                <td align="right" style="border-style: none none dotted none; border-width: thin">
                                    <asp:LinkButton ID="lbtnEdit" CommandName="Edit"
                                        CommandArgument='<%# Eval("SCAN_ORD_ID")%>'
                                        runat="server" Text="Edit" CausesValidation="false" />
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center" style="height: 5px"><strong>
                    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label></strong></td>
            </tr>
            <tr>
                <td colspan="3" align="center">
                    <asp:Button ID="btnCompleteOrder" runat="server" Text="Complete Order" CssClass="td_text_larger" OnClientClick="return completeOrderClick();"
                        Width="290px" Height="45px" />
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center" style="height: 25px"></td>
            </tr>
            <tr>
                <td colspan="3" align="center" style="height: 25px">
                    <asp:Button ID="btnScanMore" runat="server" Text="Scan More" CssClass="td_text_larger"
                        Width="290px" Height="45px" />
                </td>
            </tr>
            <tr>
                <td colspan="3" align="center" style="height: 25px"></td>
            </tr>
            <tr>
                <td colspan="3" align="center" style="height: 25px"></td>
            </tr>
            <tr>
                <td colspan="3" align="center" style="height: 25px"></td>
            </tr>
            <tr>
                <td colspan="3" align="center" style="height: 25px"></td>
            </tr>
            <tr>
                <td colspan="3" align="center" style="height: 25px">
                    <asp:Button ID="btnCancelOrder" runat="server" Text="Cancel Order" CssClass="td_text_larger"
                        Width="290px" Height="45px"
                        OnClientClick="return confirm('Are you sure you want to CANCEL this order?')"/>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="td_text_small_subdued">Picking ID: 
            <asp:Label ID="lblPickingNo" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
