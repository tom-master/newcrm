using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Infrastructure.CommonTools.CustomExtension
{
    public static class BooleanExtension
    {
        public static Int32 ParseToInt32(this Boolean boo)
        {
            return boo ? 1 : 0;
        }
    }
}
