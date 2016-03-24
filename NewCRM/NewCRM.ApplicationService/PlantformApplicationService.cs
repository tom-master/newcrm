using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NewCRM.ApplicationService.IApplicationService;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.DomainService;
using NewCRM.DomainService.Impl;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomHelper;

namespace NewCRM.ApplicationService
{
    public class PlantformApplicationService : IPlantformApplicationService
    {
        private readonly IPlatformDomainService _platformDomainService = new PlatformDomainService();

        public ResponseInformation<UserDto> Login(String userName, String passWord)
        {
            Parameter.Vaildate(userName, passWord);
            var userResult = _platformDomainService.VaildateUser(userName, passWord);
            return userResult != null ? new ResponseInformation<UserDto>(ResponseType.Success, userResult.ConvertToDto<User, UserDto>()) : new ResponseInformation<UserDto>(ResponseType.PasswordInvalid | ResponseType.QueryNull);
        }

        public ResponseInformation<dynamic> UserApp(Int32 userId)
        {
            Parameter.Vaildate(false, userId);
            var appResult = _platformDomainService.UserApp(userId);

            return appResult != null ? new ResponseInformation<dynamic>(ResponseType.Success, appResult) : new ResponseInformation<dynamic>(ResponseType.Warning);
        }

        public ResponseInformation<Boolean> DefaultDesk(Int32 userId, Int32 deskId)
        {
            Parameter.Vaildate(false, userId, deskId);

            var isSuccessSetDefaultDesk = _platformDomainService.SetDefaultDesk(userId, deskId);
            return new ResponseInformation<Boolean>(isSuccessSetDefaultDesk ? ResponseType.Success : ResponseType.QueryNull | ResponseType.Error);
        }

        public ResponseInformation<Boolean> AppDirection(Int32 userId, String direction)
        {
            Parameter.Vaildate(false, userId);

            Parameter.Vaildate(direction);
            var isSuccessSetAppDirection = _platformDomainService.SetAppDirection(userId, direction);
            return new ResponseInformation<Boolean>(isSuccessSetAppDirection ? ResponseType.Success : ResponseType.QueryNull | ResponseType.Error);

        }

        public ResponseInformation<Boolean> AppSize(Int32 userId, Int32 appSize)
        {
            Parameter.Vaildate(false, userId, appSize);

            var isSuccessSetAppSize = _platformDomainService.SetAppSize(userId, appSize);
            return new ResponseInformation<Boolean>(isSuccessSetAppSize ? ResponseType.Success : ResponseType.QueryNull | ResponseType.Error);
        }

        public ResponseInformation<Boolean> AppVertical(Int32 userId, Int32 appVertical)
        {
            Parameter.Vaildate(false, userId, appVertical);

            var isSuccessSetAppVertical = _platformDomainService.SetAppVertical(userId, appVertical);
            return new ResponseInformation<Boolean>(isSuccessSetAppVertical ? ResponseType.Success : ResponseType.QueryNull | ResponseType.Error);
        }

        public ResponseInformation<Boolean> AppHorizontal(Int32 userId, Int32 appHorizontal)
        {
            Parameter.Vaildate(false, userId, appHorizontal);

            var isSuccessSetAppVertical = _platformDomainService.SetAppHorizontal(userId, appHorizontal);
            return new ResponseInformation<Boolean>(isSuccessSetAppVertical ? ResponseType.Success : ResponseType.QueryNull | ResponseType.Error);
        }

        public ResponseInformation<Boolean> DockPosition(Int32 userId, String pos, Int32 deskNum)
        {
            Parameter.Vaildate(false, userId, deskNum);
            Parameter.Vaildate(pos);
            var isSuccessSetAppVertical = _platformDomainService.SetDockPosition(userId, pos, deskNum);
            return new ResponseInformation<Boolean>(isSuccessSetAppVertical ? ResponseType.Success : ResponseType.QueryNull | ResponseType.Error);
        }

        public ResponseInformation<Boolean> Skin(Int32 userId, String skin)
        {
            Parameter.Vaildate(false, userId);
            Parameter.Vaildate(skin);

            var isSuccessSetSkin = _platformDomainService.SetSkin(userId, skin);

            return new ResponseInformation<Boolean>(isSuccessSetSkin ? ResponseType.Success : ResponseType.QueryNull | ResponseType.Error);
        }

        public ResponseInformation<IDictionary<String, dynamic>> AllSkin(String skinPath)
        {
            IDictionary<String, dynamic> dataDictionary = new Dictionary<String, dynamic>();
            Directory.GetFiles(skinPath, "*.css").ToList().ForEach(path =>
            {
                var fileName = Get(path, x => x.LastIndexOf(@"\", StringComparison.OrdinalIgnoreCase) + 1).Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0];
                dataDictionary.Add(fileName, new
                {
                    cssPath = path.Substring(path.LastIndexOf("script", StringComparison.OrdinalIgnoreCase) - 1).Replace(@"\", "/"),
                    imgPath = GetLocalImagePath(fileName, skinPath)
                });
            });
            return new ResponseInformation<IDictionary<String, dynamic>>(ResponseType.Success, dataDictionary);
        }

        public async Task<ResponseInformation<Boolean>> WebImgAsync(Int32 userId, String webImg)
        {
            var imageResult = await GetWebImgInfoAsync(webImg);

            var isSuccessSetWebWallpaper = _platformDomainService.SetWebWallpaper(userId, imageResult);
            return
                new ResponseInformation<Boolean>(isSuccessSetWebWallpaper
                    ? ResponseType.Success
                    : ResponseType.QueryNull | ResponseType.Error);
        }

        public ResponseInformation<IDictionary<Int32, Tuple<String, String>>> Wallpapers()
        {
            var result = _platformDomainService.GetWallpapers();
            if (result.Any())
            {
                return new ResponseInformation<IDictionary<Int32, Tuple<String, String>>>(ResponseType.Success, result);
            }
            return new ResponseInformation<IDictionary<Int32, Tuple<String, String>>>(ResponseType.Warning);
        }

        #region 内部使用
        private String GetLocalImagePath(String fileName, String fullPath)
        {
            var dic =
                Directory.GetFiles(fullPath, "preview.png",
                    SearchOption.AllDirectories).ToList();
            foreach (var dicItem in from dicItem in dic let regex = new Regex(fileName) where regex.IsMatch(dicItem) select dicItem)
            {
                return dicItem.Substring(dicItem.LastIndexOf("script", StringComparison.OrdinalIgnoreCase) - 1).Replace(@"\", "/");
            }
            return "";
        }

        private String Get(String path, Func<String, Int32> filterFunc)
        {
            return path.Substring(filterFunc(path));
        }

        private async Task<dynamic> GetWebImgInfoAsync(String webImgUrl)
        {
            var imageTitle = Path.GetFileNameWithoutExtension(webImgUrl);
            Image image = null;
            using (HttpClient httpClient = new HttpClient())
            {
                using (var imageStream = await httpClient.GetStreamAsync(new Uri(webImgUrl)))
                {
                    using (image = Image.FromStream(imageStream)) { }
                }
            }

            return new
            {
                imageTitle,
                width = image.Width,
                height = image.Height,
                url = webImgUrl
            };
        }
        #endregion

    }
}