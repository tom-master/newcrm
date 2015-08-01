<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptUserAddUser.aspx.cs" Inherits="NewCRM.Web.AdminPage.DeptUserAddUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></f:PageManager>
    <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px" Layout="Fit">
        <Toolbars>
            <f:Toolbar ID="Toolbar1" runat="server">
                <Items>
                    <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server" Text="关闭">
                    </f:Button>
                    <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                    </f:ToolbarSeparator>
                    <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose" OnClick="btnSaveClose_Click" runat="server" Text="选择后关闭">
                    </f:Button>
                    <f:ToolbarFill runat="server">
                    </f:ToolbarFill>
                    <f:TwinTriggerBox ID="ttbSearchMessage" Width="160px" runat="server" ShowLabel="false" EmptyText="在用户名称中搜索" Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" OnTrigger2Click="ttbSearchMessage_Trigger2Click" OnTrigger1Click="ttbSearchMessage_Trigger1Click">
                    </f:TwinTriggerBox>
                </Items>
            </f:Toolbar>
        </Toolbars>
        <Items>
            <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="true" DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortDirection="DESC" AllowPaging="true" IsDatabasePaging="true" OnPageIndexChange="Grid1_PageIndexChange" ClearSelectedRowsAfterPaging="false" SortField="Name">
                <Columns><f:RowNumberField></f:RowNumberField>
                    <f:BoundField DataField="Name" SortField="Name" Width="100px" HeaderText="用户名"></f:BoundField>
                    <f:BoundField DataField="ChineseName" SortField="RealName" Width="100px" HeaderText="中文名"></f:BoundField>
                    <f:CheckBoxField DataField="Enabled" SortField="Enabled" HeaderText="启用" RenderAsStaticField="true" Width="50px"></f:CheckBoxField>
                    <f:BoundField DataField="Gender" SortField="Gender" Width="50px" HeaderText="性别"></f:BoundField>
                    <f:BoundField DataField="Email" SortField="Email" Width="150px" HeaderText="邮箱"></f:BoundField>
                    <f:BoundField DataField="Remark" ExpandUnusedSpace="true" HeaderText="备注"></f:BoundField>
                </Columns>
                <PageItems>
                    <f:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                    </f:ToolbarSeparator>
                    <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                    </f:ToolbarText>
                    <f:DropDownList ID="ddlGridPageSize" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlGridPageSize_SelectedIndexChanged" runat="server">
                        <f:ListItem Text="10" Value="10"></f:ListItem>
                        <f:ListItem Text="20" Value="20"></f:ListItem>
                        <f:ListItem Text="50" Value="50"></f:ListItem>
                        <f:ListItem Text="100" Value="100"></f:ListItem>
                    </f:DropDownList>
                </PageItems>
            </f:Grid>
        </Items>
    </f:Panel>
    <f:HiddenField ID="hfSelectedIDS" runat="server">
    </f:HiddenField>
    </form>
</body>
</html>
