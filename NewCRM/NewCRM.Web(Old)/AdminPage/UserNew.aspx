<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserNew.aspx.cs" Inherits="NewCRM.Web.AdminPage.UserNew" %>

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
                        <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                        </f:ToolbarSeparator>
                        <f:Button ID="btnSaveClose" ValidateForms="SimpleForm1" Icon="SystemSaveClose" OnClick="btnSaveClose_Click" runat="server" Text="保存后关闭">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Form ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px" Title="SimpleForm">
                    <Rows>
                        <f:FormRow runat="server">
                            <Items>
                                <f:TextBox ID="tbxName" runat="server" Label="用户名" Required="true" ShowRedStar="true">
                                </f:TextBox>
                                <f:TextBox ID="tbxRealName" runat="server" Label="中文名" Required="true" ShowRedStar="true">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:RadioButtonList ID="ddlGender" Label="性别" Required="true" ShowRedStar="true" runat="server">
                                    <f:RadioItem Text="男" Value="男"></f:RadioItem>
                                    <f:RadioItem Text="女" Value="女"></f:RadioItem>
                                </f:RadioButtonList>
                                <f:CheckBox ID="cbxEnabled" runat="server" Label="是否启用">
                                </f:CheckBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow5" runat="server">
                            <Items>
                                <f:TextBox ID="tbxPassword" runat="server" TextMode="Password" Label="登录密码" Required="true" ShowRedStar="true">
                                </f:TextBox>
                                <f:Label runat="server"></f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:TextBox ID="tbxEmail" runat="server" Label="邮箱" Required="true" ShowRedStar="true" RegexPattern="EMAIL">
                                </f:TextBox>
                                <f:TextBox ID="tbxCompanyEmail" runat="server" Label="公司邮箱" RegexPattern="EMAIL">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:TextBox ID="tbxOfficePhone" runat="server" Label="工作电话">
                                </f:TextBox>
                                <f:TextBox ID="tbxOfficePhoneExt" runat="server" Label="分机号">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow4" runat="server">
                            <Items>
                                <f:TextBox ID="tbxHomePhone" runat="server" Label="家庭电话">
                                </f:TextBox>
                                <f:TextBox ID="tbxCellPhone" runat="server" Label="手机号">
                                </f:TextBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:TriggerBox ID="tbSelectedRole" EnableEdit="false" EnablePostBack="false" TriggerIcon="Search" Label="所属角色" runat="server">
                                </f:TriggerBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow2" runat="server">
                            <Items>
                                <f:TriggerBox ID="tbSelectedDept" EnableEdit="false" EnablePostBack="false" TriggerIcon="Search" Label="所属部门" runat="server">
                                </f:TriggerBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow3" runat="server">
                            <Items>
                                <f:TriggerBox ID="tbSelectedTitle" EnableEdit="false" EnablePostBack="false" TriggerIcon="Search" Label="拥有职称" runat="server">
                                </f:TriggerBox>
                            </Items>
                        </f:FormRow>
                        <f:FormRow runat="server">
                            <Items>
                                <f:TextArea ID="tbxRemark" runat="server" Label="备注">
                                </f:TextArea>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
        <f:HiddenField ID="hfSelectedRole" runat="server">
        </f:HiddenField>
        <f:HiddenField ID="hfSelectedDept" runat="server">
        </f:HiddenField>
        <f:HiddenField ID="hfSelectedTitle" runat="server">
        </f:HiddenField>
        <f:Window ID="Window1" Title="编辑" EnableIFrame="true" runat="server" EnableMaximize="true" EnableResize="true" Target="Top" IsModal="True" Width="550px" Height="350px" Hidden="true">
        </f:Window>
    </form>
</body>
</html>
