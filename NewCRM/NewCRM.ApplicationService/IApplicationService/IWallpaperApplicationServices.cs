using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NewCRM.Dto.Dto;

namespace NewCRM.Application.Services.IApplicationService
{
    public interface IWallpaperApplicationServices
    {
        #region wallpaper
        /// <summary>
        /// 获取所有的系统壁纸
        /// </summary>
        /// <returns></returns>
        IList<WallpaperDto> GetWallpaper();

        /// <summary>
        /// 修改壁纸的显示模式
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newMode"></param>
        void ModifyWallpaperMode(Int32 userId, String newMode);

        /// <summary>
        /// 修改壁纸
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newWallpaperId"></param>
        void ModifyWallpaper(Int32 userId, Int32 newWallpaperId);

        /// <summary>
        /// 添加壁纸
        /// </summary>
        /// <param name="wallpaperDto"></param>
        /// <returns></returns>
        Tuple<Int32, String> AddWallpaper(WallpaperDto wallpaperDto);

        /// <summary>
        /// 根据用户id获取上传的壁纸
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IList<WallpaperDto> GetUploadWallpaper(Int32 userId);

        /// <summary>
        /// 根据用户id删除上传的壁纸
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="wallpaperId"></param>
        void RemoveWallpaper(Int32 userId, Int32 wallpaperId);

        /// <summary>
        /// 根据用户id添加来自于网络的壁纸
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        Task<Tuple<Int32, String>> AddWebWallpaper(Int32 userId, String url);

        /// <summary>
        /// 根据md5获取上传的壁纸
        /// </summary>
        /// <param name="md5"></param>
        /// <returns></returns>
        WallpaperDto GetUploadWallpaper(String md5);
        #endregion
    }
}
