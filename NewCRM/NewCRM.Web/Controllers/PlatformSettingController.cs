using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using NewCRM.ApplicationService;
using NewCRM.ApplicationService.IApplicationService;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    public class PlatformSettingController : BaseController
    {

        private IPlantformApplicationService _plantformApplicationService;

        private IUserApplicationService _userApplicationService;

        //// GET: PlatformSetting
        public ActionResult SystemWallPaper()
        {
            return View();
        }

        public ActionResult CustomWallPaper()
        {
            return View();
        }

        public ActionResult SettingSkin()
        {
            return View();
        }

        public ActionResult DeskTopSetting()
        {
            _userApplicationService = new UserApplicationService();
            ViewData["UserData"] = _userApplicationService.GetUser(UserEntity.Id);
            return View();
        }

        //获取全部的系统壁纸
        public ActionResult GetAllWallpaper()
        {
            _plantformApplicationService = new PlantformApplicationService();
            return Json(new { data = _plantformApplicationService.GetWallpapers() }, JsonRequestBehavior.AllowGet);
        }
        //设置背景壁纸
        public ActionResult SetWallpaper(String wallPaperShowType = "", Int32 wallpaperId = 0)
        {
            _plantformApplicationService = new PlantformApplicationService();
            _plantformApplicationService.SetWallPaper(wallpaperId, wallPaperShowType, UserEntity.Id);
            return null;
        }
        //载入用户之前上传的壁纸
        public ActionResult GetAllUploadWallPaper()
        {
            _plantformApplicationService = new PlantformApplicationService();
            var wallPapers = _plantformApplicationService.GetUserUploadWallPaper(UserEntity.Id);
            return Json(new { data = wallPapers }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除上传的壁纸
        /// </summary>
        /// <param name="wallPaperId"></param>
        /// <returns></returns>
        public ActionResult DeleteWallPaper(Int32 wallPaperId = 0)
        {
            _userApplicationService = new UserApplicationService();
            var deleteResult = _userApplicationService.DeleteWallPaper(wallPaperId, UserEntity.Id);
            return Json(deleteResult ? new { data = 1 } : new { data = 0 });
        }
        /// <summary>
        /// 上传壁纸     
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadWallPaper()
        {
            _userApplicationService = new UserApplicationService();
            var httpPostedFile = HttpContext.Request.Files[0];
            var savePath = ConfigurationManager.AppSettings["UploadWallPaperPath"];
            FileUpLoadHelper fileUpLoad = new FileUpLoadHelper(savePath, false, true, true, true, 160, 115,
                ThumbnailMode.AUTO, false, "");

            if (fileUpLoad.SaveFile(httpPostedFile))
            {
                var result = _userApplicationService.UploadWallPaper(UserEntity.Id, fileUpLoad);

                return Json(new { Id = result.Id, Url = result.SmallUrl }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { value = 0 }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 设置网络图片作为壁纸
        /// </summary>
        /// <param name="webUrl"></param>
        /// <returns></returns>
        public ActionResult SetWebWallPaper(String webUrl = "")
        {
            if ((webUrl + "").Length <= 0)
            {
                return null;
            }
            _userApplicationService = new UserApplicationService();
            var result = _userApplicationService.SetWebWallPaper(webUrl);
            return Json(result != 0 ? new { Id = result } : new { Id = 0 });
        }

        /// <summary>
        /// 获取全部的皮肤
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllSkin()
        {
            _plantformApplicationService = new PlantformApplicationService();
            var skinFullPath = Server.MapPath(ConfigurationManager.AppSettings["PlantFormSkinPath"]);
            var data = _plantformApplicationService.GetAllSkin(skinFullPath);
            return Json(data.Any() ? new { data = data, currentSkin = UserEntity.Skin } : new { data = data, currentSkin = "" }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 更换皮肤
        /// </summary>
        /// <param name="skin"></param>
        /// <returns></returns>
        public ActionResult ChangeSkin(String skin = "")
        {
            _plantformApplicationService = new PlantformApplicationService();
            var data = _plantformApplicationService.UpdateSkin(skin, UserEntity.Id);
            return Json(data ? new { data = 1 } : new { data = 0 }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 更换默认显示的桌面
        /// </summary>
        /// <param name="deskNum"></param>
        /// <returns></returns>
        public ActionResult ChangeDefaultDesk(Int32 deskNum = 1)
        {
            _plantformApplicationService = new PlantformApplicationService();
            var result = _plantformApplicationService.UpdateDefaultDesk(deskNum, UserEntity.Id);
            return Json(result ? new { data = 1 } : new { data = 0 }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 更换图标的排列方向
        /// </summary>
        /// <param name="appXy"></param>
        /// <returns></returns>
        public ActionResult ChangeAppXy(String appXy = "")
        {
            _plantformApplicationService = new PlantformApplicationService();
            var result = _plantformApplicationService.UpdateAppXy(appXy, UserEntity.Id);
            return Json(result ? new { data = 1 } : new { data = 0 }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 更改图标大小
        /// </summary>
        /// <param name="appSize"></param>
        /// <returns></returns>
        public ActionResult ChangeAppSize(String appSize = "")
        {
            _plantformApplicationService = new PlantformApplicationService();
            var result = _plantformApplicationService.UpdateAppSize(appSize, UserEntity.Id);
            return Json(result ? new { data = 1 } : new { data = 0 }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 更改码头的位置
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="deskNum"></param>
        /// <returns></returns>
        public ActionResult ChangeDockPosition(String pos = "", Int32 deskNum = 0)
        {
            _plantformApplicationService = new PlantformApplicationService();
            var data = _plantformApplicationService.UpdateDockPosition(pos, deskNum, UserEntity.Id);
            return Json(data ? new { data = 1 } : new { data = 0 });
        }
        ///// <summary>
        ///// 桌面元素移动
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult ElementMove(String moveType = "", Int32 id = 0, Int32 from = 0, Int32 to = 0)
        //{
        //    switch (moveType)
        //    {
        //        case "dock-folder":
        //            break;
        //    }
        //    return null;
        //}
    }
}