using System;

namespace NewCRM.Repository.DataBaseProvider.Redis.CacheEvent
{
    internal interface ICacheCreateEvent
    {
        event EventHandler OnCacheCreate;

        void CacheCreate();
    }
}
