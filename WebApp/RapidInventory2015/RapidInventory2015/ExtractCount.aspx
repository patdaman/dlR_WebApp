<%@ Page Title="" Language="C#" AutoEventWireup="false" MasterPageFile="~/InventoryMaster.Master" CodeBehind="ExtractCount.aspx.cs" Inherits="RapidInventoryWebApp.ExtractCount" %>
<%@ MasterType VirtualPath="~/InventoryMaster.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
	<div style="text-align:center; font-size:30px">
		Press the button below to extract your counts. Follow this link below to see how to load the file into CounterPoint.
		<a href="http://counterpointuniversity.com/"> CounterPoint University </a>
	</div>
	<div style="text-align:center">
		<asp:Button ID="btnExtract" runat="server" Text="Extract Count"/>
	</div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>