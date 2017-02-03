using System;

namespace NewCRM.Repository.DataBaseProvider.CacheEvent
{
    internal interface ICacheModifyEvent
    {
        event EventHandler OnCacheModify;

        void CacheModify();
    }
}
