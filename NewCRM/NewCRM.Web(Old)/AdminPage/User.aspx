<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="User.aspx.cs" Inherits="NewCRM.Web.AdminPage.User" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></f:PageManager>
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px" ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start" ShowHeader="false" Title="用户管理">
            <Items>
                <f:Form ID="Form2" runat="server" Height="36px" BodyPadding="5px" ShowHeader="false" ShowBorder="false" LabelAlign="Right">
                    <Rows>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:TwinTriggerBox ID="ttbSearchMessage" runat="server" ShowLabel="false" EmptyText="在用户名称中搜索" Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" OnTrigger2Click="ttbSearchMessage_Trigger2Click" OnTrigger1Click="ttbSearchMessage_Trigger1Click">
                                </f:TwinTriggerBox>
                                <f:RadioButtonList ID="rblEnableStatus" AutoPostBack="true" OnSelectedIndexChanged="rblEnableStatus_SelectedIndexChanged" Label="启用状态" ColumnNumber="3" runat="server">
                                    <f:RadioItem Text="全部" Selected="true" Value="all"></f:RadioItem>
                                    <f:RadioItem Text="启用" Value="enabled"></f:RadioItem>
                                    <f:RadioItem Text="禁用" Value="disabled"></f:RadioItem>
                                </f:RadioButtonList>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
                <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="true" DataKeyNames="ID,Name" AllowSorting="true" OnSort="Grid1_Sort" SortDirection="DESC" AllowPaging="true" IsDatabasePaging="true" OnPreDataBound="Grid1_PreDataBound" OnPreRowDataBound="Grid1_PreRowDataBound" OnRowCommand="Grid1_RowCommand" OnPageIndexChange="Grid1_PageIndexChange" SortField="Name">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" runat="server">
                            <Items>
                                <f:Button ID="btnDeleteSelected" Icon="Delete" runat="server" Text="删除选中记录" OnClick="btnDeleteSelected_Click">
                                </f:Button>
                                <f:ToolbarSeparator runat="server">
                                </f:ToolbarSeparator>
                                <f:Button ID="btnChangeEnableUsers" Icon="GroupEdit" EnablePostBack="false" runat="server" Text="设置启用状态">
                                    <Menu runat="server">
                                        <f:MenuButton ID="btnEnableUsers" OnClick="btnEnableUsers_Click" runat="server" Text="启用选中记录">
                                        </f:MenuButton>
                                        <f:MenuButton ID="btnDisableUsers" OnClick="btnDisableUsers_Click" runat="server" Text="禁用选中记录">
                                        </f:MenuButton>
                                    </Menu>
                                </f:Button>
                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="false" Text="新增用户">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <PageItems>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
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
                    <Columns><f:RowNumberField EnablePagingNumber="true"></f:RowNumberField>
                        <f:BoundField DataField="Name" SortField="Name" Width="100px" HeaderText="用户名"></f:BoundField>
                        <f:BoundField DataField="ChineseName" SortField="ChineseName" Width="100px" HeaderText="中文名"></f:BoundField>
                        <f:CheckBoxField DataField="Enabled" SortField="Enabled" HeaderText="启用" RenderAsStaticField="true" Width="50px"></f:CheckBoxField>
                        <f:BoundField DataField="Gender" SortField="Gender" Width="50px" HeaderText="性别"></f:BoundField>
                        <f:BoundField DataField="Email" SortField="Email" Width="150px" HeaderText="邮箱"></f:BoundField>
                        <f:BoundField DataField="Remark" ExpandUnusedSpace="true" HeaderText="备注"></f:BoundField>
                        <f:WindowField TextAlign="Center" Icon="Information" ToolTip="查看详细信息" Title="查看详细信息" WindowID="Window1" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/AdminPage/UserView.aspx?id={0}" Width="50px"></f:WindowField>
                        <f:WindowField ColumnID="changePasswordField" TextAlign="Center" Icon="Key" ToolTip="修改密码" WindowID="Window1" Title="修改密码" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/AdminPage/UserChangePassword.aspx?id={0}" Width="50px"></f:WindowField>
                        <f:WindowField ColumnID="editField" TextAlign="Center" Icon="Pencil" ToolTip="编辑" WindowID="Window1" Title="编辑" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/AdminPage/UserEdit.aspx?id={0}" Width="50px"></f:WindowField>
                        <f:LinkButtonField ColumnID="deleteField" TextAlign="Center" Icon="Delete" ToolTip="删除" ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="50px"></f:LinkButtonField>
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="650px" Height="450px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
