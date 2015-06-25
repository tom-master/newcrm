using NewCRM.Common;
using System;
using System.Linq;
using Users = NewCRM.Entity.Entity.User;
using NewCRM.IBLL.AdminPageIBLL;
using NewCRM.Datafactory;
using FineUI;
namespace NewCRM.Web.AdminPage
{
    
    public partial class UserView : BasePage
    {
        private static readonly IUserViewBll UserViews = CreatefactoryBll.CreateUserViewBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreUserView; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }
        private void LoadData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHideReference();
            Int32 id = GetQueryIntValue("id");
            Users current = UserViews.IncludeUserWithId(id);
            if (current==null)
            {
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            labName.Text = current.Name;
            labRealName.Text = current.ChineseName;
            labCompanyEmail.Text = current.CompanyEmail;
            labEmail.Text = current.Email;
            labCellPhone.Text = current.CellPhone;
            labOfficePhone.Text = current.OfficePhone;
            labOfficePhoneExt.Text = current.OfficePhoneExt;
            labHomePhone.Text = current.HomePhone;
            labRemark.Text = current.Remark;
            labEnabled.Text = current.Enabled ? "启用" : "禁用";
            labGender.Text = current.Gender;

            labRole.Text = String.Join(",",current.Roles.Select(d=>d.Name).ToArray());
            labTitle.Text = String.Join(",", current.Titles.Select(d => d.Name).ToArray());

            if (current.Dept!=null)
            {
                labDept.Text = current.Dept.Name;
            }
        }
    }
}