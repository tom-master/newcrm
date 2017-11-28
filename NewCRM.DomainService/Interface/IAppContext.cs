using System;
using System.Collections.Generic;
using NewCRM.Domain.Entitys.System;

namespace NewCRM.Domain.Services.Interface
{
    public interface IAppContext
    {
        /// <summary>
        /// 获取所有的app
        /// </summary>
        List<App> GetApps(Int32 accountId, Int32 appTypeId, Int32 orderId, String searchText, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);

        /// <summary>
        /// 获取当前账户下所有的app
        /// </summary>
        List<App> GetAccountApps(Int32 accountId, String searchText, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);

        /// <summary>
        /// 获取app
        /// </summary>
        App GetApp(Int32 appId);

        /// <summary>
        /// 是否已经安装app
        /// </summary>
        Boolean IsInstallApp(Int32 accountId, Int32 appId);

        /// <summary>
        /// 获取系统App
        /// </summary>
        List<App> GetSystemApp(IEnumerable<Int32> appIds = default(IEnumerable<Int32>));

        /// <summary>
        /// 更改app评分
        /// </summary>
        void ModifyAppStar(Int32 accountId, Int32 appId, Int32 starCount);

        /// <summary>
        /// 创建新的app
        /// </summary>
        void CreateNewApp(App app);

        /// <summary>
        /// app审核通过
        /// </summary>
        void Pass(Int32 appId);

        /// <summary>
        /// app审核不通过
        /// </summary>
        void Deny(Int32 appId);

        /// <summary>
        /// 设置今日推荐app
        /// </summary>
        void SetTodayRecommandApp(Int32 appId);

        /// <summary>
        /// 移除app
        /// </summary>
        void RemoveApp(Int32 appId);

        /// <summary>
        /// 发布app
        /// </summary>
        void ReleaseApp(Int32 appId);

        /// <summary>
        /// 修改开发者（用户）的app信息
        /// </summary>
        void ModifyAccountAppInfo(Int32 accountId, App app);

        /// <summary>
        /// 删除App分类
        /// </summary>
        void DeleteAppType(Int32 appTypeId);

        /// <summary>
        /// 创建新的app分类
        /// </summary>
        void CreateNewAppType(AppType appType);

        /// <summary>
        /// 修改app分类
        /// </summary>
        void ModifyAppType(String appTypeName, Int32 appTypeId);

        /// <summary>
        /// 更改app图标
        /// </summary>
        void ModifyAppIcon(Int32 accountId, Int32 appId, String newIcon);

        /// <summary>
        /// 用户安装app
        /// </summary> 
        void Install(Int32 accountId, Int32 appId, Int32 deskNum);

        /// <summary>
        /// 获取当前账户下已开发和未发布的app
        /// </summary>
        Tuple<Int32, Int32> GetAccountDevelopAppCountAndNotReleaseAppCount(Int32 accountId);

        /// <summary>
        /// 获取所有App类型
        /// </summary>
        /// <returns></returns>
        List<AppType> GetAppTypes();

        /// <summary>
        /// 获取今日推荐
        /// </summary>
        dynamic GetTodayRecommend(Int32 accountId);
    }
}
