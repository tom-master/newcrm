using FineUI;
using NewCRM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Titles = NewCRM.Entity.Entity.Title;
using Users = NewCRM.Entity.Entity.User;
using NewCRM.Datafactory;
using NewCRM.IBLL;
using System.Globalization;
namespace NewCRM.Web.AdminPage
{
    public partial class TitleUserAddNew :BasePage
    {
        private static readonly ITitleUserAddNewBll TitleUserAddNews =
            Createfactory.CreatefactoryBll.CreateTitleUserAddNewBll();

        private static readonly IAttachBll Attach = Createfactory.CreatefactoryBll.CreateAttachBll();
        protected override string PagePowers
        {
            get { return Conststring.ViewPowerString.CoreTitleUserNew; }
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
            Titles title = TitleUserAddNews.FindTitleWithId(id);
            if (title==null)
            {
                Alert.Show("参数错误", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            Grid1.PageSize = HelperCode.ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = HelperCode.ConfigHelper.PageSize.ToString(CultureInfo.InvariantCulture);

            BindGrid();
        }

        private void BindGrid()
        {
            IQueryable<Users> q = TitleUserAddNews.FindUser();
            String searchText = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                q = q.Where(d => d.Name.Contains(searchText));
            }
            q = q.Where(d => !d.Name.Equals("Admin"));
            Int32 titleId = GetQueryIntValue("id");
            q = q.Where(d => d.Titles.All(t => !t.ID.Equals(titleId)));
            Grid1.RecordCount = q.Count();
            q = SortAndPage(q, Grid1);

            Grid1.DataSource = q;
            Grid1.DataBind();

            UpdateSelectedIdRowIndexArray(hfSelectedIDS, Grid1);
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            SyncSelectedRowIndexArrayToHiddenField(hfSelectedIDS, Grid1);
            Int32 titleId = GetQueryIntValue("id");
            List<Int32> titleList = GetSelectedIdsFormHiddent(hfSelectedIDS);
            Titles title = TitleUserAddNews.IncludeTitleWithTitleId(titleId);
            foreach (Users users in titleList.Select(userId=>Attach.Attach<Users>(userId)))
            {
                title.Users.Add(users);
            }
            TitleUserAddNews.SaveChange();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
        protected void ttbSearchMessage_Trigger2Click(object sender, EventArgs e)
        {
            SyncSelectedRowIndexArrayToHiddenField(hfSelectedIDS, Grid1);

            ttbSearchMessage.ShowTrigger1 = true;
            BindGrid();
        }

        protected void ttbSearchMessage_Trigger1Click(object sender, EventArgs e)
        {
            SyncSelectedRowIndexArrayToHiddenField(hfSelectedIDS, Grid1);

            ttbSearchMessage.Text = String.Empty;
            ttbSearchMessage.ShowTrigger1 = false;
            BindGrid();
        }

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            SyncSelectedRowIndexArrayToHiddenField(hfSelectedIDS, Grid1);

            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();
        }

        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            SyncSelectedRowIndexArrayToHiddenField(hfSelectedIDS, Grid1);

            Grid1.PageIndex = e.NewPageIndex;
            BindGrid();
        }


        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            SyncSelectedRowIndexArrayToHiddenField(hfSelectedIDS, Grid1);

            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);

            BindGrid();
        }
    }
}