<%@ Page Language="VB" AutoEventWireup="false" CodeFile="ReceiveScan.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<script runat="server">

    Protected Sub Page_Load(sender As Object, e As EventArgs)

    End Sub

</script>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rapid POS Mobile WL - Recieve - Scan</title>
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

        function PageMethod(fn, paramArray, successFn, errorFn) 
        {
            var pagePath = window.location.pathname;
                       
            //Create list of parameters in the form : {"paramName1":"paramValue1","paramName2":"paramValue2"} 
            var paramList = ''; 
            if (paramArray.length > 0) 
            { 
                for (var i=0; i<paramArray.length; i+=2) 
                { 
                    if (paramList.length > 0)
                        paramList += ','; 
                    paramList += '"' + paramArray[i] + '":"' + paramArray[i+1] + '"'; 
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

        function AjaxSucceeded(result) 
        {
            var errorText;
            var setWeightValueTo;
            var outputArray = result.d.split('|');
            errorText = outputArray[0];
            setWeightValueTo = outputArray[1];

            if (setWeightValueTo > 0) {
                document.getElementById("txtQtyPcs").value = setWeightValueTo;
            }

            if (errorText.length > 0) {
                //alert(result.d);
                $("#valUpcCustom").text(errorText);
                $('#valUpcCustom').css({ 'display': 'inherit' });
                passedValidation = 0;
            } else {
                passedValidation = 1;
            }
        }

        function AjaxFailed (result)
        {
            alert("Error on Page");
        }

        function icodyDidScanBarcode(value, typeID, typeValue, date) {
            PageMethod("validateReceiverBarcodeViaJSON", ["barcode", value, "vendor", document.getElementById('lblVendorNum').innerHTML], AjaxSucceeded, AjaxFailed);

            if ($('#valQtyReq').css('display') !== 'none') {
                passedValidation = "0"
            }

            if ($('#valQtyRange').css('display') !== 'none') {
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

        function doSubmit() {
            document.forms["form1"].submit();
        }

        function manualSubmitForm() {
            PageMethod("validateReceiverBarcodeViaJSON", ["barcode", document.getElementById('webbarcode').value, "vendor", document.getElementById('lblVendorNum').innerHTML],
                AjaxSucceeded, AjaxFailed);

            if ($('#valQtyReq').css('display') !== 'none') {
                passedValidation = "0"
            }

            if ($('#valQtyRange').css('display') !== 'none') {
                passedValidation = "0"
            }

            if (passedValidation == "1") {
                document.forms["form1"].submit();
                return "playSuccessSound:0";
            } else {
                return "playFailureSound:3";
            }
        }
        
        function cleanForm() {
            document.forms[0].reset();

            document.getElementById('webbarcode').value = '';
            document.getElementById('txtQtyPcs').value = '1';

            if (window.Page_Validators)
                for (var vI = 0; vI < Page_Validators.length; vI++) {
                    var vValidator = Page_Validators[vI];
                    vValidator.isvalid = true;
                    ValidatorUpdateDisplay(vValidator);
                }

            location.reload();

            return false;
        }
        
        function cancelEdit() {
            document.getElementById('webbarcode').disabled = false;
            document.getElementById('webbarcode').value = '';
            document.getElementById('txtQtyPcs').value = '1';

            return false;
        }
    </script>
   
</head>
<body bgcolor="#FFFFFF" >
    <form id="form1" runat="server">
    <table border="0" cellpadding="1" cellspacing="0" bgcolor="#FFFFFF" align="center">    
    <tr>
        <td colspan="7" align="center"><asp:Image ID="imgLogo" runat="server" 
                ImageUrl="~/Images/logo_small.gif" /></td>
    </tr>
    <tr>
        <td colspan="7"></td>
    </tr>    
    <tr>
        <td colspan="7" align="center"><h3>Receive - Scan</h3></td>
    </tr>
    <tr>
        <td colspan="7" style="height:5px"></td>
    </tr>  
    <tr>
        <td colspan="3" align="left">
            <asp:Button ID="btnPrintRcvrReport" runat="server" Text="Print Report" CssClass="td_text" 
                Width="85px" Height="20px" OnClick="btnPrintRcvrReport_Click" />
        </td>
        <td colspan="4" align="right">
            <asp:Button ID="btnCancelReceiver"  runat="server" Text="Cancel Receiver" CssClass="td_text" 
                Width="115px" Height="20px" 
                OnClientClick="return confirm('Are you sure you want to CANCEL this receiver?')" />
        </td>
    </tr>
    <tr>
        <td colspan="7" style="height:5px"></td>
    </tr>
    <tr>
        <td colspan="7" class="td_text"><strong>Vendor</strong></td>
    </tr>
    <tr>
        <td colspan="7" class="td_text">
            #<asp:Label ID="lblVendorNum" runat="server" Text=""></asp:Label> - <asp:Label ID="lblVendorDesc" runat="server" Text=""></asp:Label>           
        </td>
    </tr>
    <tr>
        <td colspan="7" style="height:5px"></td>
    </tr> 

    <tr>
        <td colspan="3" class="td_text"><strong>Pcs: </strong><asp:TextBox ID="txtQtyPcs" runat="server" MaxLength="5" Width="35px"></asp:TextBox></td>
        <td colspan="4" class="td_text"><strong>Barcode: </strong>
            <asp:TextBox ID="webbarcode" runat="server" MaxLength="52" AutoPostBack="False"
                Width="140px" Text=""></asp:TextBox></td>        
    </tr>
    <tr>
        <td colspan="7">
             <%--<asp:CustomValidator ID="valVendorNumber" runat="server" CssClass="td_text" ForeColor="Red"
                Display="Dynamic" ValidationGroup="Scan" EnableClientScript="true">Vendor must be specified. Please click [Change Vendor] to specify vendor.<br /></asp:CustomValidator>--%>
             <asp:RequiredFieldValidator ID="valQtyReq" runat="server" 
                ErrorMessage="RequiredFieldValidator" ControlToValidate="txtQtyPcs"  SetFocusOnError="true" EnableClientScript="true"
                CssClass="td_text" ForeColor="Red" Display="Dynamic" Enabled="true">Pcs (quantity) is required.<br /></asp:RequiredFieldValidator>
            <asp:RangeValidator ID="valQtyRange" runat="server"
                ErrorMessage="Pcs must be geater than 0.<br />" ControlToValidate="txtQtyPcs" SetFocusOnError="true"
                CssClass="td_text" ForeColor="Red" Display="Dynamic" MinimumValue="1" 
                MaximumValue="999999" EnableClientScript="true"></asp:RangeValidator>            
             <%--<asp:RequiredFieldValidator ID="valUpcReq" runat="server" 
                ErrorMessage="RequiredFieldValidator" ControlToValidate="webbarcode" 
                CssClass="td_text" ForeColor="Red" Display="Dynamic" Enabled="true" EnableClientScript="true">Barcode required. Please scan or type it.<br /></asp:RequiredFieldValidator>--%>
            <asp:CustomValidator ID="valUpcCustom" runat="server" ControlToValidate="webbarcode" CssClass="td_text" ForeColor="Red"
                Display="Dynamic" ValidationGroup="Scan"></asp:CustomValidator></td>
    </tr>
    <tr>
        <td colspan="7" style="height:5px"></td>
    </tr> 
    <tr>
        <td colspan="7" align="center">   
            <table border="0" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF" align="center">
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblCancelEditSpacer" runat="server" Text="" Height="30" Visible="false"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="btnSaveEdit" runat="server" Text="Save Edit" CausesValidation="False" Width="135px" Height="30px" CssClass="td_text_larger" Visible="false"/>
                    </td>
                    <td width="5"></td>
                    <td align="right">
                        <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel Edit" CausesValidation="False" Width="135px" Height="30px" CssClass="td_text_larger" Visible="false" 
                            OnClientClick="cancelEdit();"/>
                    </td>
                </tr>
            </table>                        
            
        </td>
    </tr>
    <tr>
        <td colspan="7" align="center" class="td_text">
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
                            "WING_LEE_CARTON_SERIAL")%>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td>Qty/Lbs: <%# DataBinder.Eval(Container.DataItem,
                            "CARTON_PIECES_WEIGHT")%></td>
                    </tr>
                    <tr>
                        <td>Carton Weight: <%# DataBinder.Eval(Container.DataItem,
                            "CARTON_WEIGHT")%></td>
                    </tr>
                    <tr>
                        <td style="border-style: none none dotted none; border-width: thin"><asp:LinkButton ID="lbtnDelete" CommandName="Delete"
                            OnClientClick='<%# "return confirm(""Are you sure you want to remove item #" & DataBinder.Eval(Container.DataItem,
                            "CARTON_ITEM_NO") & "?"")" %>' CommandArgument='<%# Eval("CARTON_NO") %>' 
                            runat="server" Text="Remove" CausesValidation="false" />
                        </td>
                        <td style="border-style: none none dotted none; border-width: thin"></td>
                        <td align="right" style="border-style: none none dotted none; border-width: thin"><asp:LinkButton ID="lbtnEdit" CommandName="Edit"
                            CommandArgument='<%# Eval("CARTON_NO") %>' 
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
        <td colspan="7" style="height:5px"></td>
    </tr> 
    <tr>
        <td colspan="7" align="center">
            <asp:Button ID="btnDoneScanning" runat="server" Text="Finish" 
                Width="290px" Height="45px" CssClass="td_text_larger" 
                CausesValidation="False" OnClientClick="return confirm('Are you sure you're done scanning?');" />
        </td>
    </tr>
    <tr>
        <td colspan="7" style="height:5px"></td>
    </tr> 
    <tr>
        <td colspan="7" align="center">
            <asp:Button ID="btnMainMenu" runat="server" Text="Main Menu" Width="290px" CssClass="td_text_larger" 
                            Height="45px" CausesValidation="False"
                            OnClientClick="return confirm('Are you sure you want to navigate away from this page and go to the Main Menu?');" />           
        </td>
    </tr>
    <tr>
        <td colspan="7" style="height:5px"></td>
    </tr>    
    <tr>
        <td colspan="7" class="td_text_small_subdued">Receiver ID: 
            <asp:Label ID="lblReceiverId" runat="server" Text=""></asp:Label>
        </td>
    </tr>    
    </table>

    <asp:TextBox ID="txtCartonNo" runat="server" Visible="false"></asp:TextBox>
    </form>

</body>
</html>
