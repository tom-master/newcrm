using System;
using System.Collections.Generic;
using NewCRM.Domain.Entitys.System;

namespace NewCRM.Domain.Services.Interface
{
    public interface IAppContext
    {
        List<App> GetApps(Int32 accountId, Int32 appTypeId, Int32 orderId, String searchText, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);

        List<App> GetAccountApps(Int32 accountId, String searchText, Int32 appTypeId, Int32 appStyleId, String appState, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);
    }
}
