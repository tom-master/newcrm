using FineUI;
using NewCRM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Roles =  NewCRM.Entity.Entity.Role;
using NewCRM.IBLL;
using NewCRM.Datafactory;
namespace NewCRM.Web.AdminPage
{
    public partial class Role : BasePage
    {
        private static readonly IRoleBll Roles = Createfactory.CreatefactoryBll.CreateRoleBll();
        protected override string PagePowers
        {
            get { return Conststring.ViewPowerString.CoreRoleView; }
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
            HelperCode.PowerHelper.CheckPowerWithButton(Conststring.CheckPowerWithControl.CoreRoleNew, btnNew);
            btnNew.OnClientClick = Window1.GetShowReference("~/AdminPage/RoleNew.aspx","新增角色");
            Grid1.PageSize = HelperCode.ConfigHelper.PageSize;
            BindGrid();
        }

        private void BindGrid()
        {
            IQueryable<Roles> role = Roles.FindRole();
            String searchText = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                role = role.Where(d => d.Name.Contains(searchText));
            }
            Grid1.RecordCount = role.Count();
            role = SortAndPage(role, Grid1);
            Grid1.DataSource = role;
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

        protected void Grid1_PreDataBound(object sender,EventArgs e)
        {
            HelperCode.PowerHelper.CheckPowerWithWindowField(Conststring.CheckPowerWithControl.CoreRoleEdit, Grid1,
                "editField");
            HelperCode.PowerHelper.CheckPowerWithLinkButtonField(Conststring.CheckPowerWithControl.CoreRoleDelete, Grid1,
                "deleteField");
        }
        protected void Grid1_Sort(object sender,GridSortEventArgs e)
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
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            Int32 id = GetSelectDataKeyId(Grid1);
            if (e.CommandName.Equals("Delete"))
            {
                if (HelperCode.PowerHelper.CheckPower(Conststring.CheckPowerWithControl.CoreRoleDelete))
                {
                    HelperCode.PowerHelper.CheckPowerFailWithAlert();
                    return;
                }
                Int32 userCountUnderThisRole = Roles.FindUserInThisRole(id);
                if (userCountUnderThisRole>0)
                {
                    Alert.ShowInTop("删除失败！需要先清空属于此角色的用户！");
                    return;
                }
                Roles.DeleteRoleWithId(id);
                BindGrid();
            }
        }

        protected void Window1_Close(object sender,EventArgs e)
        {

        }
    }
}