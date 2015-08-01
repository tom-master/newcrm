using System.Collections.Generic;
using System.Globalization;
using FineUI;
using NewCRM.Common;
using System;
using NewCRM.Entity.Entity;
using NewCRM.IBLL.AdminPageIBLL;
using NewCRM.Datafactory;
using System.Linq;
using Users = NewCRM.Entity.Entity.User;
namespace NewCRM.Web.AdminPage
{
    public  partial class DeptUserAddUser :BasePage
    {
        private static readonly IDeptUserAddUserBll Dept = CreatefactoryBll.CreateDeptUserAddBll();
        private static readonly IAttachBll Attach = CreatefactoryBll.CreateAttachBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreDeptUserNew; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadData();
            }
        }

        private  void LoadData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHideReference();
            Int32 id = GetQueryIntValue("id");
            Depts current = Dept.Find(id);
            if (current==null)
            {
                // 参数错误，首先弹出Alert对话框然后关闭弹出窗口
                Alert.Show("参数错误！", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString(CultureInfo.InvariantCulture);
            BindGrid();
        }

        private void BindGrid()
        {
            //在职务名中搜素
            IQueryable<Users> user = Dept.Paging(ttbSearchMessage.Text.Trim());
            Grid1.RecordCount = user.Count();
            user = SortAndPage(user, Grid1);
            Grid1.DataSource = user;
            Grid1.DataBind();

            UpdateSelectedIdRowIndexArray(hfSelectedIDS, Grid1);
        }


        protected void btnSaveClose_Click(object sender,EventArgs e)
        {
            SyncSelectedRowIndexArrayToHiddenField(hfSelectedIDS, Grid1);
            Int32 deptId = GetQueryIntValue("id");

            List<Int32> ids = GetSelectedIdsFormHiddent(hfSelectedIDS);
            Depts dept = Attach.Attach<Depts>(deptId);
            Dept.SaveChange(dept, ids);
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }

        protected void ttbSearchMessage_Trigger2Click(object sender,EventArgs  e)
        {
            SyncSelectedRowIndexArrayToHiddenField(hfSelectedIDS,Grid1);
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