<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RolePower.aspx.cs" Inherits="NewCRM.Web.AdminPage.RolePower" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style>
        ul.powers li {
            margin: 5px 15px 5px 0;
            display: inline-block;
            min-width: 150px;
        }

            ul.powers li input {
                vertical-align: middle;
            }

            ul.powers li label {
                margin-left: 5px;
            }

        /* 自动换行，放置权限列表过长 */
        .x-grid3-row .x-grid3-cell-inner {
            white-space: normal;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowBorder="false" ShowHeader="false" Split="true" Margins="0 0 0 0" Width="200px" Position="Left" Layout="Fit" BodyPadding="5px 0 5px 5px" runat="server">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="false" DataKeyNames="ID" AllowSorting="true" OnSort="Grid1_Sort" SortDirection="DESC" AllowPaging="false" EnableMultiSelect="false" OnRowClick="Grid1_RowClick" EnableRowClickEvent="true" SortField="Name">
                            <Columns><f:RowNumberField></f:RowNumberField>
                                <f:BoundField DataField="Name" SortField="Name" ExpandUnusedSpace="true" HeaderText="角色名称"></f:BoundField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" ShowBorder="false" ShowHeader="false" Position="Center" Layout="Fit" BodyPadding="5px 5px 5px 0" runat="server">
                    <Items>
                        <f:Grid ID="Grid2" runat="server" ShowBorder="true" ShowHeader="false" EnableCheckBoxSelect="false" DataKeyNames="ModuleId,ModuleName" AllowSorting="true" OnSort="Grid2_Sort" OnRowDataBound="Grid2_RowDataBound" SortDirection="DESC" AllowPaging="false" SortField="GroupName">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                        <f:Button ID="Button1" EnablePostBack="false" runat="server" Text="全选/取消">
                                            <Menu ID="Menu1" runat="server">
                                                <f:MenuButton ID="btnSelectAll" EnablePostBack="false" runat="server" Text="全选">
                                                </f:MenuButton>
                                                <f:MenuButton ID="btnSelectRows" EnablePostBack="false" runat="server" Text="全选选中行">
                                                </f:MenuButton>
                                                <f:MenuButton ID="btnUnselectAll" EnablePostBack="false" runat="server" Text="取消">
                                                </f:MenuButton>
                                                <f:MenuButton ID="btnUnselectRows" EnablePostBack="false" runat="server" Text="取消选中行">
                                                </f:MenuButton>
                                            </Menu>
                                        </f:Button>
                                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                                        </f:ToolbarSeparator>
                                        <f:Button ID="btnGroupUpdate" Icon="GroupEdit" runat="server" Text="更新当前角色的权限" OnClick="btnGroupUpdate_Click">
                                        </f:Button>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns><f:RowNumberField></f:RowNumberField>
                                <f:BoundField DataField="GroupName" SortField="GroupName" HeaderText="分组名称" Width="120px"></f:BoundField>
                                <f:TemplateField ExpandUnusedSpace="true" ColumnID="Powers" HeaderText="权限列表">
                                    <ItemTemplate>
                                        <asp:CheckBoxList ID="ddlPowers" CssClass="powers" RepeatLayout="UnorderedList" RepeatDirection="Vertical" runat="server">
                                        </asp:CheckBoxList>
                                    </ItemTemplate>
                                </f:TemplateField>
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
    </form>
    <script>
        var grid2ID = '<%= Grid2.ClientID %>';
        var selectAllID = '<%= btnSelectAll.ClientID %>';
        var selectRowsID = '<%= btnSelectRows.ClientID %>';
        var unselectAllID = '<%= btnUnselectAll.ClientID %>';
        var unselectRowsID = '<%= btnUnselectRows.ClientID %>';

        function createQtip() {
            Ext.select('.powers li span').each(function (el) {
                var qtip = el.getAttribute('data-qtip');
                el.select('input,label').set({ 'ext:qtip': qtip });
            });
        }

        F.ready(function() {
            var grid = F(grid2ID);
            grid.addListener('viewready', function () {
                createQtip();
            });



            F(selectAllID).on('click', function () {
                Ext.select('.powers li span input').set({ checked: true }, false);
            });
            F(selectRowsID).on('click', function () {
                Ext.select('.x-grid-row-selected .powers li span input').set({ checked: true }, false);
            });

            F(unselectAllID).on('click', function () {
                Ext.select('.powers li span input').set({ checked: false }, false);
            });
            F(unselectRowsID).on('click', function () {
                Ext.select('.x-grid-row-selected .powers li span input').set({ checked: false }, false);
            });
        });

        F.ajaxReady(function() {
            createQtip();
        });
    </script>
</body>
</html>
