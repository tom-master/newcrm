using System;

namespace NewCRM.Domain.Interface.BoundedContextMember
{
    public interface IModifyWallpaperServices
    {
        /// <summary>
        /// 修改壁纸的显示模式
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="newMode"></param>
        void ModifyWallpaperMode(Int32 accountId, String newMode);

        /// <summary>
        /// 更换壁纸
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="newWallpaperId"></param>
        void ModifyWallpaper(Int32 accountId, Int32 newWallpaperId);


        /// <summary>
        /// 根据用户id删除壁纸
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="wallpaperId"></param>
        void RemoveWallpaper(Int32 accountId, Int32 wallpaperId);
    }
}
