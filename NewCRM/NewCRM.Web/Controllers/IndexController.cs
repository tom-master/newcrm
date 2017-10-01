using System;
using System.ComponentModel.Composition;
using System.Web;
using System.Web.Mvc;
using NewCRM.Application.Interface;
using NewCRM.Dto.Dto;
using NewCRM.Web.Controllers.ControllerHelper;
using Newtonsoft.Json;

namespace NewCRM.Web.Controllers
{
    public class IndexController : BaseController
    {

        private readonly IAccountApplicationServices _accountApplicationServices;

        private readonly IAppApplicationServices _appApplicationServices;

        private readonly IDeskApplicationServices _deskApplicationServices;

        
        public IndexController(IAccountApplicationServices accountApplicationServices,
            IAppApplicationServices appApplicationServices,
            IDeskApplicationServices deskApplicationServices)
        {

            _accountApplicationServices = accountApplicationServices;

            _appApplicationServices = appApplicationServices;

            _deskApplicationServices = deskApplicationServices;
        }


        #region 页面

        // GET: Index
        /// <summary>
        /// 桌面
        /// </summary>
        /// <returns></returns>
        public ActionResult Desktop()
        {
            ViewBag.Title = "桌面";
            
            if (Request.Cookies["Account"] != null)
            {
                ViewData["Account"] = JsonConvert.DeserializeObject<AccountDto>(Request.Cookies["Account"].Value);

                ViewData["AccountConfig"] = _accountApplicationServices.GetConfig(0);

                ViewData["Desks"] = _accountApplicationServices.GetConfig(0).DefaultDeskCount;

                return View();
            }

            return RedirectToAction("Index", "Login");
        }

        #endregion

        /// <summary>
        /// 账户登出
        /// </summary>
        public void Logout()
        {
            Response.Cookies.Add(new HttpCookie("Account")
            {
                Expires = DateTime.Now.AddDays(-1)
            });
        }

        /// <summary>
        /// 初始化皮肤
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSkin()
        {
            var skinName = _accountApplicationServices.GetConfig(0).Skin;

            return Json(new { data = skinName }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 初始化壁纸
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWallpaper()
        {
            var config = _accountApplicationServices.GetConfig(0);

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

        /// <summary>
        /// 初始化应用码头
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDockPos()
        {
            var dockPos = _accountApplicationServices.GetConfig(0).DockPosition;

            return Json(new { data = dockPos }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取我的应用
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAccountDeskMembers()
        {
            var app = _appApplicationServices.GetDeskMembers(0);

            return Json(new { app }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAccountFace()
        {
            return Json(new { data = _accountApplicationServices.GetConfig(0).AccountFace }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 创建一个窗口
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public ActionResult CreateWindow(Int32 id = 0, String type = "")
        {

            var internalMemberResult = type == "folder" ? _deskApplicationServices.GetMember(0, id, true)
                 : _deskApplicationServices.GetMember(0, id);

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
            });
        }
    }
}