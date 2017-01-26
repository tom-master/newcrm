using System;

namespace NewCRM.Repository.DataBaseProvider.Event
{
    internal interface ICacheCreateEvent
    {
        event EventHandler OnCacheCreate;

        void CacheCreate();
    }
}
