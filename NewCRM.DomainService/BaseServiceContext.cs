using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NewCRM.Infrastructure.CommonTools;
using NewLib.Data.Redis.InternalHelper;
using NewLib.Validate;

namespace NewCRM.Domain.Services
{

    public class BaseServiceContext
    {
        private static readonly ICacheQueryProvider _cacheQuery = new DefaultRedisQueryProvider();

        protected ParameterValidate Parameter => new ParameterValidate();

        protected async Task<TModel> GetCache<TModel>(CacheKeyBase cache, Func<Task<TModel>> func) where TModel : class
        {
            var cts = new CancellationTokenSource(cache.CancelToken);

            TModel cacheResult = null;
            try
            {
                cacheResult = await Task.Run(() => _cacheQuery.StringGetAsync<TModel>(cache.GetKey()), cts.Token);
            }
            catch(OperationCanceledException)
            {
            }

            if(cacheResult != null)
            {
                return cacheResult;
            }

            var dbResult = await func();
            _cacheQuery.StringSet(cache.GetKey(), dbResult, cache.KeyTimeout);
            return dbResult;
        }

        /// <summary>
        /// 更新时移除旧的缓存键
        /// </summary>
        protected void RemoveOldKeyWhenModify(params CacheKeyBase[] caches)
        {
            if(caches.Any())
            {
                return;
            }

            foreach(var cache in caches)
            {
                if(_cacheQuery.KeyExists(cache.GetKey()))
                {
                    _cacheQuery.KeyDelete(cache.GetKey());
                }
            }
        }

    }
}
