using System;
using System.Web;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;
using NewCRM.Dto.Dto;
using NewCRM.Web.Controllers.ControllerHelper;
using Newtonsoft.Json;
using NewCRM.Infrastructure.CommonTools;
using System.Collections;
using System.Collections.Generic;

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

		/// <summary>
		/// 首页
		/// </summary>
		/// <returns></returns>
		public ActionResult Desktop(Int32 accountId)
		{
			ViewBag.Title = "桌面";

			if (Request.Cookies["Account"] != null)
			{
				ViewData["Account"] = JsonConvert.DeserializeObject<AccountDto>(Request.Cookies["Account"].Value);
				ViewData["AccountConfig"] = _accountApplicationServices.GetConfig(accountId);
				ViewData["Desks"] = _accountApplicationServices.GetConfig(accountId).DefaultDeskCount;

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
            var skinName = _accountApplicationServices.GetConfig(accountId).Skin;
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
            var result = _accountApplicationServices.GetConfig(accountId);
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
            var result = _accountApplicationServices.GetConfig(accountId).DockPosition;
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
            var result = _appApplicationServices.GetDeskMembers(accountId);
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
            var result = _accountApplicationServices.GetConfig(accountId).AccountFace;
            response.IsSuccess = true;
            response.Message = "获取用户头像成功";
            response.Model = result;

            return Json(response, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// 创建一个窗口
		/// </summary>
		/// <param name="id"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public ActionResult CreateWindow(Int32 accountId, Int32 id = 0, String type = "")
		{
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel<dynamic>();
            var internalMemberResult = type == "folder" ? _deskApplicationServices.GetMember(accountId, id, true)
				 : _deskApplicationServices.GetMember(0, id);
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