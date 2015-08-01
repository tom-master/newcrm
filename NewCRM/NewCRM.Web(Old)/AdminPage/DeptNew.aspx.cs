using System;
using System.Collections.Generic;
using FineUI;
using NewCRM.Common;
using NewCRM.Entity.Entity;
using NewCRM.IBLL.AdminPageIBLL;
using NewCRM.Datafactory;
namespace NewCRM.Web.AdminPage
{
    public partial class DeptNew : BasePage
    {
        private static readonly IDeptNewBll Dept = CreatefactoryBll.CreateDeptNewBll();
        private static readonly IAttachBll Attach = CreatefactoryBll.CreateAttachBll();
        protected override string PagePowers
        {
            get
            {
                return ConstString.ViewPowerString.CoreDeptNew;
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
            btnClose.OnClientClick = ActiveWindow.GetHideReference();
            BindDdl();
        }

        private void BindDdl()
        {
            List<Depts> depts = DropDownListHelper.ResolveDropDownList.ResloveDdl(DeptHelper.Depts);
            ddlParent.EnableSimulateTree = true;
            ddlParent.DataTextField = "Name";
            ddlParent.DataValueField = "Value";
            ddlParent.DataSimulateTreeLevelField = "TreeLevel";
            ddlParent.DataSource = depts;
            ddlParent.DataBind();
            ddlParent.SelectedValue = "0";
        }

        private void Save() 
        {
            Depts dept = new Depts
            {
                Name = tbxName.Text.Trim(),
                SortIndex = Convert.ToInt32(tbxSortIndex.Text.Trim()),
                Remark = tbxRemark.Text.Trim()
            };
            Int32 id = Convert.ToInt32(ddlParent.Items[ddlParent.SelectedIndex].Value);
            dept.Parent = id == -1 ? null : Attach.Attach<Depts>(id);
            Dept.SaveChange(dept);
        }
        protected void btnSaveClose_Click(object sender, EventArgs e) 
        {
            Save();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
    }
}