<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserSelectDept.aspx.cs" Inherits="NewCRM.Web.AdminPage.UserSelectDept" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="Grid1" runat="server"></f:PageManager>
        <f:Grid ID="Grid1" runat="server" ShowBorder="false" ShowHeader="false" EnableCheckBoxSelect="true" DataKeyNames="ID,Name" EnableMultiSelect="false" OnRowDataBound="Grid1_RowDataBound">
            <Toolbars>
                <f:Toolbar ID="Toolbar2" Position="Top" runat="server">
                    <Items>
                        <f:Button ID="btnClose" Icon="SystemClose" EnablePostBack="false" runat="server" Text="关闭">
                        </f:Button>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                        </f:ToolbarSeparator>
                        <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose" OnClick="btnSaveClose_Click" runat="server" Text="选择后关闭">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Columns><f:RowNumberField></f:RowNumberField>
                <f:BoundField DataField="Name" HeaderText="部门名称" DataSimulateTreeLevelField="TreeLevel" Width="150px"></f:BoundField>
                <f:BoundField DataField="Remark" HeaderText="部门描述" ExpandUnusedSpace="true"></f:BoundField>
            </Columns>
        </f:Grid>
    </form>
</body>
</html>
