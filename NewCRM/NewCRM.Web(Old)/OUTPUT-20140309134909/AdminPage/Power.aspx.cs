using FineUI;
using NewCRM.Common;
using System;
using System.Linq;
using System.Globalization;
using NewCRM.IBLL;
using NewCRM.Datafactory;
using Powers = NewCRM.Entity.Entity.Power;
namespace NewCRM.Web.AdminPage
{
    public partial class Power :BasePage
    {
        private static readonly IPowerBll Powers = Createfactory.CreatefactoryBll.CreatePowerBll();
        protected override string PagePowers
        {
            get { return Conststring.ViewPowerString.CorePowerView; }
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
            HelperCode.PowerHelper.CheckPowerWithButton(Conststring.CheckPowerWithControl.CorePowerNew, btnNew);

            btnNew.OnClientClick = Window1.GetShowReference("~/AdminPage/PowerNew.aspx", "新增权限");
            Grid1.PageSize = HelperCode.ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = HelperCode.ConfigHelper.PageSize.ToString(CultureInfo.InvariantCulture);
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
            HelperCode.PowerHelper.CheckPowerWithWindowField(Conststring.CheckPowerWithControl.CorePowerEdit, Grid1,
                "editField");
            HelperCode.PowerHelper.CheckPowerWithLinkButtonField(Conststring.CheckPowerWithControl.CoresPowerDelete,
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
                if (!HelperCode.PowerHelper.CheckPower(Conststring.CheckPowerWithControl.CorePowerNew))
                {
                    HelperCode.PowerHelper.CheckPowerFailWithAlert();
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