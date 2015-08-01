<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ProFile.aspx.cs" Inherits="NewCRM.Web.AdminPage.ProFile" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <f:PageManager ID="PageManager1" AutoSizePanelID="Panel1" runat="server"></f:PageManager>
    <f:Panel ID="Panel1" ShowHeader="false" ShowBorder="false" BodyPadding="5px" runat="server" AutoScroll="true">
        <Items>
            <f:SimpleForm ID="SimpleForm1" runat="server" LabelWidth="100px" BodyPadding="5px" LabelAlign="Top" ShowBorder="false" ShowHeader="false" Width="400px">
                <Items>
                    <f:TextBox ID="tbxOldPassword" TextMode="Password" runat="server" Label="当前密码" Required="true" ShowRedStar="true">
                    </f:TextBox>
                    <f:TextBox ID="tbxNewPassword" TextMode="Password" runat="server" Label="新密码" Required="true" MinLength="6" ShowRedStar="true">
                    </f:TextBox>
                    <f:TextBox ID="tbxConfirmNewPassword" TextMode="Password" runat="server" Label="确认新密码" Required="true" ShowRedStar="true">
                    </f:TextBox>
                    <f:Button ID="btnSave" runat="server" Icon="SystemSave" OnClick="btnSave_OnClick" ValidateForms="SimpleForm1" ValidateTarget="Top" Text="修改密码">
                    </f:Button>
                </Items>
            </f:SimpleForm>
        </Items>
    </f:Panel>
    </form>
</body>
</html>
