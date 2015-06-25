using System.Globalization;
using NewCRM.Common;
using System;
using System.Collections.Generic;
using FineUI;
using NewCRM.Entity;
using NewCRM.Entity.Entity;
using NewCRM.IBLL.AdminPageIBLL;
using NewCRM.Datafactory;
namespace NewCRM.Web.AdminPage
{
    public partial class DeptEdit : BasePage
    {
        private static readonly IDeptEditBll Dept = CreatefactoryBll.CreateDeptEditBll();
        private static readonly IAttachBll Attach = CreatefactoryBll.CreateAttachBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreDeptEdit; }
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
            Int32 id = GetQueryIntValue("id");
            Depts currentDept = Dept.SelectDeptByValue(id);
            tbxName.Text = currentDept.Name;
            tbxRemark.Text = currentDept.Remark;
            tbxSortIndex.Text = currentDept.SortIndex.ToString(CultureInfo.InvariantCulture);
            BindDdl(currentDept);
        }

        private void BindDdl(IKeyId currentDept)
        {
            List<Depts> deptList = DropDownListHelper.ResolveDropDownList.ResloveDdl(DeptHelper.Depts, currentDept.Id);
            ddlParent.EnableSimulateTree = true;
            ddlParent.DataTextField = "Name";
            ddlParent.DataValueField = "ID";
            ddlParent.DataSimulateTreeLevelField = "TreeLevel";
            ddlParent.DataEnableSelectField = "Enabled";
            ddlParent.DataSource = deptList;
            ddlParent.DataBind();
            ddlParent.SelectedValue = currentDept.Id.ToString(CultureInfo.InvariantCulture);
        }

        protected void btnSaveClose_Click(object sender,EventArgs e) 
        {
            Int32 id = GetQueryIntValue("id");
            Depts depts = Dept.SelectDeptByValue(id);
            depts.Name = tbxName.Text.Trim();
            depts.SortIndex = Convert.ToInt32(tbxSortIndex.Text.Trim());
            depts.Remark = tbxRemark.Text.Trim();
            Int32 parentId = Convert.ToInt32(ddlParent.Items[ddlParent.SelectedIndex].Value);
            depts.Parent = parentId==-1 ? null : Attach.Attach<Depts>(parentId);
            Dept.SaveChange(depts);//保存

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
    }
}