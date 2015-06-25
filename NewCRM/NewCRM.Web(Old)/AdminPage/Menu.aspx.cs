using System;
using System.Collections.Generic;
using FineUI;
using NewCRM.Common;
using Menus = NewCRM.Entity.Entity.App;
using NewCRM.IBLL.AdminPageIBLL;
using NewCRM.Datafactory;
namespace NewCRM.Web.AdminPage
{
    public partial class Menu :BasePage
    {

        private static readonly IMenuBll M = CreatefactoryBll.CreateMenuBll();
        protected override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreMenuView; }
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
            PowerHelper.CheckPowerWithButton(ConstString.CheckPowerWithControl.CoreMenuNew, btnNew);
            btnNew.OnClientClick = Window1.GetShowReference("~/AdminPage/MenuNew.aspx","新增菜单");
            BindGrid();
        }

          
        private void BindGrid()
        {
            List<Menus> menus = MenuHelper.Menus as List<Menus>;
            Grid1.DataSource = menus;
            Grid1.DataBind();
        }

        protected String GetModuleName(Object moduleNameObj)
        {
            String moduleName = moduleNameObj.ToString();
            return moduleName == "None" ? String.Empty : moduleName;
        }

        protected void Grid1_PreDataBound(object sender, EventArgs e)
        {
            PowerHelper.CheckPowerWithWindowField(ConstString.CheckPowerWithControl.CoreMenuEdit, Grid1,
                "editField");
            PowerHelper.CheckPowerWithLinkButtonField(ConstString.CheckPowerWithControl.CoreMenuDelete, Grid1,
                "deleteField");
        }

        protected void Grid1_RowCommand(object sender, GridCommandEventArgs e)
        {
            Int32 menuId = GetSelectDataKeyId(Grid1);
            if (e.CommandName=="Delete")
            {
                if (!PowerHelper.CheckPower(ConstString.CheckPowerWithControl.CoreMenuDelete))
                {
                    PowerHelper.CheckPowerFailWithAlert();
                    return;
                }
                Int32 childCount = M.CountMenu(menuId);
                if (childCount>0)
                {
                    Alert.ShowInTop("删除失败！请先删除子菜单！");
                    return;
                }
                M.DeleteMenu(menuId);
                MenuHelper.Reload();
                BindGrid();
            }
        }

        protected void Window1_Close(object sender, EventArgs e)
        {
            MenuHelper.Reload();
            BindGrid();
        }

    }
}