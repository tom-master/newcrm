using System;
using System.Threading.Tasks;
using NewLib.Data.Redis.InternalHelper;
using NewLib.Validate;

namespace NewCRM.Domain.Services
{

	public class BaseServiceContext
	{
		private static readonly ICacheQueryProvider _cacheQuery = new DefaultRedisQueryProvider();

		protected ParameterValidate Parameter => new ParameterValidate();

		protected async Task<TModel> GetCache<TModel>(String cacheKey, Func<Task<TModel>> func)
		{
			var cacheResult = _cacheQuery.StringGetAsync<TModel>(cacheKey);
			if (cacheResult != null)
			{
				return await cacheResult;
			}

			var dbResult = await func();
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
