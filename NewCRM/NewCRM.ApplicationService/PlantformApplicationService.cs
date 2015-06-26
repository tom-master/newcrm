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

        /// <summary>
        /// 获取所有的皮肤
        /// </summary>
        /// <param name="skinFullPath">皮肤路径</param>
        /// <returns>IDictionary<String, dynamic/> </returns>
        public IDictionary<String, dynamic> GetAllSkin(String skinFullPath)
        {
            PublicHelper.VaildateArgument(skinFullPath, "skinFullPath");

            return _platformDomainService.GetAllSkin(skinFullPath);
        }

        /// <summary>
        /// 修改平台的皮肤
        /// </summary>
        /// <param name="skinName">皮肤名称</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public Boolean UpdateSkin(String skinName, Int32 userId)
        {
            PublicHelper.VaildateArgument(skinName, "skinName");
            PublicHelper.VaildateArgument(userId, "userId");
            return _platformDomainService.UpdateSkin(skinName, userId);
        }

        /// <summary>
        /// 更新默认显示的桌面
        /// </summary>
        /// <param name="deskNum">默认桌面编号</param>
        /// <param name="userId">用户Id</param>
        /// <returns>Boolean</returns>
        public Boolean UpdateDefaultDesk(Int32 deskNum, Int32 userId)
        {
            PublicHelper.VaildateArgument(deskNum, "deskNum");
            PublicHelper.VaildateArgument(userId, "userId");
            return _platformDomainService.UpdateDefaultDesk(deskNum, userId);
        }
        /// <summary>
        /// 更新应用的排列方向
        /// </summary>
        /// <param name="direction">排列方向</param>
        /// <param name="userId">用户Id</param>
        /// <returns>Boolean</returns>
        public Boolean UpdateAppXy(String direction, Int32 userId)
        {
            PublicHelper.VaildateArgument(direction, "direction");
            PublicHelper.VaildateArgument(userId, "userId");
            return _platformDomainService.UpdateAppXy(direction, userId);
        }

        /// <summary>
        /// 更新应用大小
        /// </summary>
        /// <param name="appSize">app大小</param>
        /// <param name="userId">用户Id</param>
        /// <returns>Boolean</returns>
        public Boolean UpdateAppSize(String appSize, Int32 userId)
        {
            PublicHelper.VaildateArgument(appSize, "appSize");
            PublicHelper.VaildateArgument(userId, "userId");
            return _platformDomainService.UpdateAppSize(appSize, userId);
        }

        /// <summary>
        /// 更新应用码头的位置
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="deskNum"></param>
        /// <param name="userId"></param>
        /// <returns>Boolean</returns>
        public Boolean UpdateDockPosition(String pos, Int32 deskNum, Int32 userId)
        {
            PublicHelper.VaildateArgument(pos, "pos");
            PublicHelper.VaildateArgument(deskNum, "deskNum");
            PublicHelper.VaildateArgument(userId, "userId");
            return _platformDomainService.UpdateDockPosition(pos, deskNum, userId);
        }
    }
}