using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.ApplicationService.IApplicationService;
using NewCRM.Domain.DomainModel.System;
using NewCRM.DomainService;
using NewCRM.DomainService.Impl;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomHelper;
using NewCRM.Infrastructure.Repositories;
using NewCRM.Infrastructure.Repositories.Repositories.System.Impl;

namespace NewCRM.ApplicationService
{
    public class PlantformApplicationService : IPlantformApplicationService
    {
        private readonly IPlatformDomainService _platformDomainService;

        public PlantformApplicationService()
        {
            _platformDomainService = new PlatformDomainService(RepositoryFactory<Wallpaper, WallPaperRepository>.GetRepository());
        }
        /// <summary>
        /// 获取所有的壁纸
        /// </summary>
        /// <returns> ICollection<Wallpaper/></returns>
        public ICollection<WallpaperDto> GetWallpapers()
        {
            return _platformDomainService.GetWallpapers().ConvertToDto<Wallpaper, WallpaperDto>();
        }

        /// <summary>
        /// 设置壁纸
        /// </summary>
        /// <param name="wallpaperId">壁纸Id</param>
        /// <param name="wallPaperShowType">壁纸的显示方式</param>
        /// <param name="userId">用户Id</param>
        public void SetWallPaper(Int32 wallpaperId, String wallPaperShowType, Int32 userId)
        {
            PublicHelper.VaildateArgument(wallpaperId, "wallpaperId", true);
            PublicHelper.VaildateArgument(wallPaperShowType, "wallPaperShowType");
            PublicHelper.VaildateArgument(userId, "userId");
            _platformDomainService.SetWallPaper(wallpaperId, wallPaperShowType, userId);
        }

        /// <summary>
        /// 获取用户上传的壁纸
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>ICollection<WallpaperDto/></returns>
        public ICollection<WallpaperDto> GetUserUploadWallPaper(Int32 userId)
        {
            PublicHelper.VaildateArgument(userId, "userId");
            return _platformDomainService.GetUserUploadWallPaper(userId).ConvertToDto<Wallpaper, WallpaperDto>();
        }
    }
}