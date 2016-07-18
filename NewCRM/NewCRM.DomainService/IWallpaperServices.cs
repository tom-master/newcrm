using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entities.DomainModel.System;

namespace NewCRM.Domain.Services
{
    public interface IWallpaperServices
    {
        #region wallpaper

        /// <summary>
        /// 获取所有系统自带的壁纸
        /// </summary>
        /// <returns></returns>
        IList<Wallpaper> GetWallpaper();

        /// <summary>
        /// 修改壁纸的显示模式
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newMode"></param>
        void ModifyWallpaperMode(Int32 userId, String newMode);

        /// <summary>
        /// 更换壁纸
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newWallpaperId"></param>
        void ModifyWallpaper(Int32 userId, Int32 newWallpaperId);

        /// <summary>
        /// 添加壁纸
        /// </summary>
        /// <param name="wallpaper"></param>
        /// <returns></returns>
        Tuple<Int32, String> AddWallpaper(Wallpaper wallpaper);

        /// <summary>
        /// 根据用户id获取上传的壁纸
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        IList<Wallpaper> GetUploadWallpaper(Int32 userId);

        /// <summary>
        /// 根据用户id删除壁纸
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="wallpaperId"></param>
        void RemoveWallpaper(Int32 userId, Int32 wallpaperId);

        /// <summary>
        /// 根据md5获取已存在的壁纸
        /// </summary>
        /// <param name="md5"></param>
        /// <returns></returns>
        Wallpaper GetUploadWallpaper(String md5);

        #endregion
    }
}
