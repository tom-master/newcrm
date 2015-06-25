using System;
using System.Collections.Generic;
using FineUI;
using NewCRM.Common;
using NewCRM.Datafactory;
using NewCRM.IBLL.AdminPageIBLL;
using Menus = NewCRM.Entity.Entity.App;
using System.Globalization;
namespace NewCRM.Web.AdminPage
{
    public partial class MenuEdit : BasePage
    {
        private static readonly IAttachBll Attach = CreatefactoryBll.CreateAttachBll();
        private static readonly IMenuEditBll MenuEdits = CreatefactoryBll.CreateMenuEditBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreMenuEdit; }
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
            Menus menu = MenuEdits.FindMenu(id);
            if (menu == null)
            {
                Alert.Show("参数错误", String.Empty, ActiveWindow.GetHideReference());
                return;
            }
            tbxName.Text = menu.Name;
            tbxUrl.Text = menu.NavigateUrl;
            tbxSortIndex.Text = menu.SortIndex.ToString(CultureInfo.InvariantCulture);
            tbxIcon.Text = menu.ImageUrl;
            tbxRemark.Text = menu.Remark;
            if (menu.ViewPower != null)
            {
                tbxViewPower.Text = menu.ViewPower.Name;
            }
            BindDll(menu);

            InitIconList(iconList);

            if (String.IsNullOrEmpty(menu.ImageUrl))
            {
                iconList.SelectedValue = menu.ImageUrl;
            }
        }

        private void InitIconList(RadioButtonList iconList)
        {
            String[] icons = { "tag_yellow", "tag_red", "tag_purple", "tag_pink", "tag_orange", "tag_green", "tag_blue" };

            foreach (string icon in icons)
            {
                String value = String.Format("~/icon/{0}.png", icon);
                String text = String.Format("<img style=\"vertical-align:bottom;\" src=\"{0}\" />&nbsp;{1}", ResolveUrl(value), icon);
                iconList.Items.Add(new RadioItem(text, value));
            }
        }

        private void BindDll(Menus menu)
        {
            List<Menus> menus = DropDownListHelper.ResolveDropDownList.ResloveDdl(MenuHelper.Menus, menu.Id);
            // 绑定到下拉列表（启用模拟树功能和不可选择项功能）
            ddlParent.EnableSimulateTree = true;
            ddlParent.DataTextField = "Name";
            ddlParent.DataValueField = "ID";
            ddlParent.DataSimulateTreeLevelField = "TreeLevel";
            ddlParent.DataEnableSelectField = "Enabled";
            ddlParent.DataSource = menus;
            ddlParent.DataBind();
            if (menu.Parent != null)
            {
                ddlParent.SelectedValue = menu.Parent.Id.ToString(CultureInfo.InvariantCulture);
            }
        }

        protected void btnSaveClose_Click(object sender, EventArgs e)
        {
            Int32 id = GetQueryIntValue("id");
            Menus menus = MenuEdits.FindMenu(id);
            menus.Name = tbxName.Text.Trim();
            menus.NavigateUrl = tbxUrl.Text.Trim();
            menus.SortIndex = Convert.ToInt32(ddlParent.SelectedValue, CultureInfo.InvariantCulture);
            menus.ImageUrl = tbxIcon.Text.Trim();
            menus.Remark = tbxRemark.Text.Trim();
            Int32 parentId = Convert.ToInt32(ddlParent.Items[ddlParent.SelectedIndex].Value,
                CultureInfo.InvariantCulture);
            if (parentId==-1)
            {
                menus.Parent = null;
            }
            else
            {
                menus.Parent = Attach.Attach<Menus>(parentId);
            }
            String viewPowerName = tbxViewPower.Text.Trim();
            if (String.IsNullOrEmpty(viewPowerName))
            {
                menus.ViewPower = null;
            }
            else
            {
                menus.ViewPower = MenuEdits.SelectPowerByViewPowerName(viewPowerName);
            }
            MenuEdits.SaveChange();

            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
    }
}