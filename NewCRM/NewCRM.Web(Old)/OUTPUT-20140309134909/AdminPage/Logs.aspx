<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logs.aspx.cs" Inherits="NewCRM.Web.AdminPage.Logs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></f:PageManager>
    <f:Panel ID="Panel1" runat="server" BodyPadding="5px" ShowBorder="false" Layout="VBox" BoxConfigAlign="Stretch" BoxConfigPosition="Start" ShowHeader="false" Title="日志管理">
        <Items>
            <f:Form ID="Form2" runat="server" Height="36px" BodyPadding="5px" ShowHeader="false" ShowBorder="false" LabelAlign="Right">
                <Rows>
                    <f:FormRow ID="FormRow1" runat="server">
                        <Items>
                            <f:TwinTriggerBox ID="ttbSearchMessage" runat="server" ShowLabel="false" EmptyText="在错误信息中搜索" Trigger1Icon="Clear" Trigger2Icon="Search" ShowTrigger1="false" OnTrigger2Click="ttbSearchMessage_Trigger2Click" OnTrigger1Click="ttbSearchMessage_Trigger1Click">
                            </f:TwinTriggerBox>
                            <f:DropDownList ID="ddlSearchLevel" runat="server" Label="错误级别" AutoPostBack="true" OnSelectedIndexChanged="ddlSearchLevel_SelectedIndexChanged">
                                <f:ListItem Text="全部" Value="ALL" Selected="true"></f:ListItem>
                                <f:ListItem Text="INFO" Value="INFO"></f:ListItem>
                                <f:ListItem Text="DEBUG" Value="DEBUG"></f:ListItem>
                                <f:ListItem Text="WARN" Value="WARN"></f:ListItem>
                                <f:ListItem Text="ERROR" Value="ERROR"></f:ListItem>
                                <f:ListItem Text="FATAL" Value="FATAL"></f:ListItem>
                            </f:DropDownList>
                            <f:DropDownList ID="ddlSearchRange" runat="server" Label="搜索范围" AutoPostBack="true" OnSelectedIndexChanged="ddlSearchRange_SelectedIndexChanged">
                                <f:ListItem Text="全部" Value="ALL"></f:ListItem>
                                <f:ListItem Text="今天" Value="TODAY" Selected="true"></f:ListItem>
                                <f:ListItem Text="最近三天" Value="LAST3DAYS"></f:ListItem>
                                <f:ListItem Text="最近七天" Value="LAST7DAYS"></f:ListItem>
                                <f:ListItem Text="最近一个月" Value="LASTMONTH"></f:ListItem>
                                <f:ListItem Text="最近一年" Value="LASTYEAR"></f:ListItem>
                            </f:DropDownList>
                        </Items>
                    </f:FormRow>
                </Rows>
            </f:Form>
            <f:Grid ID="Grid1" runat="server" BoxFlex="1" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="true" DataKeyNames="Logid" AllowSorting="true" OnSort="Grid1_Sort" SortDirection="DESC" AllowPaging="true" IsDatabasePaging="true" OnPreDataBound="Grid1_PreDataBound" OnPageIndexChange="Grid1_PageIndexChange" OnRowCommand="Grid1_RowCommand" SortField="DatetimeX">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" runat="server">
                        <Items>
                            <f:Button ID="btnDeleteSelected" Icon="Delete" runat="server" Text="删除选中记录" OnClick="btnDeleteSelected_Click">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
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
                <Columns><f:RowNumberField></f:RowNumberField>
                    <f:BoundField DataField="DatetimeX" SortField="DatetimeX" DataFormatString="{0:yyyy-MM-dd HH:mm}" Width="120px" HeaderText="时间"></f:BoundField>
                    <f:BoundField DataField="LogLevel" SortField="LogLevel" Width="50px" HeaderText="级别"></f:BoundField>
                    <f:BoundField DataField="Logger" SortField="Logger" Width="100px" HeaderText="源"></f:BoundField>
                    <f:BoundField DataField="Message" ExpandUnusedSpace="true" HeaderText="错误信息"></f:BoundField>
                    <f:BoundField DataField="Exception" Width="200px" HeaderText="异常信息"></f:BoundField>
                    <f:WindowField Icon="Information" ToolTip="查看详细信息" Title="查看详细信息" WindowID="Window1" DataIFrameUrlFields="Logid" DataIFrameUrlFormatString="~/admin/log_view.aspx?id={0}" Width="50px"></f:WindowField>
                    <f:LinkButtonField ColumnID="deleteField" Icon="Delete" ToolTip="删除" ConfirmText="确定删除此记录？" ConfirmTarget="Top" CommandName="Delete" Width="50px"></f:LinkButtonField>
                </Columns>
            </f:Grid>
        </Items>
    </f:Panel>
    <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" Target="Top" EnableResize="true" EnableMaximize="true" EnableIFrame="true" IFrameUrl="about:blank" Width="650px" Height="450px">
    </f:Window>
    </form>
</body>
</html>
