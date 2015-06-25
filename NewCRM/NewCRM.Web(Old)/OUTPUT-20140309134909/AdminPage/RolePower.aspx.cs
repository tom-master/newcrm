using FineUI;
using NewCRM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.IBLL;
using NewCRM.Datafactory;
using System.Globalization;
using Roles = NewCRM.Entity.Entity.Role;
using Powers = NewCRM.Entity.Entity.Power;
using ListItems = System.Web.UI.WebControls.ListItem;
using CheckBox = System.Web.UI.WebControls.CheckBoxList;
namespace NewCRM.Web.AdminPage
{
    public partial class RolePower : BasePage
    {
        private static readonly IRolePowerBll RolePowers = Createfactory.CreatefactoryBll.CreateRolePowerBll();
        private static readonly Dictionary<String, Boolean> CurrentRolePowers = new Dictionary<String, Boolean>();
        private static readonly IAttachBll Attach = Createfactory.CreatefactoryBll.CreateAttachBll();
        protected override string PagePowers
        {
            get { return Conststring.ViewPowerString.CoreRolePowerView; }
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
            Grid1.PageSize = HelperCode.ConfigHelper.PageSize;
            BindGrid();
            Grid1.SelectedRowIndex = 0;
            Grid2.PageSize = HelperCode.ConfigHelper.PageSize;
            BindGrid2();
        }
        private void BindGrid()
        {
            IQueryable<Roles> role = RolePowers.FindRole();
            role = Sort(role, Grid1);
            Grid1.DataSource = role;
            Grid1.DataBind();
        }
        private void BindGrid2()
        {
            Int32 roleId = GetSelectDataKeyId(Grid1);
            if (roleId == -1)
            {
                Grid2.DataSource = null;
                Grid2.DataBind();
            }
            else
            {
                CurrentRolePowers.Clear();
                Roles role = RolePowers.IncludeRoleWithId(roleId);
                foreach (var items in role.Powers.Select(item => item.Name).Where(d => !CurrentRolePowers.ContainsKey(d)))
                {
                    CurrentRolePowers.Add(items, true);
                }
                IQueryable<IGrouping<String, Powers>> group = RolePowers.GroupByGroupName();
                if (Grid2.SortField.Equals("GroupName"))
                {
                    @group = Grid2.SortDirection.Equals("ASC") ? @group.OrderBy(d => d.Key) : @group.OrderByDescending(d => d.Key);
                }
                var powers = group.Select(d => new
                {
                    GroupName=d.Key,
                    Powers = d
                });
                Grid2.DataSource = powers;
                Grid2.DataBind();
            }
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

        protected void Grid2_RowDataBound(object sender, GridRowEventArgs e)
        {
            CheckBox ddlPowers = (CheckBox)Grid2.Rows[e.RowIndex].FindControl("ddlPowers");
            IGrouping<String,Powers> powers = e.DataItem.GetType().GetProperty("Powers").GetValue(e.DataItem) as IGrouping<String, Powers>;
            if (powers == null) return;
            foreach (Powers power in powers.ToList())
            {
                ListItems items = new ListItems
                {
                    Value = power.ID.ToString(CultureInfo.InvariantCulture),
                    Text = power.Title
                };
                items.Attributes["data-qtip"] = power.Name;
                items.Selected = CurrentRolePowers.ContainsKey(power.Name);
                if (ddlPowers == null) return;
                ddlPowers.Items.Add(items);
            }
        }

        protected void Grid2_Sort(object sender, GridSortEventArgs e)
        {
            Grid2.SortDirection = e.SortDirection;
            Grid2.SortField = e.SortField;
            BindGrid2();
        }

        protected void btnGroupUpdate_Click(object sender, EventArgs e)
        {
            if (!HelperCode.PowerHelper.CheckPower(Conststring.CheckPowerWithControl.CoreRolePowerEdit))
            {
                HelperCode.PowerHelper.CheckPowerFailWithAlert();
                return;
            }
            Int32 roleId = GetSelectDataKeyId(Grid1);
            if (roleId == -1) return;
            List<Int32> newPowerIdList = (from rowItem in Grid2.Rows select rowItem.FindControl("ddlPowers") as CheckBox into ddlPowers from ListItems item in ddlPowers.Items where item.Selected select Convert.ToInt32(item.Value, CultureInfo.InvariantCulture)).ToList();
            Roles role = RolePowers.IncludeRoleWithId(roleId);
            ReplaceEntities(role.Powers, newPowerIdList.ToArray(), Attach.Attach<Powers>(roleId));

            Alert.ShowInTop("当前角色的权限更新成功！");
        }
    }
}