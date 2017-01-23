<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UploadItemData.aspx.vb" Inherits="RapidInventroyWebApp.UploadItemData" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="CSS/Styles.css" rel="stylesheet" runat="server"/>
</head>
<body>

    <header id="top" class="top">Rapid Inventory</header>

    <form id="dashboard" runat="server">
        <div>
        <table>
            <tr>
                <td>
                    <div class="dashboardItem">
                    <div class="dashboardItemHeader">Query for Export</div>
                    <div id="queryforexport" class="dashboardCountItemBody" />
                        <div style="border:solid">
                            <asp:Label ID="lblQueryforExport" runat="server" Text=""></asp:Label>
                        </div>
                    </div>
                </td>
                <td>
                    <div class="dashboardItem">
                    <div class="dashboardItemHeader">File Upload</div>
                    <div id="uploaditem" class="dashboardCountItemBody" />
                        <div >
                            <div>
                        File Name: 
                        <asp:FileUpload ID="ItemFileUpload" runat="server" />
                        <div>
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="button" />
                        </div>
                        <div><asp:Label ID="lblErrMsg" runat="server" Text=""></asp:Label>
                        </div>
                        </div>
                    </div>
                    </div>
                </td>
            </tr>
        </table>
            </div>
        <div class="bottom">&copy; Rapid POS 2014
            </div>
        
    </form>
    
</body>
</html>
