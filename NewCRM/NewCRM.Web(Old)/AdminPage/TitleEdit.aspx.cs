using NewCRM.Common;
using System;
using NewCRM.IBLL.AdminPageIBLL;
using NewCRM.Datafactory;
using FineUI;
using Titles = NewCRM.Entity.Entity.Title;
namespace NewCRM.Web.AdminPage
{
    public partial class TitleEdit : BasePage
    {
        private static readonly ITitleEditBll TitleEdits = CreatefactoryBll.CreateTitleEditBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreTitleEdit; }
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
            Titles currentTitle = TitleEdits.FindTitleWithId(id);
            if (currentTitle==null)
            {
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            tbxName.Text = currentTitle.Name;
            tbxRemark.Text = currentTitle.Remark;
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            Int32 id = GetQueryIntValue("id");
            Titles title = TitleEdits.FindTitleWithId(id);
            title.Name = tbxName.Text.Trim();
            title.Remark = tbxRemark.Text.Trim();
            TitleEdits.SaveChange();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
    }
}