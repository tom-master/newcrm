using System.Globalization;
using NewCRM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using FineUI;
using NewCRM.Entity.Entity;
using NewCRM.IBLL;
using NewCRM.Datafactory;
using Users = NewCRM.Entity.Entity.User;
namespace NewCRM.Web.AdminPage
{
    public partial class DeptUser :BasePage
    {

        private static readonly IDeptUserBll Dept = Createfactory.CreatefactoryBll.CreateDeptUserBll();
        protected override string PagePowers
        {
            get { return Conststring.ViewPowerString.CoreDeptUserView; }
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
            HelperCode.PowerHelper.CheckPowerWithButton(Conststring.CheckPowerWithControl.CoreDeptUserNew,btnNew);
            HelperCode.PowerHelper.CheckPowerWithButton(Conststring.CheckPowerWithControl.CoreDeptUserDelete, btnDeleteSelected);
            ResolveDeleteButtonForGrid(btnDeleteSelected, Grid2, "确定要从当前部门下面删除用户吗？");

            BindGrid1();
            Grid1.SelectedRowIndex = 0;
            Grid2.PageSize = HelperCode.ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = HelperCode.ConfigHelper.PageSize.ToString(CultureInfo.InvariantCulture);
            BindGrid2();
        }

        private void BindGrid2()
        {
            Int32 id = GetSelectDataKeyId(Grid1); 
            if (id==-1)
            {
                Grid2.RecordCount = 0;
                Grid2.DataSource = null;
                Grid2.DataBind();
            }
            else
            {
                //分页方法
                IQueryable<Users> user = Dept.Paging(ttbSearchUser.Text.Trim(), id);
                Grid2.RecordCount = user.Count();
                user = SortAndPage(user, Grid2);
                Grid2.DataSource = user;
                Grid2.DataBind();
            }
        }

        private void BindGrid1()
        {
            List<Depts> depts = HelperCode.DeptHelper.Depts;
            Grid1.DataSource = depts;
            Grid1.DataBind();
        }

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e) 
        {
            Grid2.PageSize = Convert.ToInt32(ddlGridPageSize.Items[ddlGridPageSize.SelectedIndex].Value);
            BindGrid2();
        }
        protected void Grid1_RowClick(object sender,GridRowClickEventArgs e) 
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
            // 数据绑定之前，进行权限检查
            HelperCode.PowerHelper.CheckPowerWithLinkButtonField(Conststring.CheckPowerWithControl.CoreDeptUserDelete, Grid2, "deleteField");
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
            if (HelperCode.PowerHelper.CheckPower(Conststring.CheckPowerWithControl.CoreDeptUserDelete))
            {
                HelperCode.PowerHelper.CheckPowerFailWithAlert();
                return;
            }
            List<Int32> userId = GetSelectedDataKeyIDs(Grid2);
            Dept.SaveChange(userId);//保存操作

            Grid2.SelectedRowIndexArray = null;
            BindGrid2();
        }

        protected void Grid2_RowCommand(object sender, GridCommandEventArgs e) 
        {
            object[] values = Grid2.DataKeys[e.RowIndex];
            Int32 userId = Convert.ToInt32(values[0]);
            if (e.CommandName=="Delete")
            {
                if (!HelperCode.PowerHelper.CheckPower(Conststring.CheckPowerWithControl.CoreDeptUserDelete))
                {
                    HelperCode.PowerHelper.CheckPowerFailWithAlert();
                    return;
                }
                Dept.SaveChange(userId);
                BindGrid2();
            }
        }
        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid2();
        }
        protected void btnNew_Click(object sender, EventArgs e)
        {
            int deptId= GetSelectDataKeyId(Grid1);
            string addUrl = String.Format("~/AdminPage/DeptUserAddNew.aspx?id={0}", deptId);

            PageContext.RegisterStartupScript(Window1.GetShowReference(addUrl, "添加用户到当前部门"));
        }
    }
}