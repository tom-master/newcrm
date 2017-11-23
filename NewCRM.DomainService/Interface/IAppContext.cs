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
    }
}
