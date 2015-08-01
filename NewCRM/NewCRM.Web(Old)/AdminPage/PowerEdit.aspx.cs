using FineUI;
using NewCRM.Common;
using System;
using Powers = NewCRM.Entity.Entity.Power;
using NewCRM.Datafactory;
using NewCRM.IBLL.AdminPageIBLL;
namespace NewCRM.Web.AdminPage
{
    public partial class PowerEdit :BasePage
    {
        private static readonly IPowerEditBll Power = CreatefactoryBll.CreatePowerEditBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CorePowerEdit; }
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

            Powers power = Power.FindPower(id);
            if (power==null)
            {
                Alert.Show("参数错误!", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            tbxName.Text = power.Name;
            tbxGroupName.Text = power.GroupName;
            tbxTitle.Text = power.Title;
            tbxRemark.Text = power.Remark;
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            Int32 id = GetQueryIntValue("id");
            Powers item = Power.FindPower(id);
            item.Name = tbxName.Text.Trim();
            item.GroupName = tbxGroupName.Text.Trim();
            item.Title = tbxTitle.Text.Trim();
            item.Remark = tbxRemark.Text.Trim();
            Power.SaveChange();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
    }
}