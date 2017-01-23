﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LocationMaintenance.aspx.vb" Inherits="RapidInventroyWebApp.LocationMaintenance" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="CSS/Styles.css" rel="stylesheet" />
</head>
<body>
    <div>
        <a href="DashBoard.aspx?field1=RAPIDPOS&field2=101">DashBoard</a>
        <a href="DeleteCount.aspx?field1=RAPIDPOS&field2=101">Delete Count</a>
        <a href="LocationMaintenance.aspx?field1=RAPIDPOS&field2=101">Location Maintenance</a>
        <a href="Login.aspx?field1=RAPIDPOS&field2=101">Login</a>
        <a href="UploadItemData.aspx?field1=RAPIDPOS&field2=101">Upload Item Data</a>
    </div>
    <form id="dashboard" runat="server">
        <div class="dashboardItem">
            <div class="dashboardItemHeader">Location Maintenance</div>
            <div class="dashboardItemBody">
    <div>
        <asp:GridView ID="gridLocation" runat="server" AutoGenerateEditButton="True" AutoGenerateColumns="False" AutoGenerateDeleteButton="True" DataKeyNames="COMPANY_NAM,LOC_ID" DataSourceID="invserv" ShowFooter="True" AllowPaging="True" EnableViewState="False">
            <Columns>
                <asp:TemplateField HeaderText="Company Name" SortExpression="COMPANY_NAM">
                    <EditItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Eval("COMPANY_NAM") %>'></asp:Label>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtBoxCompanyName" runat="server" placeholder="Company Name..."></asp:TextBox>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("COMPANY_NAM") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Location" SortExpression="LOC_ID">
                    <EditItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Eval("LOC_ID") %>'></asp:Label>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtBoxLocation" runat="server" placeholder="Location..."></asp:TextBox>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# Bind("LOC_ID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Username" SortExpression="USR_NAM">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("USR_NAM") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtBoxUsername" runat="server" placeholder="Username..."></asp:TextBox>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("USR_NAM") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Password" SortExpression="PWD">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("PWD") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtBoxPassword" runat="server" placeholder="Password..." TextMode="password"></asp:TextBox>
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("PWD") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Failed Login Count" SortExpression="FLD_LGN_CNT">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("FLD_LGN_CNT") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("FLD_LGN_CNT") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Allow Force" SortExpression="ALLOW_FORCE">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("ALLOW_FORCE")%>'></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtBoxAllowForce" runat="server" placeholder="(Y/N)"></asp:TextBox>
                        <asp:Button ID="btnAdd" runat="server" Text="Add" CommandName="Insert" />
                    </FooterTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("ALLOW_FORCE")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="invserv" runat="server" ConnectionString="Data Source=WEBINVSERV;Initial Catalog=RAPIDINVENTORY;Persist Security Info=True;User ID=rapidadmin;Password=DbRposAdmin!" DeleteCommand="DELETE FROM [RAPID_USR_ACCT] WHERE [COMPANY_NAM] = @COMPANY_NAM AND [LOC_ID] = @LOC_ID" InsertCommand="INSERT INTO [RAPID_USR_ACCT] ([COMPANY_NAM], [LOC_ID], [USR_NAM], [PWD], [FLD_LGN_CNT], [ALLOW_FORCE]) VALUES (@COMPANY_NAM, @LOC_ID, @USR_NAM, @PWD, @FLD_LGN_CNT, @ALLOW_FORCE)" ProviderName="System.Data.SqlClient" SelectCommand="SELECT * FROM [RAPID_USR_ACCT]" UpdateCommand="UPDATE [RAPID_USR_ACCT] SET [USR_NAM] = @USR_NAM, [PWD] = @PWD, [FLD_LGN_CNT] = @FLD_LGN_CNT, [ALLOW_FORCE] = @ALLOW_FORCE WHERE [COMPANY_NAM] = @COMPANY_NAM AND [LOC_ID] = @LOC_ID">
            <DeleteParameters>
                <asp:Parameter Name="COMPANY_NAM" Type="String" />
                <asp:Parameter Name="LOC_ID" Type="String" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="COMPANY_NAM" Type="String" />
                <asp:Parameter Name="LOC_ID" Type="String" />
                <asp:Parameter Name="USR_NAM" Type="String" />
                <asp:Parameter Name="PWD" Type="String" />
                <asp:Parameter Name="FLD_LGN_CNT" Type="Int32" />
                <asp:Parameter Name="ALLOW_FORCE" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="USR_NAM" Type="String" />
                <asp:Parameter Name="PWD" Type="String" />
                <asp:Parameter Name="FLD_LGN_CNT" Type="Int32" />
                <asp:Parameter Name="ALLOW_FORCE" Type="String" />
                <asp:Parameter Name="COMPANY_NAM" Type="String" />
                <asp:Parameter Name="LOC_ID" Type="String" />
            </UpdateParameters>
        </asp:SqlDataSource>
        </div>
                </div>
    </div>
    </form>
</body>
</html>
