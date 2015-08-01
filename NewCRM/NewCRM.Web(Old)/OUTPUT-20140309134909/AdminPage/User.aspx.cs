using NewCRM.Common;
using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NewCRM.Entity.Entity;
using Users = NewCRM.Entity.Entity.User;
using NewCRM.IBLL;
using NewCRM.Datafactory;
using FineUI;
namespace NewCRM.Web.AdminPage
{
    public partial class User : BasePage
    {
        private static readonly IUserBll Us = Createfactory.CreatefactoryBll.CreateUserBll();
        protected override string PagePowers
        {
            get { return Conststring.ViewPowerString.CoreUserView; }
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
            HelperCode.PowerHelper.CheckPowerWithButton(Conststring.CheckPowerWithControl.CoreUserEdit,
                btnChangeEnableUsers);
            HelperCode.PowerHelper.CheckPowerWithButton(Conststring.CheckPowerWithControl.CoreUserDelete,
                btnDeleteSelected);
            HelperCode.PowerHelper.CheckPowerWithButton(Conststring.CheckPowerWithControl.CoreUserNew, btnNew);
            ResolveDeleteButtonForGrid(btnDeleteSelected, Grid1);
            ResolveEnableStatusButtonForGrid(btnEnableUsers, Grid1, true);

            ResolveEnableStatusButtonForGrid(btnDisableUsers,Grid1,false);

            btnNew.OnClientClick = Window1.GetShowReference("~/AdminPage/UserNew.aspx","新增用户");
            Grid1.PageSize = HelperCode.ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = HelperCode.ConfigHelper.PageSize.ToString(CultureInfo.InvariantCulture);
            BindGrid();
        }

        private void BindGrid()
        {
            IQueryable<Users> users = Us.FindUser();
            String searchText = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                users = users.Where(d => d.Name.Contains(searchText));
            }
            if (!GetIndentityName.Equals("admin"))
            {
                users = users.Where(d => !d.Name.Equals("admin"));
            }
            if (!rblEnableStatus.Items[rblEnableStatus.SelectedIndex].Value.Equals("all"))
            {
                users =
                    users.Where(
                        d => d.Enabled.Equals(rblEnableStatus.Items[rblEnableStatus.SelectedIndex].Value.Equals("enabled") ? true : false));
            }
            Grid1.RecordCount = users.Count();
            users = SortAndPage(users, Grid1);
            Grid1.DataSource = users;
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

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            HelperCode.PowerHelper.CheckPowerWithWindowField(Conststring.CheckPowerWithControl.CoreUserEdit, Grid1,
                "editField");
            HelperCode.PowerHelper.CheckPowerWithLinkButtonField(Conststring.CheckPowerWithControl.CoreUserDelete, Grid1,
                "deleteField");
            HelperCode.PowerHelper.CheckPowerWithWindowField(Conststring.CheckPowerWithControl.CoreUserChangePassword,
                Grid1, "changePasswordField");
        }

        protected void Grid1_PreRowDataBound(object sender, FineUI.GridPreRowEventArgs e)
        {
            Users use = e.DataItem as Users;
            if (use != null && use.Name.Equals("admin"))
            {
                LinkButtonField linkField = Grid1.FindColumn("deleteField") as LinkButtonField;
                if (linkField == null) return;
                linkField.Enabled = false;
                linkField.ToolTip = "不能删除超级管理员!";
            }
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

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            if (!HelperCode.PowerHelper.CheckPower(Conststring.CheckPowerWithControl.CoreUserDelete))
            {
                HelperCode.PowerHelper.CheckPowerFailWithAlert();
                return;
            }
            List<Int32> ids = GetSelectedDataKeyIDs(Grid1);
            Us.DeleteUserWithId(ids);
            BindGrid();
        }
        protected void btnEnableUsers_Click(object sender, EventArgs e)
        {
            SetSelectedUsersEnableStatus(true);
        }
        protected void btnDisableUsers_Click(object sender, EventArgs e)
        {
            SetSelectedUsersEnableStatus(false);
        }

        private void SetSelectedUsersEnableStatus(bool p)
        {
            if (!HelperCode.PowerHelper.CheckPower(Conststring.CheckPowerWithControl.CoreUserEdit))
            {
                HelperCode.PowerHelper.CheckPowerFailWithAlert();
                return;
            }
            List<Int32> ids = GetSelectedDataKeyIDs(Grid1);
            Us.UpdateUserWithId(ids, p);
            BindGrid();
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            Int32 userId = GetSelectDataKeyId(Grid1);
            String userName = GetSelectedDataKey(Grid1, 1);
            if (e.CommandName.Equals("Delete"))
            {
                if (HelperCode.PowerHelper.CheckPower(Conststring.CheckPowerWithControl.CoreUserDelete))
                {
                    HelperCode.PowerHelper.CheckPowerFailWithAlert();
                    return;
                }
                if (userName.Equals("admin"))
                {
                    Alert.ShowInTop("不能删除默认的系统管理员（admin）！");
                }
                else
                {
                    Us.DeleteUserWithId(userId);
                    BindGrid();
                }
            }
        }
        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }
        protected void rblEnableStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }
        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);

            BindGrid();
        }
    }
}