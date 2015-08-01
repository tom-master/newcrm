using NewCRM.Common;
using System;
using System.Linq;
using Titles = NewCRM.Entity.Entity.Title;
using NewCRM.Datafactory;
using NewCRM.IBLL;
using FineUI;
namespace NewCRM.Web.AdminPage
{
    public partial class Title : BasePage
    {
        private static readonly ITitleBll Titles = Createfactory.CreatefactoryBll.CreateTitleBll();
        protected override string PagePowers
        {
            get { return Conststring.ViewPowerString.CoreTitleView; }
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
            HelperCode.PowerHelper.CheckPowerWithButton(Conststring.CheckPowerWithControl.CoreTitleNew, btnNew);
            btnNew.OnClientClick = Window1.GetShowReference("~/AdminPage/TitleNew.aspx", "新增职务");
            Grid1.PageSize = HelperCode.ConfigHelper.PageSize;
            BindGrid();
        }

        private void BindGrid()
        {
            IQueryable<Titles> title = Titles.FindTitle();
            String searchText = ttbSearchMessage.Text.Trim();
            if (!String.IsNullOrEmpty(searchText))
            {
                title = title.Where(d => d.Name.Contains(searchText));
            }
            Grid1.RecordCount = title.Count();
            title = SortAndPage(title, Grid1);
            Grid1.DataSource = title;
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
            HelperCode.PowerHelper.CheckPowerWithWindowField(Conststring.CheckPowerWithControl.CoreTitleEdit, Grid1,
                "editField");
            HelperCode.PowerHelper.CheckPowerWithLinkButtonField(Conststring.CheckPowerWithControl.CoreTitleDelete,
                Grid1, "deleteField");
        }
        protected void Grid1_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            Int32 titleId = GetSelectDataKeyId(Grid1);
            if (e.CommandName.Equals("Delete"))
            {
                if (!HelperCode.PowerHelper.CheckPower(Conststring.CheckPowerWithControl.CoreTitleDelete))
                {
                    HelperCode.PowerHelper.CheckPowerFailWithAlert();
                    return;
                }
                Int32 userCount = Titles.FindUserWithTitleId(titleId);
                if (userCount>0)
                {
                    Alert.ShowInTop("删除失败！需要先清空拥有此职务的用户！");
                    return;
                }
                Titles.DeleteTitleWithTitleId(titleId);
                BindGrid();
            }
        }
        protected void Grid1_Sort(object sender, GridSortEventArgs e)
        {
            Grid1.SortDirection = e.SortDirection;
            Grid1.SortField = e.SortField;
            BindGrid();
        }
        protected void Window1_Close(object sender, EventArgs e)
        {
            BindGrid();
        }
    }
}