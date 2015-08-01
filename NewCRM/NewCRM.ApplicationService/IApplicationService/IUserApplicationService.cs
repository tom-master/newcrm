using System;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;

namespace NewCRM.ApplicationService.IApplicationService
{
    public interface IUserApplicationService
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns>UserDto</returns>
        UserDto UserLogin(String userName, String passWord);
        /// <summary>
        /// 获取登录的用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>UserDto</returns>
        UserDto GetUser(Int32 userId);
        /// <summary>
        /// 获取用户设置的壁纸
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserDto GetUserWallPaper(Int32 userId);
        /// <summary>
        /// 获取当前用户所拥有的app
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>String</returns>
        String GetUserApp(UserDto userDto);

        /// <summary>
        /// 构建一个应用窗体
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns>String</returns>
        String BuilderWindow(Int32 id, String type);

        /// <summary>
        /// 删除用户上传的壁纸
        /// </summary>
        /// <param name="wallPaperId">壁纸Id</param>
        /// <param name="userId">用户Id</param>
        /// <returns>Boolean</returns>
        Boolean DeleteWallPaper(Int32 wallPaperId, Int32 userId);

        /// <summary>
        /// 上传壁纸
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fileUpLoadHelper"></param>
        /// <returns>dynamic</returns>
        WallpaperDto UploadWallPaper(Int32 userId, FileUpLoadHelper fileUpLoadHelper);

        /// <summary>
        /// 设置网络图片作为背景
        /// </summary>
        /// <param name="webUrl"></param>
        Int32 SetWebWallPaper(String webUrl);
    }
}