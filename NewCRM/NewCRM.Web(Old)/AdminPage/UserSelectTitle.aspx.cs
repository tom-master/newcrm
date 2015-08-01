using FineUI;
using NewCRM.Common;
using System;
using NewCRM.Datafactory;
using NewCRM.IBLL.AdminPageIBLL;
using System.Linq;
namespace NewCRM.Web.AdminPage
{
    public partial class UserSelectTitle : BasePage
    {
        private static readonly IUserSelectTitleBll UserSelectTitles =
            CreatefactoryBll.CreateUserSelectTitleBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreTitleView; }
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
            String ids = GetQueryStrValue("ids");
            BindDdlTitle();
            cblJobTitle.SelectedValueArray = ids.Split(',');
        }

        private void BindDdlTitle()
        {
            cblJobTitle.DataTextField = "Name";
            cblJobTitle.DataValueField = "ID";
            cblJobTitle.DataSource = UserSelectTitles.FindTitle();
            cblJobTitle.DataBind();
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            String titleValue = String.Join(",", cblJobTitle.SelectedItemArray.Select(d => d.Value));
            String titleText = String.Join(",", cblJobTitle.SelectedItemArray.Select(d => d.Text));
            PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(titleValue, titleText)
            + ActiveWindow.GetHideReference());
        }
    }
}