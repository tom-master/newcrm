using FineUI;
using NewCRM.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using NewCRM.Entity.Entity;
using NewCRM.IBLL;
using NewCRM.Datafactory;
namespace NewCRM.Web.AdminPage
{
    public partial class Logs :BasePage
    {
        private static readonly ILogBll Log = Createfactory.CreatefactoryBll.CreateLogBll();
        protected override string PagePowers
        {
            get { return Conststring.ViewPowerString.CoreLogView; }
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
            HelperCode.PowerHelper.CheckPowerWithButton(Conststring.CheckPowerWithControl.CoreLogDelete,btnDeleteSelected);
            Grid1.PageSize = HelperCode.ConfigHelper.PageSize;
            ddlGridPageSize.SelectedValue = HelperCode.ConfigHelper.PageSize.ToString(CultureInfo.InvariantCulture);
            ResolveDeleteButtonForGrid(btnDeleteSelected, Grid1);
            BindGrid();
        }

        private void BindGrid()
        {
            IQueryable<Log> log = Log.QueryLog();
            String searchCode = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchCode))
            {
                log = log.Where(d => d.Message.Contains(searchCode));
            }
            if (!ddlSearchLevel.SelectedValue.Equals("ALL"))
            {
                log = log.Where(d => d.Level == ddlSearchLevel.Items[ddlSearchLevel.SelectedIndex].Value);
            }
            if (ddlSearchRange.SelectedValue.Equals("ALL")) return;
            DateTime dTime = Conststring.OtherString.LocalTimeToDate;
            switch (ddlSearchRange.Items[ddlSearchRange.SelectedIndex].Value)
            {
                case "TODAY":
                    log = log.Where(d => d.LogTime >= dTime);
                    break;
                case "LAST3DAYS":
                    log = log.Where(d => d.LogTime >= dTime.AddDays(-3));
                    break;
                case "LAST7DAYS":
                    log = log.Where(d => d.LogTime >= dTime.AddDays(-7));
                    break;
                case "LASTMONTH":
                    log = log.Where(d => d.LogTime >= dTime.AddMonths(-1));
                    break;
                case "LASTYEAR":
                    log = log.Where(d => d.LogTime >= dTime.AddYears(-1));
                    break;
            }
            // 在查询添加之后，排序和分页之前获取总记录数
            Grid1.RecordCount = log.Count();
            //排序
            log = SortAndPage(log, Grid1);

            Grid1.DataSource = log;
            Grid1.DataBind(); 
        }
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;

            BindGrid();
        }
        protected void ttbSearchMessage_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearchMessage.Text = String.Empty;
            ttbSearchMessage.ShowTrigger1 = false;
            BindGrid();
        }
        protected void ttbSearchMessage_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearchMessage.ShowTrigger1 = true;
            BindGrid();
        }
        protected void ddlSearchLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void ddlSearchRange_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            //检查权限
            HelperCode.PowerHelper.CheckPowerWithLinkButtonField(Conststring.CheckPowerWithControl.CoreLogDelete,Grid1,"deleteField");
        }

        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();
        }

        protected void btnDeleteSelected_Click(object sender, EventArgs e)
        {
            if (!HelperCode.PowerHelper.CheckPower(Conststring.CheckPowerWithControl.CoreLogDelete))
            {
                HelperCode.PowerHelper.CheckPowerFailWithAlert();
                return;
            }
            List<Int32> ids = GetSelectedDataKeyIDs(Grid1);
            Log.DeleteLog(ids);//删除日志
            BindGrid();
        }

        protected void Grid1_RowCommand(object sender,GridCommandEventArgs e)
        {
            Int32 logId = GetSelectDataKeyId(Grid1);
            if (e.CommandName=="Delete")
            {
                if (!HelperCode.PowerHelper.CheckPower(Conststring.CheckPowerWithControl.CoreLogDelete))
                {
                    HelperCode.PowerHelper.CheckPowerFailWithAlert();
                    return;
                }
                Log.DeleteLog(logId);

                BindGrid();
            }
        }

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.Items[ddlGridPageSize.SelectedIndex].Value);
            BindGrid();
        }
    }
}