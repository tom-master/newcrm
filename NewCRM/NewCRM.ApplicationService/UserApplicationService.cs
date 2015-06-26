using NewCRM.ApplicationService.IApplicationService;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Domain.DomainModel.System;
using NewCRM.DomainService;
using NewCRM.DomainService.Impl;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomHelper;
using NewCRM.Infrastructure.Repositories;
using NewCRM.Infrastructure.Repositories.Repositories.Account.Impl;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using NewCRM.Infrastructure.CommonTools.CustemException;


namespace NewCRM.ApplicationService
{
    /// <summary>
    /// 应用服务层
    /// </summary>
    public class UserApplicationService : IUserApplicationService
    {
        private readonly IUserDomainService _userDomainService;

        public UserApplicationService()
        {
            _userDomainService = new UserDomainService(RepositoryFactory<User, UserRepository>.GetRepository());
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <returns></returns>
        public UserDto UserLogin(String userName, String passWord)
        {
            PublicHelper.VaildateArgument(userName, "userName");
            PublicHelper.VaildateArgument(passWord, "passWord");
            return _userDomainService.ValidateUser(userName, passWord).ConvertToDto<User, UserDto>();
        }

        /// <summary>
        /// 获取登陆后的用户的壁纸
        /// </summary>
        /// <returns></returns>
        public UserDto GetUserWallPaper(Int32 userId)
        {
            PublicHelper.VaildateArgument(userId, "userId");
            return _userDomainService.Wallpaper(userId).ConvertToDto<User, UserDto>();
        }

        /// <summary>
        /// 获取当前用户所拥有的app
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns>String</returns>
        public String GetUserApp(UserDto userDto)
        {
            PublicHelper.VaildateArgument(userDto, "userDto");
            return _userDomainService.App(userDto.ConvertToDomainModel<UserDto, User>());
        }


        public string BuilderWindow(Int32 id, String type)
        {
            PublicHelper.VaildateArgument(id, "id");
            PublicHelper.VaildateArgument(type, "type");
            return _userDomainService.BuildWindow(id, type);
        }

        /// <summary>
        /// 删除用户上传的壁纸
        /// </summary>
        /// <param name="wallPaperId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Boolean DeleteWallPaper(Int32 wallPaperId, Int32 userId)
        {
            PublicHelper.VaildateArgument(wallPaperId, "wallPaperId");
            PublicHelper.VaildateArgument(userId, "userId");
            return _userDomainService.DeleteWallPaper(wallPaperId, userId);
        }

        /// <summary>
        /// 上传壁纸
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="fileUpLoadHelper">上传类对象</param>
        /// <returns>Boolean</returns>
        public WallpaperDto UploadWallPaper(Int32 userId, FileUpLoadHelper fileUpLoadHelper)
        {
            PublicHelper.VaildateArgument(userId, "userId");
            PublicHelper.VaildateArgument(fileUpLoadHelper, "fileUpLoadHelper");
            var wallPaperDto = new WallpaperDto
            {
                Title = fileUpLoadHelper.NewFileName,
                Url = fileUpLoadHelper.FilePath + fileUpLoadHelper.NewFileName,
                SmallUrl = fileUpLoadHelper.WebThumbnailFilePath.Substring(fileUpLoadHelper.WebThumbnailFilePath.LastIndexOf("scripts", StringComparison.Ordinal)).Replace('\\', '/').Insert(0, "../"),
                WallpaperWebUrl = fileUpLoadHelper.WebFilePath,
                Description = fileUpLoadHelper.NewFileName,
                Width = fileUpLoadHelper.FileWidth,
                Heigth = fileUpLoadHelper.FileHeight,
                IsSystem = false,
                WallpaperType = "自定义",
                UploaderId = userId
            };
            return _userDomainService.UploadWallPaper(userId,
                wallPaperDto.ConvertToDomainModel<WallpaperDto, Wallpaper>()).ConvertToDto<Wallpaper, WallpaperDto>();
        }

        /// <summary>
        /// 设置网络图片作为背景
        /// </summary>
        /// <param name="webUrl"></param>
        public Int32 SetWebWallPaper(String webUrl)
        {
            PublicHelper.VaildateArgument(webUrl, "webUrl");
            using (WebClient webClient = new WebClient())
            {
                var stream = webClient.OpenRead(new Uri(webUrl));
                if (stream != null)
                {
                    Image webImage = Image.FromStream(stream, true);
                    var wallPaperDto = new WallpaperDto
                    {
                        Title = Guid.NewGuid().ToString(),
                        Url = webUrl,
                        SmallUrl = webUrl,
                        WallpaperWebUrl = webUrl,
                        Description = "",
                        Width = webImage.Width,
                        Heigth = webImage.Height,
                        IsSystem = false,
                        WallpaperType = "网络"
                    };
                    return _userDomainService.SetWebWallPaper(wallPaperDto.ConvertToDomainModel<WallpaperDto, Wallpaper>());
                }
                throw new BusinessException("未能获取到所指定的网络图片");
            }
        }

        /// <summary>
        /// 获取登录的用户
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns>UserDto</returns>
        public UserDto GetUser(Int32 userId)
        {
            PublicHelper.VaildateArgument(userId, "userId");
            return _userDomainService.GetUser(userId).ConvertToDto<User, UserDto>();
        }
    }
}