using FineUI;
using NewCRM.Common;
using System;
using System.Linq;
using Roles =  NewCRM.Entity.Entity.Role;
using NewCRM.IBLL.AdminPageIBLL;
using NewCRM.Datafactory;
namespace NewCRM.Web.AdminPage
{
    public partial class Role : BasePage
    {
        private static readonly IRoleBll Roles = CreatefactoryBll.CreateRoleBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreRoleView; }
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
            PowerHelper.CheckPowerWithButton(ConstString.CheckPowerWithControl.CoreRoleNew, btnNew);
            btnNew.OnClientClick = Window1.GetShowReference("~/AdminPage/RoleNew.aspx","新增角色");
            Grid1.PageSize = ConfigHelper.PageSize;
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
            PowerHelper.CheckPowerWithWindowField(ConstString.CheckPowerWithControl.CoreRoleEdit, Grid1,
                "editField");
            PowerHelper.CheckPowerWithLinkButtonField(ConstString.CheckPowerWithControl.CoreRoleDelete, Grid1,
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
                if (PowerHelper.CheckPower(ConstString.CheckPowerWithControl.CoreRoleDelete))
                {
                    PowerHelper.CheckPowerFailWithAlert();
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