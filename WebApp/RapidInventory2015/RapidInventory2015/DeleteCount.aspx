<%@ Page Title="" Language="C#" AutoEventWireup="false" MasterPageFile="~/InventoryMaster.Master" CodeBehind="DeleteCount.aspx.cs" Inherits="RapidInventoryWebApp.DeleteCount" %>
<%@ MasterType VirtualPath="~/InventoryMaster.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

	<link href="CSS/tables.css" rel="stylesheet" />

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
	<div style="height:20px;"> </div>
	<div style="text-align:center;width:100%;">
			<asp:GridView ID="gridDeleteCount" runat="server" AutoGenerateColumns="False" ShowFooter="false"
						CssClass="grid"
		DataKeyNames="COMPANY_NAM,LOC_ID,MAX_SCAN_DAT" CellPadding="4" ForeColor="#333333"
						GridLines="None"
		onRowdeleting="gridDeleteCount_RowDeleting" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" AllowPaging="True">
						<AlternatingRowStyle BackColor="White" />
						<Columns>
							<asp:TemplateField HeaderText="">
								<ItemTemplate>
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
							<asp:TemplateField HeaderText="Company Name">
								<EditItemTemplate>
									<asp:TextBox ID="txtCompanyName" runat="server" Text='<%#Bind("COMPANY_NAM")%>' CssClass="" MaxLength="30"></asp:TextBox>
									<asp:RequiredFieldValidator ID="valCompany" runat="server" ControlToValidate="txtCompany"
									Display="Dynamic" ErrorMessage="Company Name is required." ForeColor="Red" SetFocusOnError="True"
								   ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
								</EditItemTemplate>
								<ItemTemplate>
									<asp:Label ID="lblCompany" runat="server" Text='<%#Bind("COMPANY_NAM")%>'></asp:Label>
								</ItemTemplate>
							</asp:TemplateField>       
							<asp:TemplateField HeaderText="Location">
								<EditItemTemplate>
									<asp:TextBox ID="txtLocation" runat="server" Text='<%#Bind("LOC_ID")%>' CssClass="" MaxLength="30"></asp:TextBox>
									<asp:RequiredFieldValidator ID="valLocation" runat="server" ControlToValidate="txtLocation"
									Display="Dynamic" ErrorMessage="Location is required." ForeColor="Red" SetFocusOnError="True"
								   ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
								</EditItemTemplate>
								<ItemTemplate>
									<asp:Label ID="lblLocation" runat="server" Text='<%#Bind("LOC_ID")%>'></asp:Label>
								</ItemTemplate>
							</asp:TemplateField>   
							<asp:TemplateField HeaderText="Last Scan Date">
								<EditItemTemplate>
									<asp:TextBox ID="txtScanDate" runat="server" Text='<%#Bind("MAX_SCAN_DAT")%>' CssClass="" MaxLength="30"></asp:TextBox>
									<asp:RequiredFieldValidator ID="valScanDate" runat="server" ControlToValidate="txtScanDate"
									Display="Dynamic" ErrorMessage="ScanDate is required." ForeColor="Red" SetFocusOnError="True"
								   ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
								</EditItemTemplate>
								<ItemTemplate>
									<asp:Label ID="lblScanDate" runat="server" Text='<%#Bind("MAX_SCAN_DAT")%>'></asp:Label>
								</ItemTemplate>
							</asp:TemplateField>                     
						</Columns>
						<EditRowStyle BackColor="#2461BF" />
						<FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
						<HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Center"/>
						<PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
						<RowStyle BackColor="#EFF3FB" Wrap="True" />
						<SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
						<SortedAscendingCellStyle BackColor="#F5F7FB" />
						<SortedAscendingHeaderStyle BackColor="#6D95E1" />
						<SortedDescendingCellStyle BackColor="#E9EBEF" />
						<SortedDescendingHeaderStyle BackColor="#4870BE" />

</asp:GridView>
		</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>