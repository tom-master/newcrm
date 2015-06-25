using FineUI;
using NewCRM.Common;
using System;
using Depts = NewCRM.Entity.Entity.Depts;
namespace NewCRM.Web.AdminPage
{
    public partial class UserSelectDept : BasePage
    {
        private static Int32 _deptId=0;
        private static Int32 _selectedRowIndex = -1;
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreDeptView; }
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
            btnClose.OnClientClick = ActiveWindow.GetHideReference();
            _deptId = GetQueryIntValue("ids");
            BindGrid();
            if (_selectedRowIndex!=-1)
            {
                Grid1.SelectedRowIndex = _selectedRowIndex;
            }
        }

        private void BindGrid()
        {
            Grid1.DataSource = DeptHelper.Depts;
            Grid1.DataBind();
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            Int32 selectedRowIndexs = Grid1.SelectedRowIndex;
            String deptIds = Grid1.DataKeys[selectedRowIndexs][0].ToString();
            String deptNames = Grid1.DataKeys[selectedRowIndexs][1].ToString();

            PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(deptIds, deptNames) + ActiveWindow.GetHideReference());
        }

        protected void Grid1_RowDataBound(object sender, GridRowEventArgs e)
        {
            Depts dept = e.DataItem as Depts;
            if (dept != null && _deptId.Equals(dept.Id))
            {
                _selectedRowIndex = e.RowIndex;
            }
        }
    }
}