using FineUI;
using NewCRM.Common;
using System;
using System.Linq;
using NewCRM.Datafactory;
using NewCRM.IBLL.AdminPageIBLL;
using Roles = NewCRM.Entity.Entity.Role;
namespace NewCRM.Web.AdminPage
{
    public partial class UserSelectRole :BasePage
    {
        private static readonly IUserSelectRoleBll UserSelectRoles =
            CreatefactoryBll.CreateUserSelectRoleBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreRoleView; }
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
            String id = GetQueryStrValue("ids");
            BindRole();

            cblRole.SelectedValueArray = id.Split(',');
        }

        private void BindRole()
        {
            cblRole.DataTextField = "Name";
            cblRole.DataValueField = "ID";
            cblRole.DataSource = UserSelectRoles.FindRole();
            cblRole.DataBind();
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            String roleValue = String.Join(",", cblRole.SelectedItemArray.Select(d => d.Value));
            String roleText = String.Join(",",cblRole.SelectedItemArray.Select(d=>d.Text));

            PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(roleValue, roleText)+ActiveWindow.GetHideReference());
        }
    }
}