using System;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Domain.DomainModel.System;

namespace NewCRM.DomainService
{
    public interface IUserDomainService
    {
        User ValidateUser(String userName, String userPassword);

        User Wallpaper(Int32 userId);

        String App(User entityUser);

        String BuildWindow(Int32 id, String type);

        /// <summary>
        /// 删除用户上传的壁纸
        /// </summary>
        /// <param name="wallPaperId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Boolean DeleteWallPaper(Int32 wallPaperId, Int32 userId);
        /// <summary>
        /// 上传壁纸
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="wallpaper">壁纸Id</param>
        /// <returns>dynamic</returns>
        Wallpaper UploadWallPaper(Int32 userId, Wallpaper wallpaper);
        /// <summary>
        /// 设置网络图片作为壁纸
        /// </summary>
        /// <param name="wallpaper"></param>
        Int32 SetWebWallPaper(Wallpaper wallpaper);

        /// <summary>
        /// 获取登录的用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>User</returns>
        User GetUser(Int32 userId);
    }
}