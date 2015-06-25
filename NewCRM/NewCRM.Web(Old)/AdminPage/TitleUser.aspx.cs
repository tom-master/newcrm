using FineUI;
using NewCRM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Datafactory;
using NewCRM.IBLL.AdminPageIBLL;
using Titles = NewCRM.Entity.Entity.Title;
using Users = NewCRM.Entity.Entity.User;
using System.Globalization;
namespace NewCRM.Web.AdminPage
{
    public partial class TitleUser :BasePage
    {
        private static readonly ITitleUserBll TitleUsers = CreatefactoryBll.CreateTitleUserBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreTitleUserView; }
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
            PowerHelper.CheckPowerWithButton(ConstString.CheckPowerWithControl.CoreTitleUserNew, btnNew);
            PowerHelper.CheckPowerWithButton(ConstString.CheckPowerWithControl.CoreTitleUserDelete,
                btnDeleteSelected);
            ResolveDeleteButtonForGrid(btnDeleteSelected, Grid2, "确定要从当前职称中移除选中的{0}项记录吗？");
            BindGrid1();
            Grid1.SelectedRowIndex = 0;
            Grid2.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString(CultureInfo.InvariantCulture);
            BindGrid2();
        }

        private void BindGrid2()
        {
            Int32 titleId = GetSelectDataKeyId(Grid1);
            if (titleId==-1)
            {
                Grid2.RecordCount = 0;
                Grid2.DataSource = 0;
                Grid2.DataBind();
            }
            else
            {
                IQueryable<Users> q = TitleUsers.FindUser();
                String searchText = ttbSearchUser.Text.Trim();
                if (!String.IsNullOrEmpty(searchText))
                {
                    q = q.Where(d => d.Name.Contains(searchText));
                }
                q = q.Where(d => !d.Name.Equals("Admin"));

                q = q.Where(d => d.Titles.Any(r => r.Id.Equals(titleId)));
                Grid2.RecordCount = q.Count();
                q = SortAndPage(q, Grid2);
                Grid2.DataSource = q;
                Grid2.DataBind();
            }
        }

        private void BindGrid1()
        {
            IQueryable<Titles> q = TitleUsers.FindTitle();
            q = Sort(q, Grid1);
            Grid1.DataSource = q;
            Grid1.DataBind();
        }
        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid2.PageSize = Convert.ToInt32(ddlGridPageSize.Items[ddlGridPageSize.SelectedIndex].Value);
            BindGrid2();
        }
        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid1();

            // 默认选中第一个职称
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
            PowerHelper.CheckPowerWithLinkButtonField(ConstString.CheckPowerWithControl.CoreTitleUserDelete,
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
            if (!PowerHelper.CheckPower(ConstString.CheckPowerWithControl.CoreTitleUserDelete))
            {
                PowerHelper.CheckPowerFailWithAlert();
                return;
            }
            Int32 titleId = GetSelectDataKeyId(Grid1);
            List<Int32> userIds = GetSelectedDataKeyIDs(Grid2);

            Titles title = TitleUsers.IncludeTitleWithTitleId(titleId);
            title.Users.Where(d => userIds.Contains(d.Id)).ToList().ForEach(d => title.Users.Remove(d));
            TitleUsers.SaveChange();
            Grid2.SelectedRowIndexArray = null;
            BindGrid2();
        }

        protected void Grid2_RowCommand(object sender, GridCommandEventArgs e)
        {
            Object[] values = Grid2.DataKeys[e.RowIndex];
            Int32 userId = Convert.ToInt32(values[0],CultureInfo.InvariantCulture);
            if (e.CommandName.Equals("Delete"))
            {
                if (PowerHelper.CheckPower(ConstString.CheckPowerWithControl.CoreTitleUserDelete))
                {
                    PowerHelper.CheckPowerFailWithAlert();
                    return;
                }
                Int32 titleId = GetSelectDataKeyId(Grid1);
                Titles title = TitleUsers.IncludeTitleWithTitleId(titleId);

                Users tobeRemovedUser = title.Users.FirstOrDefault(d => d.Id.Equals(userId));
                if (tobeRemovedUser!=null)
                {
                    title.Users.Remove(tobeRemovedUser);
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
            Int32 titleId = GetSelectDataKeyId(Grid1);
            String addUrl = String.Format("~/AdminPage/TitleUserAddNew?id={0}", titleId);
            PageContext.RegisterStartupScript(Window1.GetShowReference(addUrl, "添加用户到当前职称"));
        }
    }
}