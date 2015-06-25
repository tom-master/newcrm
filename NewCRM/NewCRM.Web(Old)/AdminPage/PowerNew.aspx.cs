using FineUI;
using NewCRM.Common;
using System;
using Powers = NewCRM.Entity.Entity.Power;
using NewCRM.Datafactory;
using NewCRM.IBLL.AdminPageIBLL;
namespace NewCRM.Web.AdminPage
{
    public partial class PowerNew : BasePage
    {
        private static readonly IPowerNewBll PowerNews = CreatefactoryBll.CreatePowerNewBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CorePowerNew; }
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

        private void SaveItems()
        {
            Powers item = new Powers
            {
                Name = tbxName.Text.Trim(),
                GroupName = tbxGroupName.Text.Trim(),
                Title = tbxTitle.Text.Trim(),
                Remark = tbxRemark.Text.Trim()
            };
            PowerNews.SaveItmes(item);
        }

        protected void btnSaveClose_Click(object sender,EventArgs e)
        {
            SaveItems();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
    }
}