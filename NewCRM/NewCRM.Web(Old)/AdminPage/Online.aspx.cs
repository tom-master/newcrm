using FineUI;
using NewCRM.Common;
using System;
using System.Linq;
using  System.Globalization;
using NewCRM.Datafactory;
using NewCRM.IBLL.AdminPageIBLL;
using Onlines = NewCRM.Entity.Entity.Online;
namespace NewCRM.Web.AdminPage
{
    public partial class Online : BasePage
    {
        private static readonly IOnlineBll Onlines = CreatefactoryBll.CreateOnlineBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreOnlineView; }
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
            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString(CultureInfo.InvariantCulture);
            BindGrid();
        }

        private void BindGrid()
        {
            IQueryable<Onlines> q = Onlines.SelectOnline();
            String searchText = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                q = q.Where(d => d.User.Name.Contains(searchText));
            }
            DateTime dTime = DateTime.Now.AddHours(-2);
            q = q.Where(d => d.UpdateTime > dTime);
            Grid1.RecordCount = q.Count();
            q = SortAndPage(q, Grid1);
            Grid1.DataSource = q;
            Grid1.DataBind();
        }

        protected void ttbSearchMessage_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearchMessage.ShowTrigger1 = true;
            BindGrid();
        }

        protected void ttbSearchMessage_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearchMessage.Text = String.Empty;
            ttbSearchMessage.ShowTrigger1 = false;
            BindGrid();
        }

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;

            BindGrid();
        }
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;

            BindGrid();
        }

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);

            BindGrid();
        }
    }
}