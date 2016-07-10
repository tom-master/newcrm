using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Web.Mvc;
using NewCRM.Application.Services;
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
        private IUserSettingApplicationServices _userSettingApplicationServices;

        // //// GET: PlatformSetting
        public ActionResult SystemWallPaper()
        {
            ViewData["CurrentUser"] = CurrentUser;
            ViewData["Wallpapers"] = _userSettingApplicationServices.GetWallpaper();
            return View();
        }

        public ActionResult CustomWallPaper()
        {
            ViewData["CurrentUser"] = CurrentUser;
            return View();
        }

        // public ActionResult SettingSkin()
        // {
        //     return View();
        // }

        public ActionResult DeskTopSetting()
        {
            ViewData["CurrentUser"] = CurrentUser;
            return View();
        }

        //设置壁纸显示模式
        public ActionResult ModifyWallpaperDisplayModel(String wallPaperShowType = "")
        {
            _userSettingApplicationServices.ModifyWallpaperMode(CurrentUser.UserId, wallPaperShowType);
            return null;
        }
        //设置壁纸
        public ActionResult ModifyWallpaper(Int32 wallpaperId)
        {
            _userSettingApplicationServices.ModifyWallpaper(CurrentUser.UserId, wallpaperId);
            return null;
        }

        //载入用户之前上传的壁纸
        public ActionResult GetAllUploadWallPaper()
        {
            var wallPapers = _userSettingApplicationServices.GetUploadWallpaper(CurrentUser.UserId);
            return Json(new { wallPapers }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除上传的壁纸
        /// </summary>
        /// <param name="wallPaperId"></param>
        /// <returns></returns>
        public ActionResult DeleteWallPaper(Int32 wallPaperId = 0)
        {
            _userSettingApplicationServices.RemoveWallpaper(CurrentUser.UserId, wallPaperId);
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
                FileUpLoadHelper fileUpLoad = new FileUpLoadHelper(ConfigurationManager.AppSettings["UploadWallPaperPath"], false, false, true, true, 160, 115, ThumbnailMode.Auto, false, "");

                if (fileUpLoad.SaveFile(httpPostedFile))
                {
                    var wallpaperResult = _userSettingApplicationServices.AddWallpaper(new WallpaperDto
                    {
                        Heigth = fileUpLoad.FileHeight,
                        Source = WallpaperSource.Upload,
                        Title = fileUpLoad.OldFileName,
                        Url = fileUpLoad.FilePath + fileUpLoad.OldFileName,
                        UserId = CurrentUser.UserId,
                        Width = fileUpLoad.FileWidth
                    });

                    return Json(new { Id = wallpaperResult.Item1, Url = wallpaperResult.Item2 }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { value = 0 }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult WebWallPaper(String webUrl = "")
        {
            //var result = await PlantformApplicationService.WebImgAsync(CurrentUser.Id, webUrl);
            //return Json(result.ResultType == ResponseType.Success ? new { Id = 1 } : new { Id = 0 });
            return null;
        }

        // /// <summary>
        // /// 获取全部的皮肤
        // /// </summary>
        // /// <returns></returns>
        // public ActionResult GetAllSkin()
        // {
        //     var skinPath = Server.MapPath(ConfigurationManager.AppSettings["PlantFormSkinPath"]);
        //     var data = PlantformApplicationService.AllSkin(skinPath);
        //     return Json(data.Data.Any() ? new { data, currentSkin = CurrentUser.UserConfigure.Skin } : new { data, currentSkin = "" }, JsonRequestBehavior.AllowGet);
        // }
        // /// <summary>
        // /// 更换皮肤
        // /// </summary>
        // /// <param name="skin"></param>
        // /// <returns></returns>
        // public ActionResult ChangeSkin(String skin = "")
        // {
        //     var data = PlantformApplicationService.Skin(CurrentUser.Id, skin);
        //     return Json(data.ResultType == ResponseType.Success ? new { data = 1 } : new { data = 0 }, JsonRequestBehavior.AllowGet);
        // }

        /// <summary>
        /// 更换默认显示的桌面
        /// </summary>
        /// <param name="deskNum"></param>
        /// <returns></returns>
        public ActionResult ChangeDefaultDesk(Int32 deskNum = 1)
        {
            _userSettingApplicationServices.ModifyDefaultDeskNumber(CurrentUser.UserId, deskNum);
            return Json(new { data = 1 }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 更换图标的排列方向
        /// </summary>
        /// <param name="appXy"></param>
        /// <returns></returns>
        public ActionResult ChangeAppXy(String appXy)
        {
            _userSettingApplicationServices.ModifyAppDirection(CurrentUser.UserId, appXy);
            return Json(new { data = 1 }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 更改图标大小
        /// </summary>
        /// <param name="appSize"></param>
        /// <returns></returns>
        public ActionResult ChangeAppSize(Int32 appSize)
        {
            _userSettingApplicationServices.ModifyAppIconSize(CurrentUser.UserId, appSize);
            return Json(new { data = 1 });
        }
        /// <summary>
        /// 更改应用图标的垂直间距
        /// </summary>
        /// <param name="appVertical"></param>
        /// <returns></returns>
        public ActionResult ChangeAppVertical(Int32 appVertical)
        {
            _userSettingApplicationServices.ModifyAppVerticalSpacing(CurrentUser.UserId, appVertical);

            return Json(new { data = 1 });
        }

        /// <summary>
        /// 修改图标的水平间距
        /// </summary>
        /// <param name="appHorizontal"></param>
        /// <returns></returns>
        public ActionResult ChangeAppHorizontal(Int32 appHorizontal)
        {
            _userSettingApplicationServices.ModifyAppHorizontalSpacing(CurrentUser.UserId, appHorizontal);

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
            _userSettingApplicationServices.ModifyDockPosition(CurrentUser.UserId, deskNum, pos);

            return Json(new { data = 1 });
        }
        // ///// <summary>
        // ///// 桌面元素移动
        // ///// </summary>
        // ///// <returns></returns>
        // //public ActionResult ElementMove(String moveType = "", Int32 id = 0, Int32 from = 0, Int32 to = 0)
        // //{
        // //    switch (moveType)
        // //    {
        // //        case "dock-folder":
        // //            break;
        // //    }
        // //    return null;
        // //}
    }
}