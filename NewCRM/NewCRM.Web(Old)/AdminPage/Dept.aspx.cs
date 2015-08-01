using System;
using NewCRM.Common;
using FineUI;
using NewCRM.IBLL.AdminPageIBLL;
using NewCRM.Datafactory;
namespace NewCRM.Web.AdminPage
{
    public partial class Dept: BasePage
    {
        /// <summary>
        /// 注入部门
        /// </summary>
        private static readonly IDeptBll DeptBll = CreatefactoryBll.CreateDeptBll();
        protected sealed override string PagePowers
        {
            get
            {
                return ConstString.ViewPowerString.CoreDeptView;
            }
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
            PowerHelper.CheckPowerWithButton(ConstString.CheckPowerWithControl.CoreDeptNew, btnNew);
            btnNew.OnClientClick = Window1.GetShowReference("~/AdminPage/DeptNew.aspx","新增部门");
        }
        private void BindGrid() 
        {
            Grid1.DataSource = DeptHelper.Depts;
            Grid1.DataBind();
        }
        protected void Grid1_PreDataBound(object sender, EventArgs e) 
        {
            PowerHelper.CheckPowerWithWindowField(ConstString.CheckPowerWithControl.CoreDeptEdit, Grid1, "editField");
            PowerHelper.CheckPowerWithLinkButtonField(ConstString.CheckPowerWithControl.CoreDeptDelete, Grid1, "deleteField");
        }
        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e) 
        {
            Int32 deptId = GetSelectDataKeyId(Grid1);
            if (e.CommandName != "Delete") return;
            if (!PowerHelper.CheckPower(ConstString.CheckPowerWithControl.CoreDeptDelete))
            {
                //权限检查失败
                PowerHelper.CheckPowerFailWithAlert();
            }
            Int32 userCount = DeptBll.UserCount(deptId);
            if (userCount > 0)
            {
                Alert.ShowInTop("删除失败！需要先清空属于此部门的用户！");
                return;
            }

            Int32 childCount = DeptBll.ChildCount(deptId);
            if (childCount >= 0)
            {
                Alert.ShowInTop("删除失败！请先删除子部门！");
                return;
            }
            DeptBll.DeleteDept(deptId);//删除部门
        }
        protected void Window1_Close(object sender,EventArgs e) 
        {
            DeptHelper.Reload();
            BindGrid();
        }
    }
}