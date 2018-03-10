using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Infrastructure.CommonTools
{
	public abstract class CacheKeyBase
	{
		public abstract String Key { get; protected set; }

		public abstract TimeSpan CancelToken { get; }

		public abstract TimeSpan Timeout { get; }

		public abstract CacheKeyBase SetIdentity(Int32 identity);
	}
}
