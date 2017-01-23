<%@ Page Language="VB" AutoEventWireup="false" CodeFile="OrderScan.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rapid POS Mobile WL - Order - Scan</title>
    <link href="mobile.css" rel="stylesheet" type="text/css" />
    <!-- Start iPhone -->
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <meta name="apple-mobile-web-app-status-bar-style" content="black" />
    <meta name="apple-touch-fullscreen" content="YES" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <!-- End iPhone -->
    <meta name="format-detection" content="telephone=no" />

    <script src="jquery.min.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">
        var passedValidation;

        function PageMethod(fn, paramArray, successFn, errorFn) {
            var pagePath = window.location.pathname;

            $("#valUpcCustom").text('');

            //Create list of parameters in the form : {"paramName1":"paramValue1","paramName2":"paramValue2"} 
            var paramList = '';
            if (paramArray.length > 0) {
                for (var i = 0; i < paramArray.length; i += 2) {
                    if (paramList.length > 0)
                        paramList += ',';
                    paramList += '"' + paramArray[i] + '":"' + paramArray[i + 1] + '"';
                }
            }

            paramList = '{' + paramList + '}';

            //Call the page method 
            $.ajax({
                type: "POST",
                url: pagePath + "/" + fn,
                contentType: "application/json; charset=utf-8",
                data: paramList,
                dataType: "json",
                global: false,
                async: false,
                success: successFn,
                error: errorFn
            });
        }

        function AjaxSucceeded(result) {
            var errorText;
            var needsItemNo;
            var needsLbs;
            var needsQtyPcs;
            var outputArray = result.d.split('|');


            errorText = outputArray[0];
            needsItemNo = outputArray[1];
            needsLbs = outputArray[2];
            needsQtyPcs = outputArray[3];

            if (needsItemNo == 1) {
                document.getElementById("txtItemNo").disabled = false;
                ValidatorEnable(valReqItemNo, true);

                if (document.getElementById("txtItemNo").value == '') {
                    document.getElementById('txtQtyLbs').value = '';
                }
            } else {
                document.getElementById("txtItemNo").disabled = true;
                document.getElementById("txtItemNo").value = '';
                ValidatorEnable(valReqItemNo, false);
            }

            if (needsLbs == 1 || needsQtyPcs == 1) {
                document.getElementById("txtQtyLbs").disabled = false;
                ValidatorEnable(valQtyLbsRange, true);
            } else {
                document.getElementById("txtQtyLbs").disabled = true;
                document.getElementById("txtQtyLbs").value = '1';
                ValidatorEnable(valQtyLbsRange, false);
            }

            if (needsQtyPcs == 1) {
                ValidatorEnable(valQtyReq, true);
            } else {
                ValidatorEnable(valQtyReq, false);
            }

            if (needsLbs == 1) {
                ValidatorEnable(valLbsReq, true);
            } else {
                ValidatorEnable(valLbsReq, false);
            }

            if (errorText.length > 0) {
                //alert(result.d);
                $("#valUpcCustom").text(errorText);
                $('#valUpcCustom').css({ 'display': 'inherit' });
                document.getElementById("webbarcode").value = '';
                passedValidation = 0;
            } else {
                passedValidation = 1;
            }
        }

        function AjaxFailed(result) {
            alert("Error on Page");
        }

        function icodyDidScanBarcode(value, typeID, typeValue, date) {
            PageMethod("validatePickingBarcodeViaJSON", ["barcode", value, "lbs",
                document.getElementById('txtQtyLbs').value, "itemNo", document.getElementById('txtItemNo').value], AjaxSucceeded, AjaxFailed);

            if ($('#valQtyReq').css('display') !== 'none') {
                passedValidation = "0"
            }

            if ($('#valQtyLbsRange').css('display') !== 'none') {
                passedValidation = "0"
            }

            if ($('#valLbsReq').css('display') !== 'none') {
                passedValidation = "0"
            }

            if ($('#valReqItemNo').css('display') !== 'none') {
                passedValidation = "0"
            }

            if (passedValidation == "1") {
                document.forms["form1"].elements["webbarcode"].value = value;
                document.forms["form1"].submit();
                return "playSuccessSound:0";
            } else {
                return "playFailureSound:3";
            }
        }

        function manualSubmitForm() {
            PageMethod("validatePickingBarcodeViaJSON", ["barcode", document.getElementById('webbarcode').value, "lbs",
                document.getElementById('txtQtyLbs').value, "itemNo", document.getElementById('txtItemNo').value], AjaxSucceeded, AjaxFailed);

            if ($('#valQtyReq').css('display') !== 'none') {
                passedValidation = "0"
            }

            if ($('#valQtyLbsRange').css('display') !== 'none') {
                passedValidation = "0"
            }

            if ($('#valLbsReq').css('display') !== 'none') {
                passedValidation = "0"
            }

            if ($('#valReqItemNo').css('display') !== 'none') {
                passedValidation = "0"
            }

            if (passedValidation == "1") {
                document.forms["form1"].submit();
                return "playSuccessSound:0";
            } else {
                return "playFailureSound:3";
            }
        }

        function serialSubmitForm() {
            document.forms["form1"].submit();
            cleanForm();
        }

        function cleanForm() {
            document.forms[0].reset();

            if (window.Page_Validators)
                for (var vI = 0; vI < Page_Validators.length; vI++) {
                    var vValidator = Page_Validators[vI];
                    vValidator.isvalid = true;
                    ValidatorUpdateDisplay(vValidator);
                }

            return false;
        }

        function cancelEdit() {
            document.getElementById('webbarcode').disabled = false;
            document.getElementById('webbarcode').value = '';
            document.getElementById('txtQtyLbs').value = '1';
            document.getElementById('txtItemNo').disabled = false;
            document.getElementById('txtItemNo').value = '';

            return false;
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            font-family: arial;
            font-size: 14px;
            font-weight: normal;
            height: 27px;
        }
        .auto-style2 {
            height: 5px;
        }
    </style>
</head>

<body bgcolor="#FFFFFF">
    <form id="form1" runat="server">
        <table border="0" cellpadding="1" cellspacing="0" bgcolor="#FFFFFF" align="center">
            <tr>
                <td colspan="2" align="center">
                    <asp:Image ID="imgLogo" runat="server"
                        ImageUrl="~/Images/logo_small.gif" /></td>
            </tr>
            <tr>
                <td></td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <h3>Order - Scan</h3>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 5px"></td>
            </tr>
            <tr>
                <td colspan="2" class="td_text"><strong>Customer</strong></td>
            </tr>
            <tr>
                <td colspan="2" class="td_text" style="font-size: 18px">#<asp:Label ID="lblCustomerNum" runat="server" Text="" Font-Size="18px"></asp:Label>
                    -
                    <asp:Label ID="lblCustomerDesc" runat="server" Text="" Font-Size="18px"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 5px"></td>
            </tr>
            <tr>
                <td class="td_text" nowrap><strong>Pcs/Lbs: </strong>
                    <asp:TextBox ID="txtQtyLbs" runat="server" MaxLength="5" Width="35px"></asp:TextBox>&nbsp;</td>
                <td class="td_text" nowrap><strong>Barcode: </strong>
                    <asp:TextBox ID="webbarcode" runat="server" MaxLength="52" AutoPostBack="false"
                        Width="110px" Text=""></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2" class="auto-style2"></td>
            </tr>
            <tr>
                <td colspan="2" class="auto-style1" nowrap><strong>Item #: </strong>
                    <asp:TextBox ID="txtItemNo" runat="server" AutoPostBack="false"
                        Width="225px" style="margin-left: 10px"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2" class="td_text" nowrap><strong>Serial #: </strong>
                    <asp:TextBox ID="txtSerialNo" runat="server" AutoPostBack="false"
                        Width="225px" type="text" pattern="[0-9]*" onKeypress="if(event.keyCode < 48 || event.keyCode > 57){return false;}"></asp:TextBox></td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:CustomValidator ID="valCustomerNumber" runat="server" CssClass="td_text" ForeColor="Red" EnableClientScript="true"
                        Display="Dynamic" ValidationGroup="Scan">Customer must be specified. Please click [Change Customer] to specify customer.<br /></asp:CustomValidator>
                    <asp:RequiredFieldValidator ID="valQtyReq" runat="server" ValidationGroup="Scan"
                        ErrorMessage="RequiredFieldValidator" ControlToValidate="txtQtyLbs" SetFocusOnError="true" EnableClientScript="true"
                        CssClass="td_text" ForeColor="Red" Display="Dynamic" Enabled="true">PCS (QUANTITY) IS REQUIRED.<br /></asp:RequiredFieldValidator>
                    <asp:RequiredFieldValidator ID="valLbsReq" runat="server" EnableClientScript="true"
                        ErrorMessage="RequiredFieldValidator" ControlToValidate="txtQtyLbs" SetFocusOnError="true" ValidationGroup="Scan"
                        CssClass="td_text" ForeColor="Red" Display="Dynamic" Enabled="true">WEIGHT IS REQUIRED. Please enter it and then scan again.<br /></asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="valQtyLbsRange" runat="server" ValidationGroup="Scan" EnableClientScript="true"
                        ErrorMessage="Pcs/Lbs must be geater than 0.<br />" ControlToValidate="txtQtyLbs" SetFocusOnError="true"
                        CssClass="td_text" ForeColor="Red" Display="Dynamic" MinimumValue="1"
                        MaximumValue="999999"></asp:RangeValidator>
                    <%--<asp:RequiredFieldValidator ID="valUpcReq" runat="server" 
                ErrorMessage="RequiredFieldValidator" ControlToValidate="webbarcode" 
                CssClass="td_text" ForeColor="Red" Display="Dynamic" Enabled="true">Barcode required. Please scan or type it.<br /></asp:RequiredFieldValidator>--%>
                    <asp:CustomValidator ID="valUpcCustom" runat="server" ControlToValidate="" CssClass="td_text" ForeColor="Red"
                        Display="Dynamic" ValidationGroup="Scan"></asp:CustomValidator>
                    <asp:RequiredFieldValidator ID="valReqItemNo" runat="server" EnableClientScript="true"
                        ErrorMessage="RequiredFieldValidator" ControlToValidate="txtItemNo" SetFocusOnError="false" ValidationGroup="Scan"
                        CssClass="td_text" ForeColor="Red" Display="Dynamic" Enabled="true">ITEM # IS REQUIRED. Please enter it and then scan again.<br /></asp:RequiredFieldValidator>
                    <%--<asp:CustomValidator ID="valItemNo" runat="server" ControlToValidate="txtItemNo" CssClass="td_text" ForeColor="Red" ValidationGroup="Scan"
                Display="Dynamic">Item # does not exist in CounterPoint. Please enter a valid item #. Please enter it and then click [Next Scan].</asp:CustomValidator>--%>
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 5px"></td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <table border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF" align="center">
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblCancelEditSpacer" runat="server" Text="" Height="30" Visible="false"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td width="5"></td>
                            <td align="right">
                                <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel Edit" CausesValidation="False" Width="135px" Height="30px" CssClass="td_text_larger" Visible="false"
                                    OnClientClick="cancelEdit();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnNextScan" runat="server" Text="Next Scan" CausesValidation="true" OnClientClick="cleanForm();"
                        Width="290px" Height="45px" CssClass="td_text_larger" Visible="false" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 5px"></td>
            </tr>
            <tr>
                <td colspan="2" align="center" class="td_text">Line Item:
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
                            "SCAN_ORD_ITEM_NO") & "?"");" %>'
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
                <td colspan="2" style="height: 5px"></td>
            </tr>
            <tr>
                <td colspan="2" style="height: 5px; text-align: center">
                    <asp:Button ID="btnSaveEdit" runat="server" Text="Save Edit" CausesValidation="False" Width="290px" CssClass="td_text_larger" Height="45px" Visible="false" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 5px"></td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnDoneScanning" runat="server" Text="Finish"
                        Width="290px" Height="45px" CssClass="td_text_larger"
                        CausesValidation="False" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 20px"></td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnSaveOrder" runat="server" Text="Save for Later"
                        Width="290px" Height="45px" CssClass="td_text_larger"
                        CausesValidation="False" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 20px"></td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnMainMenu" runat="server" Text="Main Menu" Width="290px" CssClass="td_text_larger"
                        Height="45px" CausesValidation="False"
                        OnClientClick="return confirm('Are you sure you want to navigate away from this page and go to the Main Menu?')" />
                </td>
            </tr>
            <tr>
                <td colspan="2" style="height: 20px"></td>
            </tr>
            <tr>
                <td colspan="2" style="height: 20px"></td>
            </tr>
            <tr>
                <td colspan="2" style="height: 20px"></td>
            </tr>
            <tr>
                <td colspan="2" style="height: 20px"></td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <asp:Button ID="btnCancelOrder" runat="server" Text="Cancel Order" CssClass="td_text_larger"
                        Width="290px" Height="45px"
                        OnClientClick="return confirm('Are you sure you want to CANCEL this order?')" />
                </td>
            </tr>
            <tr>
                <td colspan="2" class="td_text_small_subdued">Picking ID: 
            <asp:Label ID="lblPickingId" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>

        <asp:TextBox ID="txtOrderId" runat="server" Visible="false"></asp:TextBox>

    </form>
</body>
</html>
