using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Mvc;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    [Export]
    public class PlatformSettingController : BaseController
    {
        [Import]
        private IWallpaperApplicationServices _wallpaperApplicationServices;

        [Import]
        private ISkinApplicationServices _skinApplicationServices;

        [Import]
        private IDeskApplicationServices _deskApplicationServices;

        [Import]
        private IAppApplicationServices _appApplicationServices;


        // //// GET: PlatformSetting
        public ActionResult SystemWallPaper()
        {
            ViewData["CurrentUser"] = CurrentUser;
            ViewData["Wallpapers"] = _wallpaperApplicationServices.GetWallpaper();
            return View();
        }

        public ActionResult CustomWallPaper()
        {
            ViewData["CurrentUser"] = CurrentUser;
            return View();
        }

        public ActionResult SettingSkin()
        {
            ViewData["CurrentUser"] = CurrentUser;
            return View();
        }

        public ActionResult DeskTopSetting()
        {
            ViewData["CurrentUser"] = CurrentUser;
            return View();
        }

        //设置壁纸显示模式
        public ActionResult ModifyWallpaperDisplayModel(String wallPaperShowType = "")
        {
            _wallpaperApplicationServices.ModifyWallpaperMode(CurrentUser.Id, wallPaperShowType);
            return null;
        }
        //设置壁纸
        public ActionResult ModifyWallpaper(Int32 wallpaperId)
        {
            _wallpaperApplicationServices.ModifyWallpaper(CurrentUser.Id, wallpaperId);
            return null;
        }

        //载入用户之前上传的壁纸
        public ActionResult GetAllUploadWallPaper()
        {
            var wallPapers = _wallpaperApplicationServices.GetUploadWallpaper(CurrentUser.Id);
            return Json(new { wallPapers }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除上传的壁纸
        /// </summary>
        /// <param name="wallPaperId"></param>
        /// <returns></returns>
        public ActionResult DeleteWallPaper(Int32 wallPaperId = 0)
        {
            _wallpaperApplicationServices.RemoveWallpaper(CurrentUser.Id, wallPaperId);
            return Json(new { data = 1 });
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
                        Heigth = fileUpLoad.FileHeight,
                        Source = WallpaperSource.Upload.ToString(),
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


        public async Task<ActionResult> WebWallPaper(String webUrl = "")
        {
            var webWallpaperResult = _wallpaperApplicationServices.AddWebWallpaper(CurrentUser.Id, webUrl);
            return Json(new { data = await webWallpaperResult });
        }

        /// <summary>
        /// 获取全部的皮肤
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllSkin()
        {
            var skinPath = Server.MapPath(ConfigurationManager.AppSettings["PlantFormSkinPath"]);
            var data = _skinApplicationServices.GetAllSkin(skinPath);
            return Json(new { data, currentSkin = CurrentUser.Skin }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 更换皮肤
        /// </summary>
        /// <param name="skin"></param>
        /// <returns></returns>
        public ActionResult ChangeSkin(String skin = "")
        {
            _skinApplicationServices.ModifySkin(CurrentUser.Id, skin);
            return Json(new { data = 1 }, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 更换默认显示的桌面
        /// </summary>
        /// <param name="deskNum"></param>
        /// <returns></returns>
        public ActionResult ChangeDefaultDesk(Int32 deskNum = 1)
        {
            _deskApplicationServices.ModifyDefaultDeskNumber(CurrentUser.Id, deskNum);
            return Json(new { data = 1 }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 更换图标的排列方向
        /// </summary>
        /// <param name="appXy"></param>
        /// <returns></returns>
        public ActionResult ChangeAppXy(String appXy)
        {
            _appApplicationServices.ModifyAppDirection(CurrentUser.Id, appXy);
            return Json(new { data = 1 }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 更改图标大小
        /// </summary>
        /// <param name="appSize"></param>
        /// <returns></returns>
        public ActionResult ChangeAppSize(Int32 appSize)
        {
            _appApplicationServices.ModifyAppIconSize(CurrentUser.Id, appSize);
            return Json(new { data = 1 });
        }
        /// <summary>
        /// 更改应用图标的垂直间距
        /// </summary>
        /// <param name="appVertical"></param>
        /// <returns></returns>
        public ActionResult ChangeAppVertical(Int32 appVertical)
        {
            _appApplicationServices.ModifyAppVerticalSpacing(CurrentUser.Id, appVertical);

            return Json(new { data = 1 });
        }

        /// <summary>
        /// 修改图标的水平间距
        /// </summary>
        /// <param name="appHorizontal"></param>
        /// <returns></returns>
        public ActionResult ChangeAppHorizontal(Int32 appHorizontal)
        {
            _appApplicationServices.ModifyAppHorizontalSpacing(CurrentUser.Id, appHorizontal);

            return Json(new { data = 1 });
        }

        /// <summary>
        /// 更改码头的位置
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="deskNum"></param>
        /// <returns></returns>
        public ActionResult ChangeDockPosition(String pos = "", Int32 deskNum = 0)
        {
            _deskApplicationServices.ModifyDockPosition(CurrentUser.Id, deskNum, pos);

            return Json(new { data = 1 });
        }
        /// <summary>
        /// 桌面元素移动
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberMove(String moveType, Int32 memberId = 0, Int32 from = 0, Int32 to = 0)
        {
            switch (moveType)
            {
                case "desk-dock": //成员从桌面移动到码头
                    _deskApplicationServices.MemberInDock(CurrentUser.Id, memberId);
                    break;
                case "dock-desk": //成员从码头移动到桌面
                    _deskApplicationServices.MemberOutDock(CurrentUser.Id, memberId,to);
                    break;
                case "dock-folder": //成员从码头移动到桌面文件夹中
                    _deskApplicationServices.DockToFolder(CurrentUser.Id, memberId, to);
                    break;
                case "folder-dock": //成员从文件夹移动到码头
                    _deskApplicationServices.FolderToDock(CurrentUser.Id, memberId);
                    break;
                case "desk-folder": //成员从桌面移动到文件夹
                    _deskApplicationServices.DeskToFolder(CurrentUser.Id, memberId, to);
                    break;
                case "folder-desk": //成员从文件夹移动到桌面
                    _deskApplicationServices.FolderToDesk(CurrentUser.Id, memberId);
                    break;
                case "folder-folder": //成员从文件夹移动到另一个文件夹中
                    _deskApplicationServices.FolderToOtherFolder(CurrentUser.Id, memberId, to);
                    break;
                case "desk-desk":
                    _deskApplicationServices.DeskToOtherDesk(CurrentUser.Id, memberId, to);
                    break;
            }
            return new EmptyResult();
        }
    }
}