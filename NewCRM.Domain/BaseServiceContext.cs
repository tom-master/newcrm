using System;
using NewCRM.Domain.Entitys;
using NewLib.Data.Redis.InternalHelper;
using NewLib.Validate;

namespace NewCRM.Domain
{

    public class BaseServiceContext
    {
        private readonly ICacheQueryProvider _cacheQuery = new DefaultRedisQueryProvider();

        /// <summary>
        /// 参数验证
        /// </summary>
        protected ParameterValidate ValidateParameter => new ParameterValidate();

        protected TModel ExecuteGet<TModel>(String cacheKey, Func<TModel> func) where TModel : DomainModelBase
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
    }
}
