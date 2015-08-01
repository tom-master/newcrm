using NewCRM.Common;
using System;
using NewCRM.IBLL.AdminPageIBLL;
using NewCRM.Datafactory;
using FineUI;
using Titles = NewCRM.Entity.Entity.Title;
namespace NewCRM.Web.AdminPage
{
    public partial class TitleNew :BasePage
    {
        private static readonly ITitleNewBll TitleNews = CreatefactoryBll.CreateTitleNewBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreTitleNew; }
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

        private void SaveTitle()
        {
            Titles title = new Titles
            {
              Name = tbxName.Text.Trim(),
              Remark = tbxRemark.Text.Trim()
            };
            TitleNews.SaveChange(title);
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SaveTitle();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
    }
}