using FineUI;
using NewCRM.Common;
using System;
using Roles = NewCRM.Entity.Entity.Role;
using NewCRM.IBLL.AdminPageIBLL;
using NewCRM.Datafactory;
namespace NewCRM.Web.AdminPage
{
    public partial class RoleNew :BasePage
    {
        private static readonly IRoleNewBll RoleNews = CreatefactoryBll.CreateRoleNewBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreRoleNew; }
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
        }

        private void SaveItem()
        {
            Roles role = new Roles
            {
                Name = tbxName.Text.Trim(),
                Remark = tbxRemark.Text.Trim()
            };
            RoleNews.SaveChange(role);
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
    }
}