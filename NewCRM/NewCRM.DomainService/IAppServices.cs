using System;
using System.Collections.Generic;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Services.Impl;

namespace NewCRM.Domain.Services
{
    public interface IAppServices
    {

        /// <summary>
        /// 获取应用的类型
        /// </summary>
        /// <returns></returns>
        List<AppType> GetAppTypes();

        /// <summary>
        /// 获取用户桌面的成员
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        IDictionary<Int32, IList<dynamic>> GetAccountDeskMembers(Int32 accountId);

        /// <summary>
        /// 获取今日推荐
        /// </summary>
        /// <returns></returns>
        dynamic GetTodayRecommend(Int32 accountId);

        /// <summary>
        /// 获取用户开发的app数量和未发布的app数量
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        Tuple<Int32, Int32> GetAccountDevelopAppCountAndNotReleaseAppCount(Int32 accountId);

        /// <summary>
        /// 获取app
        /// </summary>
        /// <param name="appId"></param>
        /// <returns></returns>
        dynamic GetApp(Int32 appId);

        /// <summary>
        /// 开发者（用户）创建新的app
        /// </summary>
        /// <param name="app"></param>
        void CreateNewApp(App app);

        /// <summary>
        /// 当前用户是否安装了这个app
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="appId"></param>
        /// <returns></returns>
        Boolean IsInstallApp(Int32 accountId, Int32 appId);

        /// <summary>
        /// 修改开发者（用户）的app信息
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="app"></param>
        void ModifyAccountAppInfo(Int32 accountId, App app);

        /// <summary>
        /// app打分
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="appId"></param>
        /// <param name="starCount"></param>
        void ModifyAppStar(Int32 accountId, Int32 appId, Int32 starCount);

        /// <summary>
        /// 用户安装app
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="appId"></param>
        /// <param name="deskNum"></param>
        void InstallApp(Int32 accountId, Int32 appId, Int32 deskNum);

        /// <summary>
        /// 获取当前开发者（用户）的开发的app
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="appName"></param>
        /// <param name="appStyleId"></param>
        /// <param name="appState"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="appTypeId"></param>
        /// <returns></returns>
        List<dynamic> GetAccountAllApps(Int32 accountId, String appName, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex, Int32 pageSize,
            out Int32 totalCount);

        /// <summary>
        /// 获取所有的app
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="appTypeId"></param>
        /// <param name="orderId"></param>
        /// <param name="searchText"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<dynamic> GetAllApps(Int32 accountId, Int32 appTypeId, Int32 orderId, String searchText, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);

    }
}
