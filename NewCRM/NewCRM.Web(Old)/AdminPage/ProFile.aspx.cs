using NewCRM.Common;
using System;
using Users = NewCRM.Entity.Entity.User;
using NewCRM.Datafactory;
using NewCRM.IBLL.AdminPageIBLL;
using FineUI;
namespace NewCRM.Web.AdminPage
{
    public partial class ProFile : BasePage
    {
        private static readonly IProFileBll ProFiles = CreatefactoryBll.CreateProFileBll();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

            }
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            String oldPwd = tbxOldPassword.Text.Trim();
            String newPwd = tbxNewPassword.Text.Trim();
            String confirmNewPwd = tbxConfirmNewPassword.Text.Trim();
            if (newPwd != confirmNewPwd)
            {
                tbxConfirmNewPassword.MarkInvalid("确认密码和新密码不一致!");
                return;
            }
            Users user = ProFiles.FindUser(User.Identity.Name);
            if (user != null)
            {
                if (!PasswordUtil.ComparePasswords(user.Password, oldPwd))
                {
                    tbxConfirmNewPassword.MarkInvalid("当前密码不正确!");
                    return;
                }
                user.Password = PasswordUtil.CreateDbPassword(newPwd);
            }
            ProFiles.SavePassword();
            Alert.ShowInTop("修改密码成功");
        }
    }
}