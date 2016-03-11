using System;
using System.Collections.Generic;
using NewCRM.Dto.Dto;

namespace NewCRM.ApplicationService.IApplicationService
{
    public interface IPlantformApplicationService
    {
        /// <summary>
        /// 获取所有的壁纸
        /// </summary>
        /// <returns> ICollection<Wallpaper/></returns>
        ICollection<WallpaperDto> GetWallpapers();
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
        ICollection<WallpaperDto> GetUserUploadWallPaper(Int32 userId);
        /// <summary>
        /// 获取所有的皮肤
        /// </summary>
        /// <param name="skinFullPath">皮肤路径</param>
        /// <returns>IDictionary<String, dynamic/> </returns>
        IDictionary<String, dynamic> GetAllSkin(String skinFullPath);
        /// <summary>
        /// 修改平台的皮肤
        /// </summary>
        /// <param name="skinName">皮肤名称</param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        Boolean UpdateSkin(String skinName, Int32 userId);
        /// <summary>
        /// 更新默认显示的桌面
        /// </summary>
        /// <param name="deskNum">默认桌面编号</param>
        /// <param name="userId">用户Id</param>
        /// <returns>Boolean</returns>
        Boolean UpdateDefaultDesk(Int32 deskNum, Int32 userId);
        /// <summary>
        /// 更新应用的排列方向
        /// </summary>
        /// <param name="direction">排列方向</param>
        /// <param name="userId">用户Id</param>
        /// <returns>Boolean</returns>
        Boolean UpdateAppXy(String direction, Int32 userId);
        /// <summary>
        /// 更新应用大小
        /// </summary>
        /// <param name="appSize">app大小</param>
        /// <param name="userId">用户Id</param>
        /// <returns>Boolean</returns>
        Boolean UpdateAppSize(Int32 appSize, Int32 userId);
        /// <summary>
        /// 更新应用码头的位置
        /// </summary>
        /// <param name="pos">码头的新位置</param>
        /// <param name="deskNum">桌面Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>Boolean</returns>
        Boolean UpdateDockPosition(String pos, Int32 deskNum, Int32 userId);
        /// <summary>
        /// 更新应用图标的垂直间距
        /// </summary>
        /// <param name="appVertical">垂直艰巨</param>
        /// <param name="userId">用户Id</param>
        /// <returns>Boolean</returns>
        Boolean UpdateAppVertical(Int32 appVertical, Int32 userId);
        /// <summary>
        /// 更新应用图标的水平间距
        /// </summary>
        /// <param name="appHorizontal">水平间距</param>
        /// <param name="userId">用户Id</param>
        /// <returns>Boolean</returns>
        Boolean UpdateAppHorizontal(Int32 appHorizontal, Int32 userId);
    }
}