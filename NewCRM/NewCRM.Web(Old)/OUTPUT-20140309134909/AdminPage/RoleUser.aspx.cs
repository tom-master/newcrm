using FineUI;
using NewCRM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.IBLL;
using NewCRM.Datafactory;
using Roles = NewCRM.Entity.Entity.Role;
using Users = NewCRM.Entity.Entity.User;
using System.Globalization;
namespace NewCRM.Web.AdminPage
{
    public partial class RoleUser : BasePage
    {
        private static readonly IRoleUserBll RoleUsers = Createfactory.CreatefactoryBll.CreateRoleUserBll();
        protected override string PagePowers
        {
            get { return Conststring.ViewPowerString.CoreRoleUserView; }
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
            HelperCode.PowerHelper.CheckPowerWithButton(Conststring.CheckPowerWithControl.CoreRoleUserNew, btnNew);
            HelperCode.PowerHelper.CheckPowerWithButton(Conststring.CheckPowerWithControl.CoreRoleUserDelete,
                btnDeleteSelected);
            ResolveDeleteButtonForGrid(btnDeleteSelected, Grid2);
            BindGrid();
            Grid1.SelectedRowIndex = 0;
            Grid2.PageSize = HelperCode.ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = HelperCode.ConfigHelper.PageSize.ToString(CultureInfo.InvariantCulture);
            BindGrid2();
        }

        private void BindGrid2()
        {
            Int32 roleId = GetSelectDataKeyId(Grid1);
            if (roleId == -1)
            {
                Grid2.RecordCount = 0;
                Grid2.DataSource = null;
                Grid2.DataBind();
            }
            else
            {
                IQueryable<Users> user = RoleUsers.FindUser();
                String searchText = ttbSearchUser.Text.Trim();
                if (!String.IsNullOrEmpty(searchText))
                {
                    user = user.Where(d => d.Name.Contains(searchText));
                }
                user = user.Where(d => !d.Name.Equals("admin"));
                user = user.Where(d => d.Roles.Any(w => w.ID.Equals(roleId)));
                Grid2.RecordCount = user.Count();
                user = SortAndPage(user, Grid2);

                Grid2.DataSource = user;
                Grid2.DataBind();
            }
        }

        private void BindGrid()
        {
            IQueryable<Roles> q = RoleUsers.FindRoles();
            q = Sort(q, Grid1);
            Grid1.DataSource = q;
            Grid1.DataBind();
        }
        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid2.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);

            BindGrid2();
        }
        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();

            // 默认选中第一个角色
            Grid1.SelectedRowIndex = 0;

            BindGrid2();
        }

        protected void Grid1_RowClick(object sender, GridRowClickEventArgs e)
        {
            BindGrid2();
        }
        protected void ttbSearchUser_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearchUser.ShowTrigger1 = true;
            BindGrid2();
        }
        protected void ttbSearchUser_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearchUser.Text = String.Empty;
            ttbSearchUser.ShowTrigger1 = false;
            BindGrid2();
        }

        protected void Grid2_PreDataBound(object sender, EventArgs e)
        {
            HelperCode.PowerHelper.CheckPowerWithLinkButtonField(Conststring.CheckPowerWithControl.CoreRoleUserDelete,
                Grid2, "deleteField");
        }
        protected void Grid2_Sort(object sender, GridSortEventArgs e)
        {
            Grid2.SortDirection = e.SortDirection;
            Grid2.SortField = e.SortField;
            BindGrid2();
        }

        protected void Grid2_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid2.PageIndex = e.NewPageIndex;
            BindGrid2();
        }

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            if (!HelperCode.PowerHelper.CheckPower(Conststring.CheckPowerWithControl.CoreRoleUserDelete))
            {
                HelperCode.PowerHelper.CheckPowerFailWithAlert();
                return;
            }
            Int32 id = GetSelectDataKeyId(Grid1);
            List<Int32> userIdList = GetSelectedDataKeyIDs(Grid2);
            Roles r = RoleUsers.IncludeRoleWithRoleId(id);
            foreach (var i in userIdList.Select(userId => r.Users.FirstOrDefault(d => d.ID.Equals(id))).Where(u=>u!=null))
            {
                r.Users.Remove(i);
            }
            RoleUsers.SaveChange();
            Grid2.SelectedRowIndexArray = null;
            BindGrid2();
        }

        protected void Grid2_RowCommand(object sender, GridCommandEventArgs e)
        {
            Object[] values = Grid2.DataKeys[e.RowIndex];
            Int32 userId = Convert.ToInt32(values[0], CultureInfo.InvariantCulture);
            if (e.CommandName.Equals("Delete"))
            {
                if (!HelperCode.PowerHelper.CheckPower(Conststring.CheckPowerWithControl.CoreRoleUserDelete))
                {
                    HelperCode.PowerHelper.CheckPowerFailWithAlert();
                    return;
                }
                Int32 roleId = GetSelectDataKeyId(Grid1);
                Roles roles = RoleUsers.IncludeRoleWithRoleId(roleId);

                Users users = roles.Users.FirstOrDefault(d => d.ID.Equals(userId));
                if (users != null)
                {
                    roles.Users.Remove(users);
                    RoleUsers.SaveChange();
                }
                BindGrid2();
            }
        }
        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid2();
        }

        protected void btnNew_Click(object sender, EventArgs e)
        {
            Int32 roleId = GetSelectDataKeyId(Grid1);
            String addUrl = String.Format("~/AdminPage/RoleUser.aspx?{0}", roleId);
            PageContext.RegisterStartupScript(Window1.GetShowReference(addUrl, "添加用户到当前角色"));
        }
    }
}