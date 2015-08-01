
namespace NewCRM.Infrastructure.CommonTools.CustomHelper
{

    //public sealed class ConfigHelper
    //{
    //    private static readonly IMainPageDal MainBll;

    //    static ConfigHelper()
    //    {
    //        MainBll = CreatefactoryDal.CreateDal<IMainPageDal>("MainPageDal");
    //    }

    //    #region fields & constructor

    //    private static List<Configs> _configs;

    //    private static readonly List<string> ChangedKeys = new List<string>();

    //    public static List<Configs> Configs
    //    {
    //        get
    //        {
    //            if (_configs == null)
    //            {
    //                InitConfigs();
    //            }
    //            return _configs;
    //        }
    //    }

    //    public static void Reload()
    //    {
    //        _configs = null;
    //    }

    //    public static void InitConfigs()
    //    {
    //        _configs = (List<Configs>)MainBll.InitConfig();
    //    }

    //    #endregion
    //    #region methods

    //    /// <summary>
    //    /// 获取配置信息
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <returns></returns>
    //    public static string GetValue(string key)
    //    {
    //        return Configs.Where(c => c.ConfigKey == key).Select(c => c.ConfigValue).FirstOrDefault();
    //    }

    //    /// <summary>
    //    /// 设置值
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="value"></param>
    //    public static void SetValue(string key, string value)
    //    {
    //        Configs config = Configs.FirstOrDefault(c => c.ConfigKey == key);
    //        if (config == null) return;
    //        if (config.ConfigValue == value) return;
    //        ChangedKeys.Add(key);
    //        config.ConfigValue = value;
    //    }

    //    /// <summary>
    //    /// 保存所有更改的配置项
    //    /// </summary>
    //    public static void SaveAll()
    //    {
    //        var allChangeOfConfig = (IQueryable<Configs>)MainBll.SaveAllWithConfig(ChangedKeys);
    //        foreach (var item in allChangeOfConfig)
    //        {
    //            item.ConfigValue = GetValue(item.ConfigKey);
    //        }
    //        MainBll.Save();
    //        Reload();
    //    }

    //    #endregion
    //    #region properties

    //    /// <summary>
    //    /// 网站标题
    //    /// </summary>
    //    public static string Title
    //    {
    //        get
    //        {
    //            return GetValue("Title");
    //        }
    //        set
    //        {
    //            SetValue("Title", value);
    //        }
    //    }

    //    /// <summary>
    //    /// 列表每页显示的个数
    //    /// </summary>
    //    public static Int32 PageSize
    //    {
    //        get
    //        {
    //            return Convert.ToInt32(GetValue("PageSize"));
    //        }
    //        set
    //        {
    //            SetValue("PageSize", value.ToString(CultureInfo.InvariantCulture));
    //        }
    //    }

    //    /// <summary>
    //    /// 帮助下拉列表
    //    /// </summary>
    //    public static string HelpList
    //    {
    //        get
    //        {
    //            return GetValue("HelpList");
    //        }
    //        set
    //        {
    //            SetValue("HelpList", value);
    //        }
    //    }


    //    /// <summary>
    //    /// 菜单样式
    //    /// </summary>
    //    public static string MenuType
    //    {
    //        get
    //        {
    //            return GetValue("MenuType");
    //        }
    //        set
    //        {
    //            SetValue("MenuType", value);
    //        }
    //    }


    //    /// <summary>
    //    /// 网站主题
    //    /// </summary>
    //    public static string Theme
    //    {
    //        get
    //        {
    //            return GetValue("Theme");
    //        }
    //        set
    //        {
    //            SetValue("Theme", value);
    //        }
    //    }


    //    #endregion
    //}
}
