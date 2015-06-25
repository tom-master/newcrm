
namespace NewCRM.Infrastructure.CommonTools.CustomHelper
{
    
    //public sealed class PowerHelper
    //{
    //    /// <summary>
    //    /// 注入首页加载的对象
    //    /// </summary>
    //    private static readonly IMainPageDal MainBll;

    //    static PowerHelper()
    //    {
    //        MainBll = CreatefactoryDal.CreateDal<IMainPageDal>("MainPageDal");
    //    }

    //    private static IEnumerable<Power> _powers;
        
    //    #region
    //    private static IEnumerable<Power> Powers
    //    {
    //        get
    //        {
    //            if (_powers == null)
    //            {
    //                InitPowers();
    //            }
    //            return _powers;
    //        }
    //    }

    //    private static void InitPowers()
    //    {
    //        _powers = (List<Power>)MainBll.InitPowers();
    //    }

    //    private static void Reload()
    //    {
    //        _powers = null;
    //    }
    //    #endregion

    //    #region 获取脚色权限名
    //    public static List<string> GetRolePowerName()
    //    {
    //        if (new Page().User.Identity.Name.Equals("admin"))
    //        {
    //            return Powers.Select(power => power.Name).ToList();
    //        }
    //        if (UserPowerList == null)
    //        {
    //            List<Int32> roleIds = GetIdentityRoleIDs();
    //            List<string> rolePowerName = new List<string>();
    //            var v = (IQueryable<Role>)MainBll.GetRolePowerName(roleIds);
    //            foreach (var roleItem in v)
    //            {
    //                foreach (var powerItem in roleItem.Powers)
    //                {
    //                    if (!rolePowerName.Contains(powerItem.Name))
    //                    {
    //                        rolePowerName.Add(powerItem.Name);
    //                    }
    //                }
    //            }
    //            UserPowerList = rolePowerName;
    //        }
    //        return UserPowerList as List<string>;
    //    }
    //    public static object UserPowerList
    //    {
    //        get
    //        {
    //            object objValue = System.Web.HttpContext.Current.Session[ConstString.OtherString.UserPowerList];
    //            return objValue ?? string.Empty;
    //        }
    //        set { System.Web.HttpContext.Current.Session[ConstString.OtherString.UserPowerList] = value; }
    //    }
    //    #endregion

    //    #region 获取管理员标识
    //    private static List<Int32> GetIdentityRoleIDs()
    //    {
    //        List<Int32> roleIds = new List<Int32>();
    //        if (!new Page().User.Identity.IsAuthenticated) return roleIds;
    //        FormsIdentity formsIdentity = new Page().User.Identity as FormsIdentity;
    //        if (formsIdentity == null) return roleIds;
    //        FormsAuthenticationTicket ticket = formsIdentity.Ticket;
    //        string userData = ticket.UserData;
    //        if (userData != null)
    //            roleIds.AddRange(
    //                userData.Split(',')
    //                    .Where(roleItem => !string.IsNullOrEmpty(roleItem))
    //                    .Select(roleItem => Convert.ToInt32(roleItem)));
    //        return roleIds;
    //    }
    //    #endregion

    //    #region 页面和操作按钮权限检查
    //    public static void CheckPowerFailWithPage()
    //    {
    //        System.Web.HttpContext.Current.Response.Write(ConstString.OtherString.CheckPowerFaliPageMessage);
    //        System.Web.HttpContext.Current.Response.End();
    //    }
    //    private static void CheckPowerFailWithAction(Button btn)
    //    {
    //        btn.Enabled = false;
    //        btn.ToolTip = ConstString.OtherString.CheckPowerFaliActionMessage;
    //    }
    //    private static void CheckPowerFailWithAction(Grid grid, string columnId)
    //    {
    //        LinkButtonField linkBtn = grid.FindColumn(columnId) as LinkButtonField;
    //        if (linkBtn == null) return;
    //        linkBtn.Enabled = false;
    //        linkBtn.ToolTip = ConstString.OtherString.CheckPowerFaliActionMessage;
    //    }
    //    /// <summary>
    //    /// 此方法第三个参数没有任何意义 只是用于区别第二个方法的检查权限的不同
    //    /// </summary>
    //    /// <param name="grid"></param>
    //    /// <param name="columnId"></param>
    //    /// <param name="boo">无意义</param>
    //    private static void CheckPowerFailWithAction(Grid grid, string columnId, bool boo)
    //    {
    //        //此方法第三个参数没有任何意义 只是用于区别第二个方法的检查权限的不同
    //        WindowField winBtn = grid.FindColumn(columnId) as WindowField;
    //        if (winBtn == null) return;
    //        winBtn.Enabled = false;
    //        winBtn.ToolTip = ConstString.OtherString.CheckPowerFaliActionMessage;
    //    }
    //    #endregion

    //    #region 权限检查失败提示
    //    public static void CheckPowerFailWithAlert()
    //    {
    //        PageContext.RegisterStartupScript(Alert.GetShowInTopReference(ConstString.OtherString.CheckPowerFaliActionMessage));
    //    }
    //    #endregion

    //    #region 检查权限
    //    public static void CheckPowerWithButton(string powerName, Button btn)
    //    {
    //        if (!CheckPower(powerName))
    //        {
    //            CheckPowerFailWithAction(btn);
    //        }
    //    }
    //    public static void CheckPowerWithLinkButtonField(string powerName, Grid grid, string columnId)
    //    {
    //        if (!CheckPower(powerName))
    //        {
    //            CheckPowerFailWithAction(grid, columnId);
    //        }
    //    }

    //    public static void CheckPowerWithWindowField(string powerName, Grid grid, string columnId)
    //    {
    //        if (!CheckPower(powerName))
    //        {
    //            CheckPowerFailWithAction(grid, columnId, false);
    //        }
    //    }
    //    #endregion

    //    #region 检查当前的用户是否有某个操作的权限
    //    public static bool CheckPower(string powerName)
    //    {
    //        if (string.IsNullOrEmpty(powerName))
    //        {
    //            return true;
    //        }
    //        List<string> rolePowerName = GetRolePowerName();
    //        if (rolePowerName.Contains(powerName))
    //        {
    //            return true;
    //        }
    //        return false;
    //    }
    //    #endregion
    //}
}
