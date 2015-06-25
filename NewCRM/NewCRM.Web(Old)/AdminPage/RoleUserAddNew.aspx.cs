using FineUI;
using NewCRM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using Roles = NewCRM.Entity.Entity.Role;
using Users = NewCRM.Entity.Entity.User;
using NewCRM.Datafactory;
using NewCRM.IBLL.AdminPageIBLL;
using System.Globalization;
namespace NewCRM.Web.AdminPage
{
    public partial class RoleUserAddNew :BasePage
    {
        private static readonly IRoleUserAddNewBll RoleUserAddNews =
            CreatefactoryBll.CreateRoleUserAddNewBll();

        private static readonly IAttachBll Attach = CreatefactoryBll.CreateAttachBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreRoleUserNew; }
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
            Roles role = RoleUserAddNews.FindRoleWithId(id);
            if (role==null)
            {
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString(CultureInfo.InvariantCulture);
            BindGrid();

        }

        private void BindGrid()
        {
            IQueryable<Users> q = RoleUserAddNews.FindUser();
            String searchText = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                q = q.Where(d => d.Name.Contains(searchText));
            }
            q = q.Where(d => !d.Name.Equals("Admin"));
            Int32 id = GetQueryIntValue("id");
            q = q.Where(d => d.Roles.All(r => !r.Id.Equals(id)));
            Grid1.DataSource = q;
            Grid1.DataBind();

            UpdateSelectedIdRowIndexArray(hfSelectedIDS, Grid1);
        }

        protected void btnSaveClose_Click(object sender,EventArgs e)
        {
            SyncSelectedRowIndexArrayToHiddenField(hfSelectedIDS, Grid1);
            Int32 roleId = GetQueryIntValue("id");
            List<Int32> ids = GetSelectedIdsFormHiddent(hfSelectedIDS);
            Roles role = RoleUserAddNews.IncludeRoleWithId(roleId);
            foreach (var items in ids.Select(d=>Attach.Attach<Users>(d)))
            {
                role.Users.Add(items);
            }
            RoleUserAddNews.SaveChange();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected void ttbSearchMessage_Trigger2Click(object sender,EventArgs e)
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