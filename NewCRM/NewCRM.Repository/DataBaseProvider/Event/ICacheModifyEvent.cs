using System;

namespace NewCRM.Repository.DataBaseProvider.Event
{
    internal interface ICacheModifyEvent
    {
        event EventHandler OnCacheModify;

        void CacheModify();
    }
}
