<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="NewCRM.Web.AdminPage.Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></f:PageManager>
        <f:Panel ID="Panel1" runat="server" BodyPadding="5px" ShowBorder="false" ShowHeader="false" Layout="Fit">
            <Items>
                <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="false" DataKeyNames="ID" OnPreDataBound="Grid1_PreDataBound" OnRowCommand="Grid1_RowCommand">
                    <Toolbars>
                        <f:Toolbar ID="Toolbar1" Position="Top" runat="server">
                            <Items>

                                <f:ToolbarFill ID="ToolbarFill1" runat="server">
                                </f:ToolbarFill>
                                <f:Button ID="btnNew" runat="server" Icon="Add" EnablePostBack="false" Text="新增菜单">
                                </f:Button>
                            </Items>
                        </f:Toolbar>
                    </Toolbars>
                    <Columns><f:RowNumberField></f:RowNumberField>
                        <f:BoundField DataField="Name" HeaderText="菜单标题" DataSimulateTreeLevelField="TreeLevel" Width="150px"></f:BoundField>
                        <f:BoundField DataField="NavigateUrl" HeaderText="链接" Width="200px"></f:BoundField>
                        <f:BoundField DataField="ViewPower.Name" HeaderText="浏览权限" Width="120px"></f:BoundField>
                        <f:BoundField DataField="Remark" HeaderText="备注" ExpandUnusedSpace="true"></f:BoundField>
                        <f:BoundField DataField="SortIndex" HeaderText="排序" Width="80px"></f:BoundField>
                        <f:WindowField ColumnID="editField" TextAlign="Center" Icon="Pencil" ToolTip="编辑" WindowID="Window1" Title="编辑" DataIFrameUrlFields="ID" DataIFrameUrlFormatString="~/AdminPage/MenuEdit.aspx?id={0}" Width="50px"></f:WindowField>
                        <f:LinkButtonField ColumnID="deleteField" TextAlign="Center" Icon="Delete" ToolTip="删除" ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="50px"></f:LinkButtonField>
                    </Columns>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window1" CloseAction="Hide" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="650px" Height="450px" OnClose="Window1_Close">
        </f:Window>
    </form>
</body>
</html>
