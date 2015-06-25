using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.DomainModel.System;
using NewCRM.Infrastructure.CommonTools.CustemException;
using NewCRM.Infrastructure.Repositories.Repositories.Account;
using NewCRM.Infrastructure.Repositories.Repositories.Account.Impl;
using NewCRM.Infrastructure.Repositories.Repositories.System;

namespace NewCRM.DomainService.Impl
{
    /// <summary>
    /// 平台服务
    /// </summary>
    public class PlatformDomainService : BaseService, IPlatformDomainService
    {

        private readonly IWallpaperRepository _wallpaperRepository;
        private readonly IUserRepository _userRepository;

        public PlatformDomainService(IWallpaperRepository wallpaperRepository)
        {
            _wallpaperRepository = wallpaperRepository;
            _userRepository = new UserRepository();
        }
        /// <summary>
        /// 获取所有的壁纸
        /// </summary>
        /// <returns> ICollection<Wallpaper/></returns>
        public ICollection<Wallpaper> GetWallpapers()
        {
            return _wallpaperRepository.Entities.Where(wallPaper => wallPaper.IsSystem == true).ToList();
        }

        /// <summary>
        /// 设置壁纸
        /// </summary>
        /// <param name="wallpaperId">壁纸Id</param>
        /// <param name="wallPaperShowType">壁纸的显示方式</param>
        /// <param name="userId">用户Id</param>
        public void SetWallPaper(Int32 wallpaperId, String wallPaperShowType, Int32 userId)
        {
            var userEntity = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);
            if (userEntity == null)
            {
                throw new RepositoryException(String.Format("更换壁纸失败，请重试"));
            }
            if (wallpaperId != 0)
            {
                userEntity.Wallpaper = _wallpaperRepository.Entities.FirstOrDefault(wallPaper => wallPaper.Id == wallpaperId && wallPaper.IsDeleted == false);
            }
            userEntity.WallpaperShowType = wallPaperShowType;
            _userRepository.Update(userEntity);
        }

        /// <summary>
        /// 获取用户上传的壁纸
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>ICollection<WallpaperDto/></returns>
        public ICollection<Wallpaper> GetUserUploadWallPaper(Int32 userId)
        {
            return _wallpaperRepository.Entities.Where(wallPaper => wallPaper.UploaderId == userId && wallPaper.IsDeleted == false).ToList();
        }
    }
}