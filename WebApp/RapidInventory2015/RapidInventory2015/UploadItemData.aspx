﻿<%@ Page Title="" Language="C#" AutoEventWireup="false" MasterPageFile="~/InventoryMaster.Master" CodeBehind="UploadItemData.aspx.cs" Inherits="RapidInventoryWebApp.UploadItemData" %>

<%@ MasterType VirtualPath="~/InventoryMaster.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

	<script type="text/javascript">

		function CopyToClipboard() {
			var controlValue = document.getElementById('<%=txtQueryforExport.ClientID%>').innerText;
			window.prompt('Copy to clipboard: Ctrl+C, Enter', controlValue);
			alert("copied to clipboard")
		}

	</script>

	<style type="text/css">
		.auto-style2 {
			width: 50%;
			height: 174px;
		}
		.auto-style3 {
			height: 40px;
		}
	</style>

	<script runat="server">

</script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
	<div>
		<table>
			<tr>
				<td style="font-size: 30px; text-align: center" colspan="2">Counts must be setup in CounterPoint and then data is extracted using the CP Data Interchange Export capability. Please refer to the following link for instructions 
					<a href="http://counterpointuniversity.com/">CounterPoint University </a>
				</td>
			</tr>
			<tr>
				<td style="height: 50px" colspan="2"></td>
			</tr>
			<tr>
				<td style="text-align: center;" colspan="1" class="auto-style2">
						<div style="font-weight:bolder">Query for Export</div>
						<div style="border: solid">
							<asp:TextBox ID="txtQueryforExport" runat="server" Text="" Font-Size="10px" Rows="10" TextMode="MultiLine" ReadOnly="True" Width="95%" Style="resize:none;overflow:hidden" BorderStyle="None"></asp:TextBox>
						</div>
						<div>
							Right click and copy the text above
						</div>
				</td>
				<td style="text-align: center;" colspan="1" class="auto-style2">
					<table style="width: 100%; height: 100%">
						<tr>
							<td style="font-weight:bolder"> File Upload </td>
						</tr>
						<tr>
							<td style="text-align: center; width: 50%" colspan="1">
								<asp:FileUpload ID="ItemFileUpload" runat="server" />
							</td>
						</tr>
						<tr>
							<td style="text-align: center; width: 50%" colspan="1">
								<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="File type not supported" ControlToValidate="ItemFileUpload"
									ValidationExpression="(.+\.([Tt][Xx][Tt])|.+\.([Cc][Ss][Vv])|.+\.([Xx][Ll][Ss]))" ForeColor="Red"></asp:RegularExpressionValidator>
							</td>
						</tr>
						<tr>
							<td class="auto-style3">
								<asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="button" />
							</td>
						</tr>
						<tr>
							<td>
								<asp:Label ID="lblErrMsg" runat="server" Font-Bold="True" Font-Size="Large"></asp:Label>
							</td>
						</tr>
					</table>
				</td>
			</tr>
			<tr>
				<td colspan="2"></td>
			</tr>
		</table>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>