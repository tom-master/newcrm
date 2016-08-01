using NewCRM.Web.Controllers.ControllerHelper;
using System;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Dto.Dto;

namespace NewCRM.Web.Controllers
{
    [Export]
    public class IndexController : BaseController
    {
        [Import]
        private IAccountApplicationServices _accountApplicationServices;

        [Import]
        private IAppApplicationServices _appApplicationServices;

        [Import]
        private IDeskApplicationServices _deskApplicationServices;

        // GET: Index
        /// <summary>
        /// 桌面
        /// </summary>
        /// <returns></returns>
        public ActionResult Desktop()
        {
            if (Request.Cookies["Account"] != null)
            {
                CurrentUser = _accountApplicationServices.GetUserConfig(Int32.Parse(Request.Cookies["Account"].Value));
                ViewData["CurrentUser"] = CurrentUser;
                return View();
            }
            return RedirectToAction("Login", "Index");
        }

        /// <summary>
        /// 登陆页
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <param name="isRememberPasswrod"></param>
        /// <returns></returns>
        public ActionResult Landing(String userName, String passWord, Boolean isRememberPasswrod = false)
        {
            /*var userData = */
            var userResult = _accountApplicationServices.Login(userName, passWord);

            Response.SetCookie(new HttpCookie("Account")
            {
                Value = userResult.Id.ToString(),
                Expires = isRememberPasswrod ? DateTime.Now.AddDays(7) : DateTime.Now.AddMinutes(30)
            });




            CurrentUser = userResult;

            return Json(new { status = 1 });
        }


        /// <summary>
        /// 初始化皮肤
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSkin()
        {
            var skinName = CurrentUser.Skin;
            return Json(new { data = skinName }, JsonRequestBehavior.AllowGet);
        }

        ///// <summary>
        ///// 初始化壁纸
        ///// </summary>
        ///// <returns></returns>
        public ActionResult GetWallpaper()
        {
            var config = _accountApplicationServices.GetUserConfig(CurrentUser.Id);
            return Json(new
            {
                data = new
                {
                    config.WallpaperSource,
                    config.WallpaperUrl,
                    config.WallpaperHeigth,
                    config.WallpaperMode,
                    config.WallpaperWidth
                }
            }, JsonRequestBehavior.AllowGet);
        }

        ///// <summary>
        ///// 初始化应用码头
        ///// </summary>
        ///// <returns></returns>
        public ActionResult GetDockPos()
        {
            var dockPos = CurrentUser.DockPosition;
            return Json(new { data = dockPos }, JsonRequestBehavior.AllowGet);
        }

        ///// <summary>
        ///// 获取我的应用
        ///// </summary>
        ///// <returns></returns>
        public ActionResult GetMyApp()
        {
            var app = _appApplicationServices.GetUserApp(CurrentUser.Id);
            return Json(new { app }, JsonRequestBehavior.AllowGet);
        }

        ///// <summary>
        ///// 获取用户头像
        ///// </summary>
        ///// <returns></returns>
        public ActionResult GetUserFace()
        {
            return Json(new { data = CurrentUser.UserFace }, JsonRequestBehavior.AllowGet);
        }

        ///// <summary>
        ///// 创建一个窗口
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="type"></param>
        ///// <returns></returns>
        public ActionResult CreateWindow(Int32 id = 0, String type = "")
        {
            var internalMemberResult = type == "folder" ? _deskApplicationServices.GetMember(CurrentUser.Id, id, true) : _deskApplicationServices.GetMember(CurrentUser.Id, id);

            return Json(new
            {
                data = new
                {
                    type = internalMemberResult.MemberType.ToLower(),
                    memberId = internalMemberResult.Id,
                    appId = internalMemberResult.AppId,
                    name = internalMemberResult.Name,
                    icon = internalMemberResult.IconUrl,
                    width = internalMemberResult.Width,
                    height = internalMemberResult.Height,
                    isOnDock = internalMemberResult.IsOnDock,
                    isDraw = internalMemberResult.IsDraw,
                    isOpenMax = internalMemberResult.IsOpenMax,
                    isSetbar = internalMemberResult.IsSetbar,
                    url = internalMemberResult.AppUrl

                }
            }, JsonRequestBehavior.AllowGet);
        }
    }
}