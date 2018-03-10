using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Infrastructure.CommonTools
{
	public static class CacheKey
	{
		private static readonly String _cachePrefix = "NewCrm";

		public class ConfigCacheKey: CacheKeyBase
		{
			public override TimeSpan CancelToken { get => throw new NotImplementedException(); }
			public override TimeSpan Timeout { get => throw new NotImplementedException(); }
			public override String Key { get { return "{0}:Config:AccountId:{1}"; } protected set { } }

			public override CacheKeyBase SetIdentity(Int32 identity)
			{
				Key = String.Format(Key, _cachePrefix, identity);
				return this;
			}
		}




		public static String Config(Int32 accountId) => $@"{_cachePrefix}:Config:AccountId:{accountId}";

		public static String Wallpaper(Int32 wallpaperId) => $@"{_cachePrefix}:Wallpaper:WallpaperId:{wallpaperId}";

		public static String Account(Int32 accountId) => $@"{_cachePrefix}:Account:AccountId:{accountId}";

		public static String AccountRoles(Int32 accountId) => $@"{_cachePrefix}:Roles:AccountId:{accountId}";

		public static String Powers() => $@"{_cachePrefix}:Powers";

		public static String Desktop(Int32 accountId) => $@"{_cachePrefix}:Desktop:AccountId:{accountId}";

		public static String AppTypes() => $@"{_cachePrefix}:AppTypes";
	}
}
