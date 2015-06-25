using FineUI;
using NewCRM.Common;
using System;
using Users = NewCRM.Entity.Entity.User;
using NewCRM.Datafactory;
using NewCRM.IBLL.AdminPageIBLL;
namespace NewCRM.Web.AdminPage
{
    public partial class UserChangePassword : BasePage
    {
        private static readonly IUserChangePasswordBll UserChange =
            CreatefactoryBll.CreateUserChangePasswordBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreUserChangePassword; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHideReference();
            Int32 id = GetQueryIntValue("id");
            Users user = UserChange.FindUserWithId(id);
            if (user==null)
            {
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            if (user.Name.Equals("admin")&&!GetIndentityName.Equals("admin"))
            {
                Alert.Show("你无权编辑超级管理员！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            labUserName.Text = user.Name;
            labUserRealName.Text = user.ChineseName;
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            Int32 id = GetQueryIntValue("id");
            Users items = UserChange.FindUserWithId(id);
            items.Password = PasswordUtil.CreateDbPassword(tbxPassword.Text.Trim());
            UserChange.SaveChange();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
    }
}