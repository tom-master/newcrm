using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Infrastructure.CommonTools
{
	public static class CacheKey
	{

		public class ConfigCacheKey: CacheKeyBase
		{
			public ConfigCacheKey(Int32 identity) : base(identity) { }

			public override String Key { get => "{0}:Config:AccountId:{1}"; protected set { } }
		}

		public class WallpaperCacheKey: CacheKeyBase
		{
			public WallpaperCacheKey(Int32 identity) : base(identity) { }

			public override String Key { get => "{0}:Wallpaper:WallpaperId:{1}"; protected set { } }
		}

		public class AccountCacheKey: CacheKeyBase
		{
			public AccountCacheKey(Int32 identity) : base(identity) { }

			public override String Key { get => "{0}:Account:AccountId:{1}"; protected set { } }
		}

		public class AccountRoleCacheKey: CacheKeyBase
		{
			public AccountRoleCacheKey(Int32 identity) : base(identity) { }

			public override String Key { get => "{0}:Roles:AccountId:{1}"; protected set { } }
		}

		public class PowersCacheKey: CacheKeyBase
		{
			public PowersCacheKey(Int32 identity) : base(identity) { }

			public override String Key { get => "{0}:Powers"; protected set { } }
		}

		public class DesktopCacheKey: CacheKeyBase
		{
			public DesktopCacheKey(Int32 identity) : base(identity) { }

			public override String Key { get => "{0}:Desktop:AccountId:{1}"; protected set { } }
		}

		public class AppTypeCacheKey: CacheKeyBase
		{
			public AppTypeCacheKey(Int32 identity = default(Int32)) : base(identity) { }

			public override String Key { get => "{0}:AppTypes"; protected set { } }
		}
	}
}
