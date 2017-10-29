using NewCRM.Application.Services.Interface;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace NewCRM.Web.Controllers
{
    public class IndexController : BaseController
    {

        private readonly IAccountServices _accountServices;

        private readonly IAppServices _appServices;

        private readonly IDeskServices _deskServices;

        public IndexController(IAccountServices accountServices, IAppServices appServices, IDeskServices deskServices)
        {
            _accountServices = accountServices;
            _appServices = appServices;
            _deskServices = deskServices;
        }


        #region 页面

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Desktop()
        {
            ViewBag.Title = "桌面";

            if (Request.Cookies["Account"] != null)
            {
                
                ViewData["Account"] = Account;
                ViewData["AccountConfig"] = _accountServices.GetConfig(Account.Id);
                ViewData["Desks"] = _accountServices.GetConfig(Account.Id).DefaultDeskCount;

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
        public ActionResult GetSkin(Int32 accountId)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel<String>();
            var skinName = _accountServices.GetConfig(accountId).Skin;
            response.IsSuccess = true;
            response.Model = skinName;
            response.Message = "初始化皮肤成功";

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 初始化壁纸
        /// </summary>
        /// <returns></returns>
        public ActionResult GetWallpaper(Int32 accountId)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel<ConfigDto>();
            var result = _accountServices.GetConfig(accountId);
            response.IsSuccess = true;
            response.Message = "初始化壁纸成功";
            response.Model = result;

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 初始化应用码头
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDockPos(Int32 accountId)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel<String>();
            var result = _accountServices.GetConfig(accountId).DockPosition;
            response.IsSuccess = true;
            response.Message = "初始化应用码头成功";
            response.Model = result;

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取我的应用
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAccountDeskMembers(Int32 accountId)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel<IDictionary<String, IList<dynamic>>>();
            var result = _appServices.GetDeskMembers(accountId);
            response.IsSuccess = true;
            response.Message = "获取我的应用成功";
            response.Model = result;

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAccountFace(Int32 accountId)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel<String>();
            var result = _accountServices.GetConfig(accountId).AccountFace;
            response.IsSuccess = true;
            response.Message = "获取用户头像成功";
            response.Model = result;

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 创建一个窗口
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateWindow(Int32 accountId, Int32 id, String type)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel<dynamic>();
            var internalMemberResult = type == "folder" ? _deskServices.GetMember(accountId, id, true) : _deskServices.GetMember(accountId, id);
            response.IsSuccess = true;
            response.Message = "创建一个窗口成功";
            response.Model = new
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
            };

            return Json(response);
        }
    }
}