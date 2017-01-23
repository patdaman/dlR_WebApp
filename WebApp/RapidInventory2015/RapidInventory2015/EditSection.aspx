﻿<%@ Page Title="" Language="C#" AutoEventWireup="false" MasterPageFile="~/InventoryMaster.Master" CodeBehind="EditSection.aspx.cs" Inherits="RapidInventoryWebApp.EditSection" %>

<%@ MasterType VirtualPath="~/InventoryMaster.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
	<div style="width: 100%;">
		<div style="height: 20px;"></div>
		<div style="width: 100%; text-align: center;"></div>
		<div class="dashboardItemBody" style="text-align: center; width: 80%; margin-left: auto; margin-right: auto;">
			<asp:GridView ID="gridSectionScans" runat="server" AutoGenerateColumns="False" ShowFooter="false" Width="100%"
				CssClass="grid"
				DataKeyNames="SCAN_DAT,BARCOD,HANDHELD_ID" CellPadding="4" ForeColor="#333333"
				GridLines="None" OnRowCancelingEdit="gridSectionScans_RowCancelingEdit"
				OnRowEditing="gridSectionScans_RowEditing"
				OnRowUpdating="gridSectionScans_RowUpdating"
				OnRowDataBound="gridSectionScans_RowDataBound"
				OnRowDeleting="gridSectionScans_RowDeleting" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" AllowPaging="True">
				<AlternatingRowStyle BackColor="White" />
				<Columns>
					<asp:TemplateField HeaderText="">
						<ItemTemplate>
							<asp:Button ID="lnkEdit" runat="server" Text="Edit" CommandName="Edit" ToolTip="Edit"
								CommandArgument=''></asp:Button>
							<asp:Button ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete"
								ToolTip="Delete" OnClientClick='return confirm("Are you sure you want to delete this entry?");'
								CommandArgument=''></asp:Button>
						</ItemTemplate>
						<EditItemTemplate>
							<asp:Button ID="lnkInsert" runat="server" Text="Update" ValidationGroup="editGrp" CommandName="Update" ToolTip="Save"
								CommandArgument=''></asp:Button>
							<asp:Button ID="lnkCancel" runat="server" Text="Cancel" CommandName="Cancel" ToolTip="Cancel"
								CommandArgument=''></asp:Button>
						</EditItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="ScanDate">
						<EditItemTemplate>
							<asp:Label ID="txtScanDate" runat="server" Text='<%#Bind("SCAN_DAT")%>' CssClass="" MaxLength="30"></asp:Label>
						</EditItemTemplate>
						<ItemTemplate>
							<asp:Label ID="lblScanDate" runat="server" Text='<%#Bind("SCAN_DAT")%>'></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Barcode">
						<EditItemTemplate>
							<asp:TextBox ID="txtBarcode" runat="server" Text='<%#Bind("BARCOD")%>' CssClass="" MaxLength="30"></asp:TextBox>
						</EditItemTemplate>
						<ItemTemplate>
							<asp:Label ID="lblBarcode" runat="server" Text='<%#Bind("BARCOD")%>'></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Item No">
						<EditItemTemplate>
							<asp:Label ID="lblitemNo" runat="server" Text='<%#Bind("ITEM_NO")%>'></asp:Label>
						</EditItemTemplate>
						<ItemTemplate>
							<asp:Label ID="lblitemNo" runat="server" Text='<%#Bind("ITEM_NO")%>'></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Description">
						<EditItemTemplate>
							<asp:Label ID="lblDescription" runat="server" Text='<%#Bind("DESCR")%>'></asp:Label>
						</EditItemTemplate>
						<ItemTemplate>
							<asp:Label ID="lblDescription" runat="server" Text='<%#Bind("DESCR")%>'></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Quantity">
						<EditItemTemplate>
							<asp:TextBox ID="txtQuantity" runat="server" Text='<%#Bind("CNT_QTY")%>' CssClass="" MaxLength="5"></asp:TextBox>
							<asp:RequiredFieldValidator ID="valQuantity" runat="server" ControlToValidate="txtQuantity"
								Display="Dynamic" ErrorMessage="Field is required." ForeColor="Red" SetFocusOnError="True"
								ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
						</EditItemTemplate>
						<ItemTemplate>
							<asp:Label ID="lblQuantity" runat="server" Text='<%#Bind("CNT_QTY")%>'></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Handheld">
						<EditItemTemplate>
							<asp:TextBox ID="txtHandheldId" runat="server" Text='<%#Bind("HANDHELD_ID")%>' CssClass="" MaxLength="10"></asp:TextBox>
							<asp:RequiredFieldValidator ID="valHandheldId" runat="server" ControlToValidate="txtHandheldId"
								Display="Dynamic" ErrorMessage="Field is required." ForeColor="Red" SetFocusOnError="True"
								ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
						</EditItemTemplate>
						<ItemTemplate>
							<asp:Label ID="lblHandheldId" runat="server" Text='<%#Bind("HANDHELD_ID")%>'></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Forced">
						<EditItemTemplate>
							<asp:Label ID="lblForced" runat="server" Text='<%#Bind("FORCED")%>'></asp:Label>
						</EditItemTemplate>
						<ItemTemplate>
							<asp:Label ID="lblForced" runat="server" Text='<%#Bind("FORCED")%>'></asp:Label>
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
				<EditRowStyle BackColor="#2461BF" />
				<FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
				<HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
				<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
				<RowStyle BackColor="#EFF3FB" Wrap="True" />
				<SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
				<SortedAscendingCellStyle BackColor="#F5F7FB" />
				<SortedAscendingHeaderStyle BackColor="#6D95E1" />
				<SortedDescendingCellStyle BackColor="#E9EBEF" />
				<SortedDescendingHeaderStyle BackColor="#4870BE" />

			</asp:GridView>
		</div>
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>