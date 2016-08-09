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

        public IList<Wallpaper> GetWallpaper()
        {
            return WallpaperRepository.Entities.Where(wallpaper => wallpaper.Source == WallpaperSource.System).ToList();
        }

        public void ModifyWallpaperMode(Int32 userId, String newMode)
        {
            WallpaperMode wallpaperMode;

            if (Enum.TryParse(newMode, true, out wallpaperMode))
            {
                var userResult = GetUserInfoService(userId);
                userResult.Config.ModifyDisplayMode(wallpaperMode);
                UserRepository.Update(userResult);
            }
            else
            {
                throw new BusinessException($"无法识别的壁纸显示模式:{newMode}");
            }

        }

        public void ModifyWallpaper(Int32 userId, Int32 newWallpaperId)
        {
            var userResult = GetUserInfoService(userId);

            var wallpaperResult =
                WallpaperRepository.Entities.FirstOrDefault(wallpaper => wallpaper.Id == newWallpaperId);

            userResult.Config.ModifyWallpaper(wallpaperResult);

            UserRepository.Update(userResult);
        }

        public Tuple<Int32, String> AddWallpaper(Wallpaper wallpaper)
        {
            if (WallpaperRepository.Entities.Count(w => w.UserId == wallpaper.UserId) == 6)
            {
                throw new BusinessException($"最多只能上传6张壁纸");
            }

            WallpaperRepository.Add(wallpaper);

            return new Tuple<Int32, String>(wallpaper.Id, wallpaper.ShortUrl);
        }

        public IList<Wallpaper> GetUploadWallpaper(Int32 userId)
        {
            var wallpapers = WallpaperRepository.Entities.Where(wallpaper => wallpaper.UserId == userId);

            return wallpapers.ToList();
        }

        public void RemoveWallpaper(Int32 userId, Int32 wallpaperId)
        {

            var userResult = GetUserInfoService(userId);

            if (userResult.Config.Wallpaper.Id == wallpaperId)
            {
                throw new BusinessException($"当前壁纸正在使用或已被删除");
            }

            WallpaperRepository.Remove(wallpaper => wallpaper.Id == wallpaperId);

        }

        public Wallpaper GetUploadWallpaper(String md5)
        {
            var wallpaperResult = WallpaperRepository.Entities.FirstOrDefault(wallpaper => wallpaper.Md5 == md5);

            return wallpaperResult;
        }
    }
}
