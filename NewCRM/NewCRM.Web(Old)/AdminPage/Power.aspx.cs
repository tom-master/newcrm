using FineUI;
using NewCRM.Common;
using System;
using System.Linq;
using System.Globalization;
using NewCRM.IBLL.AdminPageIBLL;
using NewCRM.Datafactory;
using Powers = NewCRM.Entity.Entity.Power;
namespace NewCRM.Web.AdminPage
{
    public partial class Power :BasePage
    {
        private static readonly IPowerBll Powers = CreatefactoryBll.CreatePowerBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CorePowerView; }
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
            PowerHelper.CheckPowerWithButton(ConstString.CheckPowerWithControl.CorePowerNew, btnNew);

            btnNew.OnClientClick = Window1.GetShowReference("~/AdminPage/PowerNew.aspx", "新增权限");
            Grid1.PageSize = ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = ConfigHelper.PageSize.ToString(CultureInfo.InvariantCulture);
            BindGrid();
        }

        private void BindGrid()
        {
            IQueryable<Powers> query = Powers.QueryPower();
            String searchText = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                query = query.Where(d => d.Name.Contains(searchText) || d.Title.Contains(searchText));
            }
            Grid1.RecordCount = query.Count();
            query = SortAndPage(query, Grid1);
            Grid1.DataSource = query;
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
            PowerHelper.CheckPowerWithWindowField(ConstString.CheckPowerWithControl.CorePowerEdit, Grid1,
                "editField");
            PowerHelper.CheckPowerWithLinkButtonField(ConstString.CheckPowerWithControl.CoresPowerDelete,
                Grid1, "deleteField");
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

        protected void Grid1_RowCommand(object sender,GridCommandEventArgs e)
        {
            Int32 powerId = GetSelectDataKeyId(Grid1);
            if (e.CommandName.Equals("Delete"))
            {
                if (!PowerHelper.CheckPower(ConstString.CheckPowerWithControl.CorePowerNew))
                {
                    PowerHelper.CheckPowerFailWithAlert();
                    return;
                }
                Int32 roleCount = Powers.PowerCount(powerId);
                if (roleCount>0)
                {
                    Alert.Show("删除失败！需要先清空使用此权限的角色！");
                    return;
                }
                Powers.DeletePower(powerId);
                BindGrid();
            }
        }
        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.Items[ddlGridPageSize.SelectedIndex].Value);

            BindGrid();
        }
    }
}