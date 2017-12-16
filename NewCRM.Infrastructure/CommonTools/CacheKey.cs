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


        public static String Config(Int32 accountId)
        {
            return $@"{_cachePrefix}:Config:AccountId:{accountId}";
        }

        public static String Wallpaper(Int32 wallpaperId)
        {
            return $@"{_cachePrefix}:Wallpaper:WallpaperId:{wallpaperId}";
        }
    }
}
