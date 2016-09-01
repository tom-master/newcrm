using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.Impl
{
    [Export(typeof(IWallpaperServices))]
    internal class WallpaperServices : BaseService, IWallpaperServices
    {
        public void ModifyWallpaperMode(Int32 accountId, String newMode)
        {
            WallpaperMode wallpaperMode;

            if (Enum.TryParse(newMode, true, out wallpaperMode))
            {
                var accountResult = GetAccountInfoService(accountId);
                accountResult.Config.ModifyDisplayMode(wallpaperMode);
                AccountRepository.Update(accountResult);
            }
            else
            {
                throw new BusinessException($"无法识别的壁纸显示模式:{newMode}");
            }

        }

        public void ModifyWallpaper(Int32 accountId, Int32 newWallpaperId)
        {
            var accountResult = GetAccountInfoService(accountId);

            var wallpaperResult =
                WallpaperRepository.Entities.FirstOrDefault(wallpaper => wallpaper.Id == newWallpaperId);

            accountResult.Config.ModifyWallpaper(wallpaperResult);

            AccountRepository.Update(accountResult);
        }

        public Tuple<Int32, String> AddWallpaper(Wallpaper wallpaper)
        {
            if (WallpaperRepository.Entities.Count(w => w.AccountId == wallpaper.AccountId) == 6)
            {
                throw new BusinessException($"最多只能上传6张壁纸");
            }

            WallpaperRepository.Add(wallpaper);

            return new Tuple<Int32, String>(wallpaper.Id, wallpaper.ShortUrl);
        }
 
        public void RemoveWallpaper(Int32 accountId, Int32 wallpaperId)
        {

            var accountResult = GetAccountInfoService(accountId);

            if (accountResult.Config.Wallpaper.Id == wallpaperId)
            {
                throw new BusinessException($"当前壁纸正在使用或已被删除");
            }

            WallpaperRepository.Remove(wallpaper => wallpaper.Id == wallpaperId);

        }
    }
}
