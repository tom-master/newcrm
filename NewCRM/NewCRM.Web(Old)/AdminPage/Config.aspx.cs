using System;
using System.Globalization;
using NewCRM.Common;
using Newtonsoft.Json.Linq;
namespace NewCRM.Web.AdminPage
{
    public partial class Config :BasePage
    {
        protected sealed override string PagePowers
        {
            get { return ConstString.ViewPowerString.CoreConfigView; }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckThisPagePower();
            }
        }

        private void CheckThisPagePower()
        {
           PowerHelper.CheckPowerWithButton(ConstString.CheckPowerWithControl.CoreConfigEdit, btnSave);
            tbxTitle.Text = ConfigHelper.Title;
            nbxPageSize.Text = ConfigHelper.PageSize.ToString(CultureInfo.InvariantCulture);
            tbxHelpList.Text = StringHelper.GetJsBeautifyString(ConfigHelper.HelpList);
            ddlMenuType.SelectedValue = ConfigHelper.MenuType;
            ddlTheme.SelectedValue = ConfigHelper.Theme;
        }
        protected void btnSave_OnClick(object obj, EventArgs e)
        {
            if (!PowerHelper.CheckPower(ConstString.CheckPowerWithControl.CoreConfigEdit))
            {
                PowerHelper.CheckPowerFailWithAlert();
                return;
            }
            string helperList = tbxHelpList.Text.Trim();
            try
            {
                JArray.Parse(helperList);
            }
            catch (Exception)
            {
                tbxHelpList.MarkInvalid("字符串格式不正确");
                return;
            }

            ConfigHelper.Title = tbxTitle.Text.Trim();
            ConfigHelper.PageSize = Convert.ToInt32(nbxPageSize.Text.Trim());
            ConfigHelper.HelpList = helperList;
            ConfigHelper.MenuType = ddlMenuType.SelectedValue;
            ConfigHelper.Theme = ddlTheme.SelectedValue;
            ConfigHelper.SaveAll();
        }
    }
}