using FineUI;
using NewCRM.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using Users = NewCRM.Entity.Entity.User;
using Depts = NewCRM.Entity.Entity.Depts;
using Roles = NewCRM.Entity.Entity.Role;
using Titles = NewCRM.Entity.Entity.Title;
using NewCRM.IBLL.AdminPageIBLL;
using NewCRM.Datafactory;
namespace NewCRM.Web.AdminPage
{
    public partial class UserNew : BasePage
    {
        private static readonly IAttachBll Attach = CreatefactoryBll.CreateAttachBll();
        private static readonly IUserNewBll UserNews = CreatefactoryBll.CreateUserNewBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreUserNew; }
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
            // 初始化用户所属角色
            InitUserRole();

            // 初始化用户所属部门
            InitUserDept();

            // 初始化用户所属职称
            InitUserTitle();
        }

        private void InitUserTitle()
        {
            String selectTitleUrl = String.Format("./UserSelectTitle.aspx?id=<script>{0}</script>",
                hfSelectedTitle.GetValueReference());
            tbSelectedTitle.OnClientTriggerClick = Window1.GetSaveStateReference(hfSelectedTitle.ClientID,
                tbSelectedTitle.ClientID) + Window1.GetShowReference(selectTitleUrl, "选择用户拥有的职称");
        }

        private void InitUserDept()
        {
            String selectDeptUrl = String.Format("./UserSelectDept.aspx?id=<script>{0}</script>",
                hfSelectedDept.GetValueReference());
            tbSelectedDept.OnClientTriggerClick = Window1.GetSaveStateReference(hfSelectedDept.ClientID, tbSelectedDept.ClientID,
                ClientID) + Window1.GetShowReference(selectDeptUrl, "选择用户所属的部门");
        }

        private void InitUserRole()
        {
            String selectRoleUrl = String.Format("./UserSelectRole.aspx?id=<script>{0}</script>",
                hfSelectedRole.GetValueReference());
            tbSelectedRole.OnClientTriggerClick = Window1.GetSaveStateReference(hfSelectedRole.ClientID,
                tbSelectedRole.ClientID) + Window1.GetShowReference(selectRoleUrl, "选择用户所属的角色");
        }

        private void SaveItem()
        {
            Users users = new Users
            {
                Name = tbxName.Text.Trim(),
                Password = PasswordUtil.CreateDbPassword(tbxPassword.Text.Trim()),
                ChineseName = tbxRealName.Text.Trim(),
                Gender = ddlGender.Items[ddlGender.SelectedIndex].Value,
                CompanyEmail = tbxCompanyEmail.Text.Trim(),
                Email = tbxEmail.Text.Trim(),
                OfficePhone = tbxOfficePhone.Text.Trim(),
                OfficePhoneExt = tbxOfficePhoneExt.Text.Trim(),
                HomePhone = tbxHomePhone.Text.Trim(),
                CellPhone = tbxCellPhone.Text.Trim(),
                Remark = tbxRemark.Text.Trim(),
                Enabled = cbxEnabled.Checked,
                CreateTime = DateTime.Now.Date
            };
            if (!String.IsNullOrEmpty(hfSelectedDept.Text))
            {
                users.Dept = Attach.Attach<Depts>(Convert.ToInt32(hfSelectedDept.Text,CultureInfo.InvariantCulture));
            }
            if (!String.IsNullOrEmpty(hfSelectedRole.Text))
            {
                users.Roles = new List<Roles>();
                Int32[] roleIds = StringHelper.GetIntArrayFromString(hfSelectedRole.Text);
                AddEntities(users.Roles, roleIds, Attach.Attach<Roles>(users.Id));
            }
            if (!String.IsNullOrEmpty(hfSelectedTitle.Text))
            {
                users.Titles = new List<Titles>();
                Int32[] titleIds = StringHelper.GetIntArrayFromString(hfSelectedTitle.Text);
                AddEntities(users.Titles, titleIds, Attach.Attach<Titles>(users.Id));
            }
            UserNews.SaveChange(users);
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            String userName = tbxName.Text.Trim();
            Users users = UserNews.FindLikeUserName(userName);
            if (users!=null)
            {
                Alert.Show("用户"+userName+"已存在");
                return;
            }
            SaveItem();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
    }
}