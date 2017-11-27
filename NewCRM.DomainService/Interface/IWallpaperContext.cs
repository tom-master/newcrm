using System;
using System.Collections.Generic;
using NewCRM.Domain.Entitys.System;

namespace NewCRM.Domain.Services.Interface
{
    public interface IWallpaperContext
    {
        /// <summary>
        /// 获取壁纸
        /// </summary>
        List<Wallpaper> GetWallpapers();

        /// <summary>
        /// 添加壁纸
        /// </summary>
        Tuple<Int32, String> AddWallpaper(Wallpaper wallpaper);

        /// <summary>
        /// 获取上传的壁纸
        /// </summary>
        List<Wallpaper> GetUploadWallpaper(Int32 accountId);

        /// <summary>
        /// 获取上传的壁纸
        /// </summary>
        Wallpaper GetUploadWallpaper(String md5);

        /// <summary>
        /// 修改壁纸的显示模式
        /// </summary>
        void ModifyWallpaperMode(Int32 accountId, String newMode);

        /// <summary>
        /// 修改壁纸
        /// </summary>
        void ModifyWallpaper(Int32 accountId, Int32 newWallpaperId);

        /// <summary>
        /// 移除壁纸
        /// </summary>
        void RemoveWallpaper(Int32 accountId, Int32 wallpaperId);
    }
}
