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

        public static String Account(Int32 accountId)
        {
            return $@"{_cachePrefix}:Account:AccountId:{accountId}";
        }

        public static String Roles(Int32 accountId)
        {
            return $@"{_cachePrefix}:Roles:AccountId:{accountId}";
        }

        public static String Powers()
        {
            return $@"{_cachePrefix}:Powers";
        }

        public static String Desktop(Int32 accountId)
        {
            return $@"{_cachePrefix}:Desktop:AccountId:{accountId}";
        }

        public static String AppTypes()
        {
            return $@"{_cachePrefix}:AppTypes";
        }
    }
}
