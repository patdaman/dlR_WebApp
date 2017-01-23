﻿<%@ Page Title="" Language="C#" AutoEventWireup="false" MasterPageFile="~/InventoryMaster.Master" CodeBehind="LocationMaintenance.aspx.cs" Inherits="RapidInventoryWebApp.LocationMaintenance" %>

<%@ MasterType VirtualPath="~/InventoryMaster.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

	<style type="text/css">
		.auto-style1 {
			width: 100px;
		}

		.auto-style2 {
			height: 30px;
		}
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
	<div style="width: 100%;">
		<div style="text-align: center;">
			<table style="width: 100%;">
				<tr>
					<td style="text-align: center" class="auto-style2">Company Name
						<asp:TextBox ID="txtboxnewComp" runat="server"></asp:TextBox>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						Location <asp:TextBox ID="txtboxnewLoc" runat="server"></asp:TextBox>
						&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						<asp:Button ID="btnAddnewLocation" runat="server" Text="Add" />
					</td>
				</tr>
			</table>
		</div>
	</div>
	<div>
	</div>
	<div class="dashboardItem">
		<div class="dashboardItemHeader"></div>
		<div class="dashboardItemBody">
			<div style="text-align: center; width: 80%; margin-left: auto; margin-right: auto;">
				<asp:GridView ID="gridLocation" runat="server" AutoGenerateColumns="False" ShowFooter="True" Width="100%"
					CssClass="grid" OnRowCommand="gridLocation_RowCommand"
					DataKeyNames="COMPANY_NAM,LOC_ID,USR_NAM" CellPadding="4" ForeColor="#333333"
					GridLines="None" OnRowCancelingEdit="gridLocation_RowCancelingEdit"
					OnRowEditing="gridLocation_RowEditing"
					OnRowUpdating="gridLocation_RowUpdating"
					OnRowDataBound="gridLocation_RowDataBound"
					OnRowDeleting="gridLocation_RowDeleting" RowStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" AllowPaging="True">
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
							<FooterTemplate>
								<asp:Button ID="lnkInsert" runat="server" Text="Insert" ValidationGroup="newGrp" CommandName="InsertNew" ToolTip="Add New Entry"
									CommandArgument=''></asp:Button>
								<asp:Button ID="lnkCancel" runat="server" Text="Cancel" CommandName="CancelNew" ToolTip="Cancel"
									CommandArgument=''></asp:Button>
							</FooterTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Company Name">
							<EditItemTemplate>
								<asp:Label ID="lblCompanyName" runat="server" Text='<%#Bind("COMPANY_NAM")%>'></asp:Label>
							</EditItemTemplate>
							<ItemTemplate>
								<asp:Label ID="lblCompanyName" runat="server" Text='<%#Bind("COMPANY_NAM")%>'></asp:Label>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox ID="txtCompanyNameNew" runat="server" CssClass="" MaxLength="30" placeholder="Company Name..."></asp:TextBox>
								<asp:RequiredFieldValidator ID="valCompanyNameNew" runat="server" ControlToValidate="txtCompanyNameNew"
									Display="Dynamic" ErrorMessage="Company name is required." ForeColor="Red" SetFocusOnError="True"
									ValidationGroup="newGrp">*</asp:RequiredFieldValidator>
							</FooterTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Location">
							<EditItemTemplate>
								<asp:Label ID="lblLocation" runat="server" Text='<%#Bind("LOC_ID")%>'></asp:Label>
							</EditItemTemplate>
							<ItemTemplate>
								<asp:Label ID="lblLocation" runat="server" Text='<%#Bind("LOC_ID")%>'></asp:Label>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox ID="txtLocationNew" runat="server" CssClass="" placeholder="Location..." MaxLength="30"></asp:TextBox>
								<asp:RequiredFieldValidator ID="valLocationNew" runat="server" ControlToValidate="txtLocationNew"
									Display="Dynamic" ErrorMessage="Location is required." ForeColor="Red" SetFocusOnError="True"
									ValidationGroup="newGrp">*</asp:RequiredFieldValidator>
							</FooterTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Username">
							<EditItemTemplate>
								<asp:TextBox ID="txtUsername" runat="server" Text='<%#Bind("USR_NAM")%>' CssClass="" MaxLength="30"></asp:TextBox>
								<asp:RequiredFieldValidator ID="valusernamre" runat="server" ControlToValidate="txtUsername"
									Display="Dynamic" ErrorMessage="Username is required." ForeColor="Red" SetFocusOnError="True"
									ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
							</EditItemTemplate>
							<ItemTemplate>
								<asp:Label ID="lblUsername" runat="server" Text='<%#Bind("USR_NAM")%>'></asp:Label>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox ID="txtUsernameNew" runat="server" CssClass="" placeholder="Username..." MaxLength="30"></asp:TextBox>
								<asp:RequiredFieldValidator ID="valUsermaneNew" runat="server" ControlToValidate="txtUsernameNew"
									Display="Dynamic" ErrorMessage="Username is required." ForeColor="Red" SetFocusOnError="True"
									ValidationGroup="newGrp">*</asp:RequiredFieldValidator>
							</FooterTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Password">
							<EditItemTemplate>
								<asp:TextBox ID="txtPassword" runat="server" Text='<%#Bind("PWD")%>' CssClass="" MaxLength="30"></asp:TextBox>
								<asp:RequiredFieldValidator ID="valPassword" runat="server" ControlToValidate="txtPassword"
									Display="Dynamic" ErrorMessage="Password is required." ForeColor="Red" SetFocusOnError="True"
									ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
							</EditItemTemplate>
							<ItemTemplate>
								<asp:Label ID="lblPassword" runat="server" Text='<%#Bind("PWD")%>'></asp:Label>
							</ItemTemplate>
							<FooterTemplate>
								<asp:TextBox ID="txtPasswordNew" runat="server" CssClass="" placeholder="Password..." MaxLength="30"></asp:TextBox>
								<asp:RequiredFieldValidator ID="valPasswordNew" runat="server" ControlToValidate="txtPasswordNew"
									Display="Dynamic" ErrorMessage="Password is required." ForeColor="Red" SetFocusOnError="True"
									ValidationGroup="newGrp">*</asp:RequiredFieldValidator>
							</FooterTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Failed Login Count">
							<EditItemTemplate>
								<asp:TextBox ID="txtFld" runat="server" Text='<%#Bind("FLD_LGN_CNT")%>' CssClass="" MaxLength="1"></asp:TextBox>
								<asp:RequiredFieldValidator ID="valFld" runat="server" ControlToValidate="txtFld"
									Display="Dynamic" ErrorMessage="Field is required." ForeColor="Red" SetFocusOnError="True"
									ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
							</EditItemTemplate>
							<ItemTemplate>
								<asp:Label ID="lblFld" runat="server" Text='<%#Bind("FLD_LGN_CNT")%>'></asp:Label>
							</ItemTemplate>
							<FooterTemplate>
								<asp:Label ID="txtFldNew" runat="server" CssClass="" MaxLength="1">0</asp:Label>
							</FooterTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Allow Force?">
							<EditItemTemplate>
								<asp:DropDownList ID="txtForce" runat="server"></asp:DropDownList>
								<asp:RequiredFieldValidator ID="valForce" runat="server" ControlToValidate="txtForce"
									Display="Dynamic" ErrorMessage="Force is required." ForeColor="Red" SetFocusOnError="True"
									ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
							</EditItemTemplate>
							<ItemTemplate>
								<asp:Label ID="lblForce" runat="server" Text='<%#Bind("ALLOW_FORCE")%>'></asp:Label>
							</ItemTemplate>
							<FooterTemplate>
								<asp:DropDownList ID="txtForceNew" runat="server" CssClass="" MaxLength="1"></asp:DropDownList>
								<asp:RequiredFieldValidator ID="valForceNew" runat="server" ControlToValidate="txtForceNew"
									Display="Dynamic" ErrorMessage="Field is required." ForeColor="Red" SetFocusOnError="True"
									ValidationGroup="newGrp">*</asp:RequiredFieldValidator>
							</FooterTemplate>
						</asp:TemplateField>
						<asp:TemplateField HeaderText="Is Manager?">
							<EditItemTemplate>
								<asp:DropDownList ID="txtManager" runat="server"></asp:DropDownList>
								<asp:RequiredFieldValidator ID="valManager" runat="server" ControlToValidate="txtManager"
									Display="Dynamic" ErrorMessage="Field is required." ForeColor="Red" SetFocusOnError="True"
									ValidationGroup="editGrp">*</asp:RequiredFieldValidator>
							</EditItemTemplate>
							<ItemTemplate>
								<asp:Label ID="lblManager" runat="server" Text='<%#Bind("IS_MANAGER")%>'></asp:Label>
							</ItemTemplate>
							<FooterTemplate>
								<asp:DropDownList ID="txtManagerNew" runat="server" CssClass="" MaxLength="1"></asp:DropDownList>
								<asp:RequiredFieldValidator ID="valManagerNew" runat="server" ControlToValidate="txtManagerNew"
									Display="Dynamic" ErrorMessage="Field is required." ForeColor="Red" SetFocusOnError="True"
									ValidationGroup="newGrp">*</asp:RequiredFieldValidator>
							</FooterTemplate>
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
	</div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>