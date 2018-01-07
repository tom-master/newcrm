using System;
using System.Collections.Generic;
using NewCRM.Domain.Entitys;
using NewLib.Data.Redis.InternalHelper;
using NewLib.Validate;

namespace NewCRM.Domain
{

    public class BaseServiceContext
    {
        private static readonly ICacheQueryProvider _cacheQuery = new DefaultRedisQueryProvider();

        /// <summary>
        /// 参数验证
        /// </summary>
        protected ParameterValidate ValidateParameter => new ParameterValidate();

        protected TModel GetCache<TModel>(String cacheKey, Func<TModel> func)
        {
            var cacheResult = _cacheQuery.StringGet<TModel>(cacheKey);
            if (cacheResult != null)
            {
                return cacheResult;
            }

            var dbResult = func();
            _cacheQuery.StringSet(cacheKey, dbResult);
            return dbResult;
        }

        protected IList<TModel> GetCache<TModel>(String cacheKey, Func<IList<TModel>> func)
        {
            var cacheResult = _cacheQuery.StringGet<IList<TModel>>(cacheKey);
            if (cacheResult != null)
            {
                return cacheResult;
            }

            var dbResult = func();
            _cacheQuery.StringSet(cacheKey, dbResult);
            return dbResult;
        }

        /// <summary>
        /// 更新时移除旧的缓存键
        /// </summary>
        protected void RemoveOldKeyWhenModify(String cacheKey)
        {
            if (_cacheQuery.KeyExists(cacheKey))
            {
                _cacheQuery.KeyDelete(cacheKey);
            }
        }
    }
}
