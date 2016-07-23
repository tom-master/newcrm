using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.Repositories.IRepository.Account;
using NewCRM.Domain.Entities.Repositories.IRepository.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.Impl
{
    [Export(typeof(IWallpaperServices))]
    public class WallpaperServices : BaseService, IWallpaperServices
    {
        [Import]
        private IWallpaperRepository _wallpaperRepository;


        public IList<Wallpaper> GetWallpaper()
        {
            return _wallpaperRepository.Entities.Where(wallpaper => wallpaper.Source == WallpaperSource.System).ToList();
        }

        public void ModifyWallpaperMode(Int32 userId, String newMode)
        {
            WallpaperMode wallpaperMode;

            if (Enum.TryParse(newMode, true, out wallpaperMode))
            {
                var userResult = GetUser(userId);
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
            var userResult = GetUser(userId);

            var wallpaperResult =
                _wallpaperRepository.Entities.FirstOrDefault(wallpaper => wallpaper.Id == newWallpaperId);

            userResult.Config.ModifyWallpaper(wallpaperResult);

            UserRepository.Update(userResult);
        }

        public Tuple<Int32, String> AddWallpaper(Wallpaper wallpaper)
        {
            if (_wallpaperRepository.Entities.Count(w => w.UserId == wallpaper.UserId) == 6)
            {
                throw new BusinessException($"最多只能上传6张壁纸");
            }

            _wallpaperRepository.Add(wallpaper);

            return new Tuple<Int32, String>(wallpaper.Id, wallpaper.ShortUrl);
        }

        public IList<Wallpaper> GetUploadWallpaper(Int32 userId)
        {
            var wallpapers = _wallpaperRepository.Entities.Where(wallpaper => wallpaper.UserId == userId);

            return wallpapers.ToList();
        }

        public void RemoveWallpaper(Int32 userId, Int32 wallpaperId)
        {

            var userResult = GetUser(userId);

            if (userResult.Config.Wallpaper.Id == wallpaperId)
            {
                throw new BusinessException($"当前壁纸正在使用或已被删除");
            }

            _wallpaperRepository.Remove(wallpaper => wallpaper.Id == wallpaperId);

        }

        public Wallpaper GetUploadWallpaper(String md5)
        {
            var wallpaperResult = _wallpaperRepository.Entities.FirstOrDefault(wallpaper => wallpaper.Md5 == md5);

            return wallpaperResult;
        }
    }
}
