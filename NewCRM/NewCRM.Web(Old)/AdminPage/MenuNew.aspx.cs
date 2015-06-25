using FineUI;
using NewCRM.Common;
using System;
using System.Collections.Generic;
using Menus = NewCRM.Entity.Entity.App;
using NewCRM.Datafactory;
using NewCRM.IBLL.AdminPageIBLL;
namespace NewCRM.Web.AdminPage
{
    public partial class MenuNew :BasePage
    {
        private static readonly IMenuNewBll MenuNews = CreatefactoryBll.CreateMenuNewBll();
        private static readonly IAttachBll Attach = CreatefactoryBll.CreateAttachBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreMenuNew; }
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
            BindDdl(iconList);
            InitIconList(iconList);
        }

        private void InitIconList(FineUI.RadioButtonList iconList)
        {
            String[] icons = { "tag_yellow", "tag_red", "tag_purple", "tag_pink", "tag_orange", "tag_green", "tag_blue" };
            foreach (String icon in icons)
            {
                String value = String.Format("~/icon/{0}.png", icon);
                String text = String.Format("<img style=\"vertical-align:bottom;\" src=\"{0}\" />&nbsp;{1}", ResolveUrl(value), icon);
                iconList.Items.Add(new RadioItem(text, value));
            }
        }

        private void BindDdl(FineUI.RadioButtonList iconList)
        {
            List<Menus> menus = DropDownListHelper.ResolveDropDownList.ResloveDdl(MenuHelper.Menus);
            ddlParent.EnableSimulateTree = true;
            ddlParent.DataTextField = "Name";
            ddlParent.DataValueField = "ID";
            ddlParent.DataSimulateTreeLevelField = "TreeLevel";
            ddlParent.DataSource = menus;
            ddlParent.DataBind();
            ddlParent.SelectedValue = "0";
        }

        private void SaveItems()
        {
            Menus items = new Menus
            {
                Name=tbxName.Text.Trim(),
                NavigateUrl = tbxUrl.Text.Trim(),
                SortIndex = Convert.ToInt32(tbxSortIndex.Text.Trim()),
                Remark = tbxRemark.Text.Trim()
            };
            Int32 parentId = Convert.ToInt32(ddlParent.Items[ddlParent.SelectedIndex].Value);
            if (parentId==-1)
            {
                items.Parent = null;    
            }
            else
            {
                items.Parent = Attach.Attach<Menus>(parentId);
            }
            String viewPowerName = tbxViewPower.Text.Trim();
            if (String.IsNullOrEmpty(viewPowerName))
            {
                items.ViewPower = null;
            }
            else
            {
                items.ViewPower = MenuNews.SelectViewPowerName(viewPowerName);
            }
            MenuNews.SaveChange(items);
        }

        protected void btnSaveClose_Click(object sender,EventArgs e)
        {
            SaveItems();
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
    }
}