using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
	public class AccountManagerController : BaseController
	{

		private readonly IAccountApplicationServices _accountApplicationServices;
		private readonly ISecurityApplicationServices _securityApplicationServices;

		public AccountManagerController(IAccountApplicationServices accountApplicationServices
			, ISecurityApplicationServices securityApplicationServices)
		{
			_accountApplicationServices = accountApplicationServices;

			_securityApplicationServices = securityApplicationServices;
		}

		#region 页面
		/// <summary>
		/// 首页
		/// </summary>
		/// <returns></returns>
		public ActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// 创建新账户
		/// </summary>
		/// <param name="accountId"></param>
		/// <returns></returns>
		public ActionResult CreateNewAccount(Int32 accountId = 0)
		{
			if (accountId != 0)
			{
				ViewData["Account"] = _accountApplicationServices.GetAccount(accountId);
			}

			ViewData["Roles"] = _securityApplicationServices.GetAllRoles();

			return View();
		}

		#endregion

		/// <summary>
		/// 获取所有账户
		/// </summary>
		/// <param name="accountName"></param>
		/// <param name="accountType"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public ActionResult GetAllAccounts(String accountName, String accountType, Int32 pageIndex, Int32 pageSize)
		{
			var response = new ResponseModels<IList<AccountDto>>();

			#region 参数验证
			Parameter.Validate(accountName).Validate(accountType);
			#endregion

			Int32 totalCount;
			var accounts = _accountApplicationServices.GetAccounts(accountName, accountType, pageIndex, pageSize, out totalCount);
			if (accounts != null)
			{
				response.TotalCount = totalCount;
				response.Message = "获取账户列表成功";
				response.Model = accounts;
				response.IsSuccess = true;
			}

			return Json(accounts, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// 创建新账户
		/// </summary>
		/// <param name="forms"></param>
		/// <param name="accountId"></param>
		/// <returns></returns>
		public ActionResult NewAccount(FormCollection forms, Int32 accountId)
		{
			var response = new ResponseModel<AccountDto>();
			var dto = WapperAccountDto(forms);
			if (accountId == 0)
			{
				_accountApplicationServices.AddNewAccount(dto);
			}
			else
			{
				_accountApplicationServices.ModifyAccount(dto);
			}
			response.Message = "创建新账户成功";
			response.IsSuccess = true;

			return Json(response);
		}

		/// <summary>
		/// 检查账户名是否已经存在
		/// </summary>
		/// <param name="param"></param>
		/// <returns></returns>
		public ActionResult CheckAccountNameExist(String param)
		{
			var response = new ResponseModel<dynamic>();
			var result = _accountApplicationServices.CheckAccountNameExist(param);
			response.IsSuccess = true;
			response.Model = result ? new { status = "y", info = "" } : new { status = "n", info = "用户名已存在" };

			return Json(response);
		}

		/// <summary>
		/// 移除账户
		/// </summary>
		/// <param name="accountId"></param>
		/// <returns></returns>
		public ActionResult RemoveAccount(Int32 accountId)
		{
			var response = new ResponseModel<String>();
			_accountApplicationServices.RemoveAccount(accountId);
			response.IsSuccess = true;
			response.Message = "移除账户成功";

			return Json(response);
		}

		/// <summary>
		/// 修改账户为禁用状态
		/// </summary>
		/// <param name="accountId"></param>
		/// <param name="isDisable"></param>
		/// <returns></returns>
		public ActionResult ChangeAccountDisableStatus(Int32 accountId, String isDisable)
		{
			var response = new ResponseModel<String>();
			if (Boolean.Parse(isDisable))
			{
				_accountApplicationServices.Disable(accountId);
			}
			else
			{
				_accountApplicationServices.Enable(accountId);
			}
			response.IsSuccess = true;
			response.Message = "账户状态修改成功";

			return Json(response);
		}

		#region private method

		private static AccountDto WapperAccountDto(FormCollection forms)
		{
			Int32 accountId = 0;
			if ((forms["id"] + "").Length > 0)
			{
				accountId = Int32.Parse(forms["id"]);
			}

			List<RoleDto> roleIds = new List<RoleDto>();

			if ((forms["val_roleIds"] + "").Length > 0)
			{
				roleIds = forms["val_roleIds"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(role => new RoleDto
				{
					Id = Int32.Parse(role)
				}).ToList();
			}

			return new AccountDto
			{
				Id = accountId,
				Name = forms["val_accountname"],
				Password = forms["val_password"],
				IsAdmin = Int32.Parse(forms["val_type"]) == 1,
				Roles = roleIds
			};
		}

		#endregion
	}
}