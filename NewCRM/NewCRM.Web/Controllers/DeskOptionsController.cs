using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    [Export]
    public class DeskOptionsController : BaseController
    {


        #region 页面

        // //// GET: PlatformSetting
        /// <summary>
        /// 系统壁纸
        /// </summary>
        /// <returns></returns>
        public ActionResult SystemWallPaper()
        {
            ViewData["CurrentUser"] = CurrentUser;
            ViewData["Wallpapers"] = WallpaperApplicationServices.GetWallpaper();
            return View();
        }

        /// <summary>
        /// 自定义壁纸
        /// </summary>
        /// <returns></returns>
        public ActionResult CustomWallPaper()
        {
            ViewData["CurrentUser"] = CurrentUser;
            return View();
        }

        /// <summary>
        /// 设置皮肤
        /// </summary>
        /// <returns></returns>
        public ActionResult SetSkin()
        {
            ViewData["CurrentUser"] = CurrentUser;
            return View();
        }

        /// <summary>
        /// 程序设置
        /// </summary>
        /// <returns></returns>
        public ActionResult DeskSet()
        {
            ViewData["CurrentUser"] = CurrentUser;
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
            WallpaperApplicationServices.ModifyWallpaperMode(CurrentUser.Id, wallPaperShowType);
            return Json(new { success = 1 });
        }

        /// <summary>
        /// 设置壁纸
        /// </summary>
        /// <param name="wallpaperId"></param>
        /// <returns></returns>
        public ActionResult ModifyWallpaper(Int32 wallpaperId)
        {
            WallpaperApplicationServices.ModifyWallpaper(CurrentUser.Id, wallpaperId);
            return Json(new { success = 1 });
        }

        /// <summary>
        /// 载入用户之前上传的壁纸
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllUploadWallPaper()
        {
            var wallPapers = WallpaperApplicationServices.GetUploadWallpaper(CurrentUser.Id);
            return Json(new { wallPapers }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除上传的壁纸
        /// </summary>
        /// <param name="wallPaperId"></param>
        /// <returns></returns>
        public ActionResult DeleteWallPaper(Int32 wallPaperId = 0)
        {
            WallpaperApplicationServices.RemoveWallpaper(CurrentUser.Id, wallPaperId);
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

                var wallpaperDtoResult = WallpaperApplicationServices.GetUploadWallpaper(CalculateFile.Calculate(httpPostedFile.InputStream));
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

                    var wallpaperResult = WallpaperApplicationServices.AddWallpaper(new WallpaperDto
                    {
                        Height = fileUpLoad.FileHeight,
                        Source = "Upload",
                        Title = fileUpLoad.OldFileName,
                        Url = fileUpLoad.FilePath + fileUpLoad.OldFileName,
                        UserId = CurrentUser.Id,
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
            var webWallpaperResult = WallpaperApplicationServices.AddWebWallpaper(CurrentUser.Id, webUrl);
            return Json(new { data = await webWallpaperResult });
        }

        /// <summary>
        /// 获取全部的皮肤
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllSkin()
        {
            var skinPath = Server.MapPath(ConfigurationManager.AppSettings["PlantFormSkinPath"]);
            var data = SkinApplicationServices.GetAllSkin(skinPath);
            return Json(new { data, currentSkin = CurrentUserConfig.Skin });
        }

        /// <summary>
        /// 更换皮肤
        /// </summary>
        /// <param name="skin"></param>
        /// <returns></returns>
        public ActionResult ModifySkin(String skin = "")
        {
            SkinApplicationServices.ModifySkin(CurrentUser.Id, skin);
            return Json(new { success = 1 });

        }

        /// <summary>
        /// 更换默认显示的桌面
        /// </summary>
        /// <param name="deskNum"></param>
        /// <returns></returns>
        public ActionResult ModifyDefaultDesk(Int32 deskNum)
        {
            DeskApplicationServices.ModifyDefaultDeskNumber(CurrentUser.Id, deskNum);
            return Json(new { success = 1 });
        }

        /// <summary>
        /// 更换图标的排列方向
        /// </summary>
        /// <param name="appXy"></param>
        /// <returns></returns>
        public ActionResult ModifyAppXy(String appXy)
        {
            AppApplicationServices.ModifyAppDirection(CurrentUser.Id, appXy);
            return Json(new { success = 1 });
        }

        /// <summary>
        /// 更改图标大小
        /// </summary>
        /// <param name="appSize"></param>
        /// <returns></returns>
        public ActionResult ModifyAppSize(Int32 appSize)
        {
            AppApplicationServices.ModifyAppIconSize(CurrentUser.Id, appSize);
            return Json(new { success = 1 });
        }

        /// <summary>
        /// 更改应用图标的垂直间距
        /// </summary>
        /// <param name="appVertical"></param>
        /// <returns></returns>
        public ActionResult ModifyAppVertical(Int32 appVertical)
        {
            AppApplicationServices.ModifyAppVerticalSpacing(CurrentUser.Id, appVertical);

            return Json(new { success = 1 });
        }

        /// <summary>
        /// 修改图标的水平间距
        /// </summary>
        /// <param name="appHorizontal"></param>
        /// <returns></returns>
        public ActionResult ModifyAppHorizontal(Int32 appHorizontal)
        {
            AppApplicationServices.ModifyAppHorizontalSpacing(CurrentUser.Id, appHorizontal);

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
            DeskApplicationServices.ModifyDockPosition(CurrentUser.Id, deskNum, pos);

            return Json(new { success = 1 });
        }

    }
}