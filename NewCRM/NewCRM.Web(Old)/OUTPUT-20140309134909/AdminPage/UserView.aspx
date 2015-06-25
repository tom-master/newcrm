<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserView.aspx.cs" Inherits="NewCRM.Web.AdminPage.UserView" %>

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
                <f:Form ID="Form2" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px" Title="SimpleForm" LabelAlign="Left">
                    <Rows>
                        <f:FormRow ID="FormRow1" runat="server">
                            <Items>
                                <f:Label ID="labName" runat="server" Label="用户名">
                                </f:Label>
                                <f:Label ID="labRealName" runat="server" Label="中文名">
                                </f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow2" runat="server">
                            <Items>
                                <f:Label ID="labGender" runat="server" Label="性别">
                                </f:Label>
                                <f:Label ID="labEnabled" runat="server" Label="是否启用">
                                </f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow3" runat="server">
                            <Items>
                                <f:Label ID="labEmail" runat="server" Label="个人邮箱">
                                </f:Label>
                                <f:Label ID="labCompanyEmail" runat="server" Label="公司邮箱">
                                </f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow4" runat="server">
                            <Items>
                                <f:Label ID="labOfficePhone" runat="server" Label="工作电话">
                                </f:Label>
                                <f:Label ID="labOfficePhoneExt" runat="server" Label="分机号">
                                </f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow5" runat="server">
                            <Items>
                                <f:Label ID="labHomePhone" runat="server" Label="家庭电话">
                                </f:Label>
                                <f:Label ID="labCellPhone" runat="server" Label="手机号">
                                </f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow6" runat="server">
                            <Items>
                                <f:Label ID="labRole" runat="server" Label="所属角色">
                                </f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow7" runat="server">
                            <Items>
                                <f:Label ID="labDept" runat="server" Label="所属部门">
                                </f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow8" runat="server">
                            <Items>
                                <f:Label ID="labTitle" runat="server" Label="拥有职称">
                                </f:Label>
                            </Items>
                        </f:FormRow>
                        <f:FormRow ID="FormRow9" runat="server">
                            <Items>
                                <f:Label ID="labRemark" runat="server" Label="备注">
                                </f:Label>
                            </Items>
                        </f:FormRow>
                    </Rows>
                </f:Form>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
