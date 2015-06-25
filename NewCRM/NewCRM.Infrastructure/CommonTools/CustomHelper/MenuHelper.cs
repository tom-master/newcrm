//using Apps = NewCRM.Models.App;
namespace NewCRM.Infrastructure.CommonTools.CustomHelper
{
    //public sealed class MenuHelper
    //{
    //    /// <summary>
    //    /// 注入首页加载的对象
    //    /// </summary>
    //    private static readonly IMainPageDal MainBll;

    //    static MenuHelper()
    //    {
    //        MainBll = CreatefactoryDal.CreateDal<IMainPageDal>("MainPageDal");
    //    }

    //    private static List<Apps> _menu;
    //    public static IEnumerable<Apps> Menus
    //    {
    //        get
    //        {
    //            if (_menu == null)
    //            {
    //                InitMenus();
    //            }
    //            return _menu;
    //        }
    //    }
    //    public static void Reload()
    //    {
    //        _menu = null;
    //    }
    //    private static void InitMenus()
    //    {
    //        _menu = new List<Apps>();
    //        List<Apps> dbMenu = (List<Apps>)MainBll.InitMenus();
    //        ResolveMenuCollection(dbMenu, null, 0);
    //    }
    //    private static Int32 ResolveMenuCollection(IEnumerable<Apps> dbMenus, Apps parentMenu, Int32 level)
    //    {
    //        Int32 count = 0;
    //        var enumerable = dbMenus as Apps[] ?? dbMenus.ToArray();
    //        var v = enumerable.Where(menu => menu.Parent == parentMenu);
    //        foreach (var menuItem in v)
    //        {
    //            count++;
    //            _menu.Add(menuItem);
    //            menuItem.TreeLevel = level;
    //            menuItem.IsTreeLeaf = true;
    //            menuItem.Enabled = true;
    //            level++;
    //            int childCount = ResolveMenuCollection(enumerable, menuItem, level);
    //            if (childCount != 0)
    //            {
    //                menuItem.IsTreeLeaf = false;
    //            }
    //            level--;
    //        }
    //        return count;
    //    }

    //    /// <summary>
    //    /// 获取当前用户权限下的可看见的菜单 
    //    /// </summary>
    //    /// <returns></returns>
    //    public static IEnumerable<Apps> ResolveUserMenuList()
    //    {
    //        //获取当前用户的权限列表
    //        List<string> rolePowerName = PowerHelper.GetRolePowerName();
    //        //获取当前用户所属的脚色可用的菜单
    //        return Menus.Where(menuItem => menuItem.ViewPower == null || rolePowerName.Contains(menuItem.ViewPower.Name)).ToList();
    //    }

    //    #region builderMenuOfAccordion
    //    public static Accordion InitAccordionMenu(IEnumerable<Apps> menus, Region region)
    //    {
    //        Accordion accordionMenu = new Accordion
    //        {
    //            ID = "accordionMenu",
    //            EnableFill = true,
    //            ShowBorder = false,
    //            ShowHeader = false
    //        };
    //        region.Controls.Add(accordionMenu);
    //        IEnumerable<Apps> enumerable = menus as IList<Apps> ?? menus.ToList();
    //        var v = enumerable.Where(menu => menu.Parent == null);
    //        foreach (var menuItem in v)
    //        {
    //            AccordionPane panel = new AccordionPane
    //            {
    //                Title = menuItem.Name,
    //                Layout = Layout.Fit,
    //                ShowBorder = false,
    //                BodyPadding = "2px 0 0 0"
    //            };
    //            accordionMenu.Controls.Add(panel);
    //            Tree innerTree = new Tree
    //            {
    //                EnableArrows = true,
    //                ShowBorder = false,
    //                ShowHeader = false,
    //                EnableIcons = false,
    //                AutoScroll = true,
    //            };
    //            panel.Controls.Add(innerTree);

    //            ResolveMenuTree(enumerable, menuItem, innerTree.Nodes);
    //        }
    //        return accordionMenu;
    //    }
    //    #endregion
    //    #region builderMenuOfNormalTree
    //    public static Tree InitTree(IEnumerable<Apps> menus, Region region)
    //    {
    //        Tree tree = new Tree
    //        {
    //            ID = "treeMenu",
    //            EnableArrows = true,
    //            ShowBorder = false,
    //            ShowHeader = false,
    //            EnableIcons = false,
    //            AutoScroll = true
    //        };
    //        region.Items.Add(tree);

    //        ResolveMenuTree(menus, null, tree.Nodes);
    //        tree.Nodes[0].Expanded = true;

    //        return tree;
    //    }
    //    #endregion
    //    private static Int32 ResolveMenuTree(IEnumerable<Apps> menus, Apps parentMenu, TreeNodeCollection nodeCollection)
    //    {
    //        Int32 count = 0;
    //        if (menus == null) return count;
    //        foreach (var menuItem in menus.Where(menu => menu.Parent == parentMenu))
    //        {
    //            TreeNode node = new TreeNode();
    //            nodeCollection.Add(node);
    //            count++;

    //            node.Text = menuItem.Name;
    //            node.IconUrl = menuItem.ImageUrl;
    //            if (!String.IsNullOrEmpty(menuItem.NavigateUrl))
    //            {
    //                node.EnablePostBack = false;
    //                node.NavigateUrl = new Control().ResolveUrl(menuItem.NavigateUrl);
    //            }

    //            if (menuItem.IsTreeLeaf)
    //            {
    //                node.Leaf = true;
    //                if (String.IsNullOrEmpty(menuItem.NavigateUrl))
    //                {
    //                    nodeCollection.Remove(node);
    //                    count--;
    //                }
    //            }
    //            else
    //            {
    //                //node.en = true;
    //                Int32 childCount = ResolveMenuTree(menus, menuItem, node.Nodes);

    //                //如果是子目录，但是计算的子节点是0，可能目录里面都是空的 ，所以删除父目录
    //                if (childCount == 0 && String.IsNullOrEmpty(menuItem.NavigateUrl))
    //                {
    //                    nodeCollection.Remove(node);
    //                    count--;
    //                }
    //            }
    //        }
    //        return count;
    //    }
    //}
}
