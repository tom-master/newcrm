<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminIndex.aspx.cs" Inherits="NewCRM.Web.AdminPage.AdminIndex" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></f:PageManager>
    <f:Panel ID="Panel1" Layout="Column" ShowBorder="false" ShowHeader="false" runat="server">
        <Items>
            <f:Panel BodyPadding="5px" Width="300px" ShowBorder="false" ShowHeader="false" runat="server">
                <Items>
                    <f:Panel ID="Panel2" Title="系统公告" Height="200px" runat="server" CssStyle="margin-bottom:5px;" BodyPadding="5px" ShowBorder="true" ShowHeader="true">
                        <Items>
                            <f:Label ID="Label1" runat="server" Text="这是系统公告">
                            </f:Label>
                        </Items>
                    </f:Panel>
                    <f:Panel ID="Panel4" Title="系统公告" Height="200px" runat="server" CssStyle="margin-bottom:5px;" BodyPadding="5px" ShowBorder="true" ShowHeader="true">
                        <Items>
                            <f:Label ID="Label3" runat="server" Text="这是系统公告">
                            </f:Label>
                        </Items>
                    </f:Panel>
                </Items>
            </f:Panel>
            <f:Panel ID="Panel5" BodyPadding="5px 5px 5px 0" ColumnWidth="100%" ShowBorder="false" ShowHeader="false" runat="server">
                <Items>
                    <f:Panel ID="Panel3" Title="代办事宜" ColumnWidth="100%" Height="200px" runat="server" CssStyle="margin-bottom:5px;" BodyPadding="5px" ShowBorder="true" ShowHeader="true">
                        <Items>
                            <f:Label ID="Label2" runat="server" Text="这是代办事宜列表">
                            </f:Label>
                        </Items>
                    </f:Panel>
                    <f:Panel ID="Panel6" Title="注意事项" ColumnWidth="100%" Height="200px" runat="server" CssStyle="margin-bottom:5px;" BodyPadding="5px" ShowBorder="true" ShowHeader="true">
                        <Items>
                            <f:Label ID="Label4" runat="server" Text="这是注意事项列表">
                            </f:Label>
                        </Items>
                    </f:Panel>
                </Items>
            </f:Panel>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
