using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using NewCRM.Application.Interface;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    public class DeskOptionsController : BaseController
    {

        private readonly IWallpaperApplicationServices _wallpaperApplicationServices;

        private readonly ISkinApplicationServices _skinApplicationServices;

        private readonly IDeskApplicationServices _deskApplicationServices;

        private readonly IAppApplicationServices _appApplicationServices;

        private readonly IAccountApplicationServices _accountApplicationServices;

        
        public DeskOptionsController(IWallpaperApplicationServices wallpaperApplicationServices,
            ISkinApplicationServices skinApplicationServices,
            IDeskApplicationServices deskApplicationServices,
            IAppApplicationServices appApplicationServices,
            IAccountApplicationServices accountApplicationServices)
        {
            _wallpaperApplicationServices = wallpaperApplicationServices;

            _skinApplicationServices = skinApplicationServices;

            _deskApplicationServices = deskApplicationServices;

            _appApplicationServices = appApplicationServices;

            _accountApplicationServices = accountApplicationServices;
        }


        #region 页面

        // //// GET: PlatformSetting
        /// <summary>
        /// 系统壁纸
        /// </summary>
        /// <returns></returns>
        public ActionResult SystemWallPaper()
        {
            ViewData["AccountConfig"] = _accountApplicationServices.GetConfig();

            ViewData["Wallpapers"] = _wallpaperApplicationServices.GetWallpaper();

            return View();
        }

        /// <summary>
        /// 自定义壁纸
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomWallPaper()
        {
            ViewData["AccountConfig"] = _accountApplicationServices.GetConfig();

            return View();
        }

        /// <summary>
        /// 设置皮肤
        /// </summary>
        /// <returns></returns>
        public ActionResult SetSkin()
        {
            return View();
        }

        /// <summary>
        /// 程序设置
        /// </summary>
        /// <returns></returns>
        public ActionResult DeskSet()
        {
            ViewData["AccountConfig"] = _accountApplicationServices.GetConfig();

            ViewData["Desks"] = _accountApplicationServices.GetConfig().DefaultDeskCount;

            return View();
        }
        #endregion

        /// <summary>
        /// 设置壁纸显示模式
        /// </summary>
        /// <param name="wallPaperShowType"></param>
        /// <returns></returns>
        public ActionResult ModifyWallpaperDisplayModel(String wallPaperShowType = "")
        {
            //_wallpaperApplicationServices.ModifyWallpaperMode(AccountDto.Id, wallPaperShowType);

            return Json(new { success = 1 });
        }

        /// <summary>
        /// 设置壁纸
        /// </summary>
        /// <param name="wallpaperId"></param>
        /// <returns></returns>
        public ActionResult ModifyWallpaper(Int32 wallpaperId)
        {
            //_wallpaperApplicationServices.ModifyWallpaper(AccountDto.Id, wallpaperId);

            return Json(new { success = 1 });
        }

        /// <summary>
        /// 载入用户之前上传的壁纸
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllUploadWallPaper()
        {
	        var wallPapers = new{}; //_wallpaperApplicationServices.GetUploadWallpaper(AccountDto.Id);

            return Json(new { wallPapers }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除上传的壁纸
        /// </summary>
        /// <param name="wallPaperId"></param>
        /// <returns></returns>
        public ActionResult DeleteWallPaper(Int32 wallPaperId = 0)
        {
           // _wallpaperApplicationServices.RemoveWallpaper(AccountDto.Id, wallPaperId);

            return Json(new { success = 1 });
        }

        /// <summary>
        /// 上传壁纸     
        /// </summary>
        /// <returns></returns>c
        public ActionResult UploadWallPaper()
        {
            if (Request.Files.Count > 0)
            {
                var httpPostedFile = HttpContext.Request.Files[0];

                var wallpaperDtoResult = _wallpaperApplicationServices.GetUploadWallpaper(CalculateFile.Calculate(httpPostedFile.InputStream));

                if (wallpaperDtoResult != null)
                {
                    return Json(new { value = 1, msg = "这张壁纸已经存在" }, JsonRequestBehavior.AllowGet);
                }

                var fileUpLoad = new FileUpLoadHelper(ConfigurationManager.AppSettings["UploadWallPaperPath"], false, false, true, true, 160, 115, ThumbnailMode.Auto, false, "");

                var imgNd5 = CalculateFile.Calculate(httpPostedFile.InputStream);

                if (fileUpLoad.SaveFile(httpPostedFile))
                {
                    var shortUrl =
                        fileUpLoad.WebThumbnailFilePath.Substring(fileUpLoad.WebThumbnailFilePath.LastIndexOf("Script", StringComparison.OrdinalIgnoreCase)).Replace(@"\", "/").Insert(0, "/");

                    var wallpaperResult = _wallpaperApplicationServices.AddWallpaper(new WallpaperDto
                    {
                        Height = fileUpLoad.FileHeight,
                        Source = "Upload",
                        Title = fileUpLoad.OldFileName,
                        Url = fileUpLoad.FilePath + fileUpLoad.OldFileName,
                        //AccountId = AccountDto.Id,
                        Width = fileUpLoad.FileWidth,
                        Md5 = imgNd5,
                        ShortUrl = shortUrl
                    });

                    return Json(new { Id = wallpaperResult.Item1, Url = wallpaperResult.Item2 }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { value = 0, msg = "请先选择一张壁纸。" }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 网络壁纸
        /// </summary>
        /// <param name="webUrl"></param>
        /// <returns></returns>
        public async Task<ActionResult> WebWallPaper(String webUrl = "")
        {
            //var webWallpaperResult = _wallpaperApplicationServices.AddWebWallpaper(AccountDto.Id, webUrl);
            //return Json(new { data = await webWallpaperResult });

	        return null;
        }

        /// <summary>
        /// 获取全部的皮肤
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllSkin()
        {
            var skinPath = Server.MapPath(ConfigurationManager.AppSettings["PlantFormSkinPath"]);

            var data = _skinApplicationServices.GetAllSkin(skinPath);

            return Json(new { data, currentSkin = _accountApplicationServices.GetConfig().Skin });
        }

        /// <summary>
        /// 更换皮肤
        /// </summary>
        /// <param name="skin"></param>
        /// <returns></returns>
        public ActionResult ModifySkin(String skin = "")
        {
            _skinApplicationServices.ModifySkin(skin);

            return Json(new { success = 1 });
        }

        /// <summary>
        /// 更换默认显示的桌面
        /// </summary>
        /// <param name="deskNum"></param>
        /// <returns></returns>
        public ActionResult ModifyDefaultDesk(Int32 deskNum)
        {
            _deskApplicationServices.ModifyDefaultDeskNumber(deskNum);

            return Json(new { success = 1 });
        }

        /// <summary>
        /// 更换图标的排列方向
        /// </summary>
        /// <param name="appXy"></param>
        /// <returns></returns>
        public ActionResult ModifyAppXy(String appXy)
        {
            _appApplicationServices.ModifyAppDirection(appXy);

            return Json(new { success = 1 });
        }

        /// <summary>
        /// 更改图标大小
        /// </summary>
        /// <param name="appSize"></param>
        /// <returns></returns>
        public ActionResult ModifyAppSize(Int32 appSize)
        {
            _appApplicationServices.ModifyAppIconSize(appSize);

            return Json(new { success = 1 });
        }

        /// <summary>
        /// 更改应用图标的垂直间距
        /// </summary>
        /// <param name="appVertical"></param>
        /// <returns></returns>
        public ActionResult ModifyAppVertical(Int32 appVertical)
        {
            _appApplicationServices.ModifyAppVerticalSpacing(appVertical);

            return Json(new { success = 1 });
        }

        /// <summary>
        /// 修改图标的水平间距
        /// </summary>
        /// <param name="appHorizontal"></param>
        /// <returns></returns>
        public ActionResult ModifyAppHorizontal(Int32 appHorizontal)
        {
            _appApplicationServices.ModifyAppHorizontalSpacing(appHorizontal);

            return Json(new { success = 1 });
        }

        /// <summary>
        /// 更改码头的位置
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="deskNum"></param>
        /// <returns></returns>
        public ActionResult ModifyDockPosition(String pos = "", Int32 deskNum = 0)
        {
            _deskApplicationServices.ModifyDockPosition(deskNum, pos);

            return Json(new { success = 1 });
        }

    }
}