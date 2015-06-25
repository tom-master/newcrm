using System;
using System.Collections.Generic;
using NewCRM.Domain.DomainModel.System;

namespace NewCRM.DomainService
{
    public interface IPlatformDomainService
    {
        /// <summary>
        /// 获取所有的壁纸
        /// </summary>
        /// <returns> ICollection<Wallpaper/></returns>
        ICollection<Wallpaper> GetWallpapers();

        /// <summary>
        /// 设置壁纸
        /// </summary>
        /// <param name="wallpaperId">壁纸Id</param>
        /// <param name="wallPaperShowType">壁纸的显示方式</param>
        /// <param name="userId">用户Id</param>
        void SetWallPaper(Int32 wallpaperId, String wallPaperShowType, Int32 userId);

        /// <summary>
        /// 获取用户上传的壁纸
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>ICollection<WallpaperDto/></returns>
        ICollection<Wallpaper> GetUserUploadWallPaper(Int32 userId);
    }
}