using FineUI;
using System.Globalization;
using System;
using System.Linq;
using NewCRM.IBLL.AdminPageIBLL;
using NewCRM.Common;
using NewCRM.Datafactory;
using Users = NewCRM.Entity.Entity.User;
using Depts = NewCRM.Entity.Entity.Depts;
using Roles = NewCRM.Entity.Entity.Role;
using Titles = NewCRM.Entity.Entity.Title;

namespace NewCRM.Web.AdminPage
{
    public partial class UserEdit : BasePage
    {
        private static readonly IAttachBll Attach = CreatefactoryBll.CreateAttachBll();
        private static readonly IUserEditBll UserEdits = CreatefactoryBll.CreateUserEditBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreUserEdit; }
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
            Users current = UserEdits.IncludeUserWithId(id);
            if (current==null)
            {
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            if (current.Name.Equals("admin")&&!GetIndentityName.Equals("admin"))
            {
                Alert.Show("你无权编辑超级管理员！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            labName.Text = current.Name;
            tbxRealName.Text = current.ChineseName;
            tbxCompanyEmail.Text = current.CompanyEmail;
            tbxEmail.Text = current.Email;
            tbxCellPhone.Text = current.CellPhone;
            tbxOfficePhone.Text = current.OfficePhone;
            tbxOfficePhoneExt.Text = current.OfficePhoneExt;
            tbxHomePhone.Text = current.HomePhone;
            tbxRemark.Text = current.Remark;
            cbxEnabled.Checked = current.Enabled;
            ddlGender.SelectedValue = current.Gender;
            // 初始化用户所属角色
            InitUserRole(current);

            // 初始化用户所属部门
            InitUserDept(current);

            // 初始化用户所属职称
            InitUserTitle(current);
        }

        private void InitUserTitle(Users current)
        {
            tbSelectedTitle.Text = String.Join(",", current.Titles.Select(d => d.Name).ToArray());
            hfSelectedTitle.Text = String.Join(",", current.Titles.Select(d => d.Id).ToArray());
            String url = String.Format("./UserSelectTitle.aspx?ids=<script>{0}</script>",
                hfSelectedTitle.GetValueReference());
            tbSelectedTitle.OnClientTriggerClick = Window1.GetSaveStateReference(hfSelectedTitle.ClientID,
                tbSelectedTitle.ClientID) + Window1.GetShowReference(url, "选择用户所属的角色");
        }

        private void InitUserDept(Users current)
        {
            if (current.Dept!=null)
            {
                tbSelectedDept.Text = current.Dept.Name;
                hfSelectedRole.Text = current.Dept.Id.ToString(CultureInfo.InvariantCulture);
            }
            String url = String.Format("./UserSelectDept.aspx?ids=<script>{0}</script>",hfSelectedDept.GetValueReference());
            tbSelectedDept.OnClientTriggerClick = Window1.GetSaveStateReference(hfSelectedDept.ClientID,
                tbSelectedDept.ClientID) + Window1.GetShowReference(url, "选择用户所属的部门");
        }

        private void InitUserRole(Users current)
        {
            if (current == null) return;
            tbSelectedRole.Text = String.Join(",", current.Roles.Select(d => d.Name).ToArray());
            hfSelectedRole.Text = String.Join(",", current.Roles.Select(d => d.Id).ToArray());
            String url = String.Format("./UserSelectRole.aspx?ids=<script>{0}</script>", hfSelectedRole.GetValueReference());
            tbSelectedRole.OnClientTriggerClick = Window1.GetSaveStateReference(hfSelectedRole.ClientID,
                tbSelectedRole.ClientID) + Window1.GetShowReference(url, "选择用户所属的角色");
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            Int32 id = GetQueryIntValue("id");
            Users items = UserEdits.IncludeUserWithId(id);
            items.ChineseName = tbxRealName.Text.Trim();
            items.Gender = ddlGender.SelectedValue;
            items.CompanyEmail = tbxCompanyEmail.Text.Trim();
            items.Email = tbxEmail.Text.Trim();
            items.CellPhone = tbxCellPhone.Text.Trim();
            items.OfficePhone = tbxOfficePhone.Text.Trim();
            items.OfficePhoneExt = tbxOfficePhoneExt.Text.Trim();
            items.HomePhone = tbxHomePhone.Text.Trim();
            items.Remark = tbxRemark.Text.Trim();
            items.Enabled = cbxEnabled.Checked;
            if (String.IsNullOrEmpty(hfSelectedDept.Text))
            {
                items.Dept = null;
            }
            else
            {
                Int32 newDeptId = Convert.ToInt32(hfSelectedDept.Text, CultureInfo.InvariantCulture);
                items.Dept = Attach.Attach<Depts>(newDeptId);
            }
            Int32[] roleArr = StringHelper.GetIntArrayFromString(hfSelectedRole.Text);
            ReplaceEntities(items.Roles, roleArr, Attach.Attach<Roles>(id));
            Int32[] titleArr = StringHelper.GetIntArrayFromString(hfSelectedTitle.Text);
            ReplaceEntities(items.Titles, titleArr, Attach.Attach<Titles>(id));

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
    }
}