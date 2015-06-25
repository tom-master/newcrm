<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="NewCRM.Web.Main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title>首页</title>
</head>
<body>
      <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="regionPanel" runat="server"></f:PageManager>
    <f:RegionPanel ID="regionPanel" ShowBorder="false" runat="server">
        <Regions>
            <f:Region ID="regionTop" Height="60px" ShowBorder="false" ShowHeader="false" Position="Top" Layout="Fit" runat="server">
                <Toolbars>
                    <f:Toolbar ID="Toolbar1" Position="Bottom" runat="server">
                        <Items>
                            <f:ToolbarText ID="txtUser" runat="server">
                            </f:ToolbarText>
                            <f:ToolbarSeparator runat="server"></f:ToolbarSeparator>
                            <f:ToolbarText ID="txtOnlineUserCount" runat="server">
                            </f:ToolbarText>
                            <f:ToolbarSeparator runat="server"></f:ToolbarSeparator>
                            <f:ToolbarText ID="txtCurrentTime" runat="server">
                            </f:ToolbarText>
                            <f:ToolbarFill runat="server"></f:ToolbarFill>
                            <f:Button ID="btnShowHideHeader" runat="server" Icon="SectionExpanded" ToolTip="隐藏标题栏" EnablePostBack="false">
                            </f:Button>
                            <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server"></f:ToolbarSeparator>
                            <f:Button ID="btnRefresh" runat="server" Icon="ArrowRotateClockwise" ToolTip="刷新主区域内容" EnablePostBack="false">
                            </f:Button>
                            <f:ToolbarSeparator runat="server"></f:ToolbarSeparator>
                            <f:Button ID="btnHelp" EnablePostBack="false" Icon="Help" Text="帮助" runat="server">
                            </f:Button>
                            <f:ToolbarSeparator runat="server"></f:ToolbarSeparator>
                            <f:Button ID="btnExit" runat="server" Icon="UserRed" Text="安全退出" ConfirmText="确定退出系统？" OnClick="btnExit_Click">
                            </f:Button>
                        </Items>
                    </f:Toolbar>
                </Toolbars>
                <Items>
                    <f:ContentPanel ShowBorder="false" ShowHeader="false" BodyStyle="background-color:#1C3E7E;" ID="ContentPanel1" runat="server">
                        <div style="font-size: 20px; color:White; font-weight:bold; padding: 5px 10px; ">
                            <a href="./main.aspx" style="color:White;text-decoration:none;"><%= NewCRM.Common.HelperCode.ConfigHelper.Title %></a>
                            <asp:Literal ID="litProductVersion" runat="server"></asp:Literal>
                        </div>
                    </f:ContentPanel>
                </Items>
            </f:Region>
            <f:Region ID="regionLeft" Split="true" Icon="Outline" EnableCollapse="true" Width="200px" ShowHeader="true" Title="系统菜单" Layout="Fit" Position="Left" runat="server">
            </f:Region>
            <f:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Margins="0 0 0 0" Position="Center" runat="server">
                <Items>
                    <f:TabStrip ID="mainTabStrip" EnableTabCloseMenu="true" ShowBorder="false" runat="server">
                        <Tabs>
                            <f:Tab ID="Tab1" Title="首页" EnableIFrame="true" IFrameUrl="~/AdminPage/AdminIndex.aspx" Icon="House" runat="server">
                            </f:Tab>
                        </Tabs>
                    </f:TabStrip>
                </Items>
            </f:Region>
        </Regions>
    </f:RegionPanel>
    <f:Window ID="Window1" runat="server" IsModal="true" Hidden="true" EnableIFrame="true" EnableResize="true" EnableMaximize="true" IFrameUrl="about:blank" Width="650px" Height="450px">
    </f:Window>
    </form>
    <script src="Scripts/CustomJavaScript/main.js" type="text/javascript"></script>
</body>
</html>
