using NewCRM.Web.Controllers.ControllerHelper;
using System;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using NewCRM.Application.Services.IApplicationService;

namespace NewCRM.Web.Controllers
{
    [Export]
    public class IndexController : BaseController
    {
        [Import]
        private IAccountApplicationServices _accountApplicationServices;

        // GET: Index
        /// <summary>
        /// 桌面
        /// </summary>
        /// <returns></returns>
        public ActionResult Desktop()
        {
            ViewData["CurrentUser"] = new object(); // CurrentUser;
            return View();
        }

        /// <summary>
        /// 登陆页
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }

        //#region 登陆

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
            _accountApplicationServices.Login(userName, passWord);
            //if (userData.ResultType != ResponseType.Success)
            //{
            //    return Json(new { status = 0, msg = userData.Message });
            //}

            //var cookie = new HttpCookie("UserInfo")
            //{
            //    ["UserId"] = userData.Data.Id.ToString(CultureInfo.InvariantCulture),
            //    Expires = isRememberPasswrod ? DateTime.Now.AddDays(7) : DateTime.Now.AddHours(1)
            //};
            //HttpContext.Response.Cookies.Add(cookie);

            //CurrentUser = userData.Data;

            return Json(new { status = 1 });
        }
        //#endregion

        //#region 桌面相关的
        ///// <summary>
        ///// 初始化皮肤
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult GetSkin()
        //{
        //    var skinName = CurrentUser.UserConfigure.Skin;
        //    return Json(new { data = skinName }, JsonRequestBehavior.AllowGet);
        //}
        /////// <summary>
        /////// 初始化壁纸
        /////// </summary>
        /////// <returns></returns>
        //public ActionResult GetWallpaper()
        //{
        //    var userWallpaper = CurrentUser.UserConfigure;
        //    return Json(new { data = userWallpaper }, JsonRequestBehavior.AllowGet);
        //}
        /////// <summary>
        /////// 初始化应用码头
        /////// </summary>
        /////// <returns></returns>
        //public ActionResult GetDockPos()
        //{
        //    var dockPos = CurrentUser.UserConfigure.DockPosition;
        //    return Json(new { data = dockPos }, JsonRequestBehavior.AllowGet);
        //}
        /////// <summary>
        /////// 获取我的应用
        /////// </summary>
        /////// <returns></returns>
        //public ActionResult GetMyApp()
        //{
        //    return Json(new { app = PlantformApplicationService.UserApp(CurrentUser.Id) }, JsonRequestBehavior.AllowGet);
        //}
        /////// <summary>
        /////// 获取用户头像
        /////// </summary>
        /////// <returns></returns>
        //public ActionResult GetUserFace()
        //{
        //    return Json(new { data = CurrentUser.UserConfigure.UserFace }, JsonRequestBehavior.AllowGet);
        //}
        /////// <summary>
        /////// 创建一个窗口
        /////// </summary>
        /////// <param name="id"></param>
        /////// <param name="type"></param>
        /////// <returns></returns>
        //public ActionResult CreateWindow(Int32 id = 0, String type = "")
        //{
        //    //_userApplicationService = new UserApplicationService();
        //    //var value = _userApplicationService.BuilderWindow(id, type);
        //    //return Json(new { data = value }, JsonRequestBehavior.AllowGet);
        //    return null;
        //}
        //#endregion
    }
}