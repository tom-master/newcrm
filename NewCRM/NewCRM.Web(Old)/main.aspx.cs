using System;
using System.Web.UI;
using NewCRM.Datafactory;
using NewCRM.IBLL.AdminPageIBLL;
using Menu = NewCRM.Entity.Entity.App;
using AjaxPro;
namespace NewCRM.Web
{
    public partial class Main : Page
    {
        private static readonly ILoginBll UserBll = CreatefactoryBll.CreateLoginBll();

        protected void Page_Load(object sender, EventArgs e)
        {
            Utility.RegisterTypeForAjax(typeof (Main));
        }
        [AjaxMethod]
        public String LoadSkin()
        {
            return UserBll.LoadSkin(User.Identity.Name).Skin;
        }

        #region old Code
        //protected void btnExit_Click(object sender,EventArgs e)
        //{
        //    FormsAuthentication.SignOut();
        //    SessionProvide.CurrectAbandon();
        //    FormsAuthentication.RedirectToLoginPage();
        //}

        ///// <summary>
        ///// 初始化工具栏上的菜单
        ///// </summary>
        ///// <param name="btn"></param>
        ///// <param name="reg"></param>
        ///// <param name="colParam"></param>
        //private static void PageInit(Button btn, Region reg, params ControlBase[] colParam)
        //{
        //    JArray jarry = JArray.Parse(ConfigHelper.HelpList);
        //    foreach (MenuButton menuItem in from JObject objItem in jarry select new MenuButton
        //    {
        //        EnablePostBack = true,
        //        Text = objItem.Value<String>("Text"),
        //        Icon = IconHelper.String2Icon(objItem.Value<String>("Icon"), true),
        //        OnClientClick =
        //            String.Format("addExampleTab('{0}','{1}',{2})", objItem.Value<String>("ID"),
        //                new Control().ResolveUrl(objItem.Value<String>("URL")), objItem.Value<String>("Text"))
        //    })
        //    {
        //        btn.Menu.Items.Add(menuItem);
        //    }

        //    IEnumerable<Menu> menus = MenuHelper.ResolveUserMenuList();
        //    IList<Menu> enumerable = menus as IList<Menu> ?? menus.ToList();
        //    if (enumerable.Count == 0)
        //    {
        //        System.Web.HttpContext.Current.Response.Write("系统管理员尚未给此用户配置菜单");
        //        System.Web.HttpContext.Current.Response.End();
        //    }
        //    else
        //    {
        //        //注册客户端脚本，服务器端控件ID和客户端ID的映射关系
        //        JObject jObj = GetClientIDs(colParam);
        //        jObj.Add("userName", BasePage.GetIndentityName);
        //        jObj.Add("userIP", System.Web.HttpContext.Current.Request.UserHostAddress);
        //        jObj.Add("onlineUserCount", UserBll.GetOnlineUser());
        //        if (ConfigHelper.MenuType == "accordion")
        //        {
        //            Accordion accMenu = MenuHelper.InitAccordionMenu(enumerable, reg);
        //            jObj.Add("treeMenu", accMenu.ClientID);
        //            jObj.Add("menuType", "accordion");
        //        }
        //        else
        //        {
        //            Tree tree = MenuHelper.InitTree(enumerable, reg);
        //            tree.EnableSingleClickExpand = true;
        //            jObj.Add("treeMenu", tree.ClientID);
        //            jObj.Add("menuType", "menu");
        //        }

        //        String jObjScript = String.Format("window.DATA={0};", jObj.ToString(Newtonsoft.Json.Formatting.None));
        //        PageContext.RegisterStartupScript(jObjScript);
        //    }
        //}
        //// 获取服务器端控件 ID对应的客户端控件ID
        //private static JObject GetClientIDs(params ControlBase[] col)
        //{
        //    JObject jo = new JObject();
        //    foreach (ControlBase baseItem in col)
        //    {
        //        // 服务器端控件ID    客户端控件ID
        //        jo.Add(baseItem.ID, baseItem.ClientID);
        //    }
        //    return jo;
        //}
        #endregion
    }
}