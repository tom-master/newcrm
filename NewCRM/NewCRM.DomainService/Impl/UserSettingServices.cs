using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
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
    [Export(typeof(IUserSettingServices))]
    public class UserSettingServices : IUserSettingServices
    {
        [Import]
        private IUserRepository _userRepository;

        [Import]
        private IDeskRepository _deskRepository;

        [Import]
        private IWallpaperRepository _wallpaperRepository;

        public void ModifyDefaultShowDesk(Int32 userId, Int32 newDefaultDeskNumber)
        {
            var userResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);

            userResult.Config.ModifyDefaultDesk(newDefaultDeskNumber);

            _userRepository.Update(userResult);
        }

        public void ModifyAppDirection(Int32 userId, String direction)
        {
            var userResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);

            if (direction.ToLower() == "x")
            {
                userResult.Config.ModifyAppDirectionToX();
            }
            else if (direction.ToLower() == "y")
            {
                userResult.Config.ModifyAppDirectionToY();
            }
            else
            {
                throw new BusinessException($"未能识别的App排列方向:{direction.ToLower()}");
            }


            _userRepository.Update(userResult);
        }

        public void ModifyAppIconSize(Int32 userId, Int32 newSize)
        {
            var userResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);
            userResult.Config.ModifyDisplayIconLength(newSize);
            _userRepository.Update(userResult);
        }

        public void ModifyAppVerticalSpacing(Int32 userId, Int32 newSize)
        {
            var userResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);
            userResult.Config.ModifyAppVerticalSpacingLength(newSize);
            _userRepository.Update(userResult);
        }

        public void ModifyAppHorizontalSpacing(Int32 userId, Int32 newSize)
        {
            var userResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);
            userResult.Config.ModifyAppHorizontalSpacingLength(newSize);
            _userRepository.Update(userResult);
        }

        public void ModifyDockPosition(Int32 userId, Int32 defaultDeskNumber, String newPosition)
        {
            var userResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);

            DockPostion dockPostion;
            if (Enum.TryParse(newPosition, true, out dockPostion))
            {
                if (dockPostion == DockPostion.None)
                {
                    var deskResult = _deskRepository.Entities.FirstOrDefault(desk => desk.DeskNumber == userResult.Config.DefaultDeskNumber);

                    var dockMembers = deskResult.Members.Where(member => member.IsOnDock == true);

                    if (dockMembers.Any())
                    {
                        dockMembers.ToList().ForEach(
                        f =>
                        {
                            f.MoveOutDock();
                        });
                        _deskRepository.Update(deskResult);
                    }
                    userResult.Config.ModifyDockPostion(dockPostion);

                }
                else
                {
                    userResult.Config.ModifyDockPostion(dockPostion);
                }
            }
            else
            {
                throw new BusinessException($"未识别出的码头位置:{newPosition}");
            }
            userResult.Config.ModifyDefaultDesk(defaultDeskNumber);

            _userRepository.Update(userResult);
        }

        public IList<Wallpaper> GetWallpaper()
        {
            return _wallpaperRepository.Entities.Where(wallpaper => wallpaper.Source == WallpaperSource.System).ToList();
        }

        public void ModifyWallpaperMode(Int32 userId, String newMode)
        {
            WallpaperMode wallpaperMode;

            if (Enum.TryParse(newMode, true, out wallpaperMode))
            {
                var userResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);
                userResult.Config.ModifyDisplayMode(wallpaperMode);
                _userRepository.Update(userResult);
            }
            else
            {
                throw new BusinessException($"无法识别的壁纸显示模式:{newMode}");
            }

        }

        public void ModifyWallpaper(Int32 userId, Int32 newWallpaperId)
        {
            var userResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);

            var wallpaperResult =
                _wallpaperRepository.Entities.FirstOrDefault(wallpaper => wallpaper.Id == newWallpaperId);

            userResult.Config.ModifyWallpaper(wallpaperResult);

            _userRepository.Update(userResult);
        }

        public Tuple<Int32, String> AddWallpaper(Wallpaper wallpaper)
        {

            if (_wallpaperRepository.Entities.Count(w => w.UserId == wallpaper.UserId) == 6)
            {
                throw new BusinessException($"最多只能上传6张壁纸");
            }

            _wallpaperRepository.Add(wallpaper);

            return new Tuple<Int32, String>(wallpaper.Id, wallpaper.Url);
        }

        public IList<Wallpaper> GetUploadWallpaper(Int32 userId)
        {
            var wallpapers = _wallpaperRepository.Entities.Where(wallpaper => wallpaper.UserId == userId);

            return wallpapers.ToList();
        }

        public void RemoveWallpaper(Int32 userId, Int32 wallpaperId)
        {

            var userResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);

            if (userResult.Config.Wallpaper.Id == wallpaperId)
            {
                throw new BusinessException($"当前壁纸正在使用或已被删除");
            }


            var defaultWallpaper = _wallpaperRepository.Entities.FirstOrDefault(wallpaper => wallpaper.Source == WallpaperSource.System);

            userResult.Config.ModifyWallpaper(defaultWallpaper);

            _userRepository.Update(userResult);

            _wallpaperRepository.Remove(wallpaper => wallpaper.Id == wallpaperId);

        }
    }
}
