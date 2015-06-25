using FineUI;
using NewCRM.Common;
using System;
using NewCRM.IBLL.AdminPageIBLL;
using NewCRM.Datafactory;
using Roles = NewCRM.Entity.Entity.Role;
namespace NewCRM.Web.AdminPage
{
    public partial class RoleEdit : BasePage
    {
        private static readonly IRoleEditBll RoleEdits = CreatefactoryBll.CreateRoleEditBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreRoleEdit; }
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
            Roles current = RoleEdits.FindRoleInId(id);
            if (current==null)
            {
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            tbxName.Text = current.Name;
            tbxRemark.Text = current.Remark;
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            Int32 id = GetQueryIntValue("id");
            Roles roleItem = RoleEdits.FindRoleInId(id);
            roleItem.Name = tbxName.Text.Trim();
            roleItem.Remark = tbxRemark.Text.Trim();
            RoleEdits.SaveRoleChange();

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
    }
}