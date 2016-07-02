using NewCRM.Application.Services.IApplicationService;

namespace NewCRM.Application.Services
{
    public class PlantformApplicationService : IPlantformApplicationService
    {
        //private readonly IPlatformDomainService _platformDomainService = new PlatformDomainService();
        //private static Parameter _parameter = new Parameter();

        //public ResponseInformation<UserDto> Login(String userName, String passWord)
        //{
        //    _parameter.Vaildate(userName).Vaildate(passWord);
        //    var userResult = _platformDomainService.VaildateUser(userName, passWord);
        //    return userResult != null ? new ResponseInformation<UserDto>(ResponseType.Success, userResult.ConvertToDto<User, UserDto>()) : new ResponseInformation<UserDto>(ResponseType.PasswordInvalid | ResponseType.QueryNull);
        //}

        //public ResponseInformation<dynamic> UserApp(Int32 userId)
        //{
        //    _parameter.Vaildate(userId);
        //    var appResult = _platformDomainService.UserApp(userId);

        //    return appResult != null ? new ResponseInformation<dynamic>(ResponseType.Success, appResult) : new ResponseInformation<dynamic>(ResponseType.Warning);
        //}

        //public ResponseInformation<Boolean> DefaultDesk(Int32 userId, Int32 deskId)
        //{
        //    _parameter.Vaildate(userId).Vaildate(deskId);
        //    var isSuccessSetDefaultDesk = _platformDomainService.SetDefaultDesk(userId, deskId);
        //    return new ResponseInformation<Boolean>(isSuccessSetDefaultDesk ? ResponseType.Success : ResponseType.QueryNull | ResponseType.Error);
        //}

        //public ResponseInformation<Boolean> AppDirection(Int32 userId, String direction)
        //{
        //    _parameter.Vaildate(userId).Vaildate(direction);
        //    var isSuccessSetAppDirection = _platformDomainService.SetAppDirection(userId, direction);
        //    return new ResponseInformation<Boolean>(isSuccessSetAppDirection ? ResponseType.Success : ResponseType.QueryNull | ResponseType.Error);

        //}

        //public ResponseInformation<Boolean> AppSize(Int32 userId, Int32 appSize)
        //{
        //    _parameter.Vaildate(userId).Vaildate(appSize);

        //    var isSuccessSetAppSize = _platformDomainService.SetAppSize(userId, appSize);
        //    return new ResponseInformation<Boolean>(isSuccessSetAppSize ? ResponseType.Success : ResponseType.QueryNull | ResponseType.Error);
        //}

        //public ResponseInformation<Boolean> AppVertical(Int32 userId, Int32 appVertical)
        //{
        //    _parameter.Vaildate(userId).Vaildate(appVertical);

        //    var isSuccessSetAppVertical = _platformDomainService.SetAppVertical(userId, appVertical);
        //    return new ResponseInformation<Boolean>(isSuccessSetAppVertical ? ResponseType.Success : ResponseType.QueryNull | ResponseType.Error);
        //}

        //public ResponseInformation<Boolean> AppHorizontal(Int32 userId, Int32 appHorizontal)
        //{
        //    _parameter.Vaildate(userId).Vaildate(appHorizontal);

        //    var isSuccessSetAppVertical = _platformDomainService.SetAppHorizontal(userId, appHorizontal);
        //    return new ResponseInformation<Boolean>(isSuccessSetAppVertical ? ResponseType.Success : ResponseType.QueryNull | ResponseType.Error);
        //}

        //public ResponseInformation<Boolean> DockPosition(Int32 userId, String pos, Int32 deskNum)
        //{
        //    _parameter.Vaildate(userId).Vaildate(pos).Vaildate(deskNum);
        //    var isSuccessSetAppVertical = _platformDomainService.SetDockPosition(userId, pos, deskNum);
        //    return new ResponseInformation<Boolean>(isSuccessSetAppVertical ? ResponseType.Success : ResponseType.QueryNull | ResponseType.Error);
        //}

        //public ResponseInformation<Boolean> Skin(Int32 userId, String skin)
        //{
        //    _parameter.Vaildate(userId).Vaildate(skin);
        //    var isSuccessSetSkin = _platformDomainService.SetSkin(userId, skin);

        //    return new ResponseInformation<Boolean>(isSuccessSetSkin ? ResponseType.Success : ResponseType.QueryNull | ResponseType.Error);
        //}

        //public ResponseInformation<IDictionary<String, dynamic>> AllSkin(String skinPath)
        //{
        //    _parameter.Vaildate(skinPath);
        //    IDictionary<String, dynamic> dataDictionary = new Dictionary<String, dynamic>();
        //    Directory.GetFiles(skinPath, "*.css").ToList().ForEach(path =>
        //    {
        //        var fileName = Get(path, x => x.LastIndexOf(@"\", StringComparison.OrdinalIgnoreCase) + 1).Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0];
        //        dataDictionary.Add(fileName, new
        //        {
        //            cssPath = path.Substring(path.LastIndexOf("script", StringComparison.OrdinalIgnoreCase) - 1).Replace(@"\", "/"),
        //            imgPath = GetLocalImagePath(fileName, skinPath)
        //        });
        //    });
        //    return new ResponseInformation<IDictionary<String, dynamic>>(ResponseType.Success, dataDictionary);
        //}

        //public async Task<ResponseInformation<Boolean>> WebImgAsync(Int32 userId, String webImg)
        //{
        //    _parameter.Vaildate(userId).Vaildate(webImg);
        //    var imageResult = await GetWebImgInfoAsync(webImg);

        //    var isSuccessSetWebWallpaper = _platformDomainService.SetWebWallpaper(userId, imageResult);
        //    return
        //        new ResponseInformation<Boolean>(isSuccessSetWebWallpaper
        //            ? ResponseType.Success
        //            : ResponseType.QueryNull | ResponseType.Error);
        //}

        //public ResponseInformation<IDictionary<Int32, Tuple<String, String>>> Wallpapers()
        //{
        //    var result = _platformDomainService.GetWallpapers();
        //    return result.Any() ? new ResponseInformation<IDictionary<Int32, Tuple<String, String>>>(ResponseType.Success, result) : new ResponseInformation<IDictionary<Int32, Tuple<String, String>>>(ResponseType.Warning);
        //}

        //public ResponseInformation<Boolean> Wallpaper(Int32 userId, Int32 wallpaperId, String wallPaperShowType)
        //{

        //    _parameter.Vaildate(userId).Vaildate(wallpaperId).Vaildate(wallPaperShowType);

        //    var isSuccessSetWallpaper = _platformDomainService.SetWallpaper(userId, wallpaperId, wallPaperShowType);
        //    return new ResponseInformation<Boolean>(isSuccessSetWallpaper ? ResponseType.Success : ResponseType.QueryNull | ResponseType.Error);
        //}

        //public ResponseInformation<IList<Tuple<Int32, String>>> UploadWallPaper(Int32 userId)
        //{
        //    _parameter.Vaildate(userId);
        //    var uploadWallpaperResult = _platformDomainService.GetUploadWallPaper(userId);
        //    return uploadWallpaperResult.Any()
        //        ? new ResponseInformation<IList<Tuple<Int32, String>>>(ResponseType.Success, uploadWallpaperResult)
        //        : new ResponseInformation<IList<Tuple<Int32, String>>>(ResponseType.Fail);
        //}

        //#region 内部使用
        //private String GetLocalImagePath(String fileName, String fullPath)
        //{
        //    var dic =
        //        Directory.GetFiles(fullPath, "preview.png",
        //            SearchOption.AllDirectories).ToList();
        //    foreach (var dicItem in from dicItem in dic let regex = new Regex(fileName) where regex.IsMatch(dicItem) select dicItem)
        //    {
        //        return dicItem.Substring(dicItem.LastIndexOf("script", StringComparison.OrdinalIgnoreCase) - 1).Replace(@"\", "/");
        //    }
        //    return "";
        //}

        //private String Get(String path, Func<String, Int32> filterFunc)
        //{
        //    return path.Substring(filterFunc(path));
        //}

        //private async Task<dynamic> GetWebImgInfoAsync(String webImgUrl)
        //{
        //    var imageTitle = Path.GetFileNameWithoutExtension(webImgUrl);
        //    Image image;
        //    using (HttpClient httpClient = new HttpClient())
        //    {
        //        using (var imageStream = await httpClient.GetStreamAsync(new Uri(webImgUrl)))
        //        {
        //            using (image = Image.FromStream(imageStream)) { }
        //        }
        //    }

        //    return new
        //    {
        //        imageTitle,
        //        width = image.Width,
        //        height = image.Height,
        //        url = webImgUrl
        //    };
        //}
        //#endregion

    }
}