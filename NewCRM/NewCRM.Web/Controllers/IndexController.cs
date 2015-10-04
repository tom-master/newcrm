using NewCRM.ApplicationService;
using NewCRM.ApplicationService.IApplicationService;
using NewCRM.Web.Controllers.ControllerHelper;
using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;

namespace NewCRM.Web.Controllers
{
    public class IndexController : BaseController
    {

        private IUserApplicationService _userApplicationService;

        // GET: Index
        /// <summary>
        /// 桌面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Desktop()
        {
            _userApplicationService = new UserApplicationService();
            ViewData["userEntity"] = _userApplicationService.GetUser(UserEntity.Id);
            return View();
        }
        [HttpGet] 
        public ActionResult Login()
        {
            return View();
        }
        #region 登陆

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="passWord"></param>
        /// <param name="isRememberPasswrod"></param>
        /// <returns></returns>
        public ActionResult Landing(String userName = "", String passWord = "", Boolean isRememberPasswrod = false)
        {
            _userApplicationService = new UserApplicationService();

            var userData = _userApplicationService.UserLogin(userName, passWord);

            var cookie = new HttpCookie("UserInfo");
            cookie["UserId"] = userData.Id.ToString(CultureInfo.InvariantCulture);
            cookie.Expires = isRememberPasswrod ? DateTime.Now.AddDays(7) : DateTime.Now.AddHours(1);
            HttpContext.Response.Cookies.Add(cookie);

            UserEntity = userData;

            return Json(new { status = 1, data = userData });
        }
        #endregion

        #region 桌面相关的
        /// <summary>
        /// 初始化皮肤
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSkin()
        {
            var skinName = UserEntity.Skin;
            return Json(new { data = skinName }, JsonRequestBehavior.AllowGet);
        }
        ///// <summary>
        ///// 初始化壁纸
        ///// </summary>
        ///// <returns></returns>
        public ActionResult GetWallpaper()
        {
            _userApplicationService = new UserApplicationService();
            var userWallpaper = _userApplicationService.GetUserWallPaper(UserEntity.Id);
            return Json(new { data = userWallpaper }, JsonRequestBehavior.AllowGet);
        }
        ///// <summary>
        ///// 初始化应用码头
        ///// </summary>
        ///// <returns></returns>
        public ActionResult GetDockPos()
        {
            var dockPos = UserEntity.DockPosition;
            return Json(new { data = dockPos }, JsonRequestBehavior.AllowGet);
        }
        ///// <summary>
        ///// 获取我的应用
        ///// </summary>
        ///// <returns></returns>
        public ActionResult GetMyApp()
        {
            _userApplicationService = new UserApplicationService();
            return Json(new { data = _userApplicationService.GetUserApp(UserEntity) }, JsonRequestBehavior.AllowGet);
        }
        ///// <summary>
        ///// 获取用户头像
        ///// </summary>
        ///// <returns></returns>
        public ActionResult GetUserFace()
        {
            return Json(new { data = UserEntity.UserFace }, JsonRequestBehavior.AllowGet);
        }
        ///// <summary>
        ///// 创建一个窗口
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="type"></param>
        ///// <returns></returns>
        public ActionResult CreateWindow(Int32 id = 0, String type = "")
        {
            _userApplicationService = new UserApplicationService();
            var value = _userApplicationService.BuilderWindow(id, type);
            return Json(new { data = value }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}