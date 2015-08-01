<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LogView.aspx.cs" Inherits="NewCRM.Web.AdminPage.LogView" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></f:PageManager>
        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" AutoScroll="true" runat="server">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server">
                    <Items>
                        <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server" Text="关闭">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px" Title="SimpleForm">
                    <Items>
                        <f:Label ID="labId" runat="server" Label="ID">
                        </f:Label>
                        <f:Label ID="labDateTime" runat="server" Label="时间">
                        </f:Label>
                        <f:Label ID="labLevel" runat="server" Label="级别">
                        </f:Label>
                        <f:Label ID="labSource" runat="server" Label="源">
                        </f:Label>
                        <f:Label ID="labMessage" runat="server" Label="错误信息">
                        </f:Label>
                        <f:Label ID="labException" runat="server" Label="异常消息">
                        </f:Label>
                    </Items>
                </f:SimpleForm>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
