﻿using NewCRM.Application.Services.Interface;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NewCRM.Web.Controllers
{
    public class AccountManagerController : BaseController
    {
        private readonly ISecurityServices _securityServices;

        public AccountManagerController(ISecurityServices securityServices)
        {
            _securityServices = securityServices;
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
                ViewData["Account"] = AccountServices.GetAccount(accountId);
            }

            ViewData["Roles"] = _securityServices.GetAllRoles();

            return View();
        }

        #endregion

        /// <summary>
        /// 获取所有账户
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllAccounts(String accountName, String accountType, Int32 pageIndex, Int32 pageSize)
        {
            var response = new ResponseModels<IList<AccountDto>>();

            #region 参数验证
            Parameter.Validate(accountName).Validate(accountType);
            #endregion

            Int32 totalCount;
            var accounts = AccountServices.GetAccounts(accountName, accountType, pageIndex, pageSize, out totalCount);
            if (accounts != null)
            {
                response.TotalCount = totalCount;
                response.Message = "获取账户列表成功";
                response.Model = accounts;
                response.IsSuccess = true;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 创建新账户
        /// </summary>
        /// <returns></returns>
        public ActionResult NewAccount(FormCollection forms)
        {
            var response = new ResponseModel<AccountDto>();
            var dto = WapperAccountDto(forms);
            if (dto.Id == 0)
            {
                AccountServices.AddNewAccount(dto);

                response.Message = "创建新账户成功";
                response.IsSuccess = true;
            }
            else
            {
                AccountServices.ModifyAccount(dto);

                response.Message = "修改账户成功";
                response.IsSuccess = true;
            }


            return Json(response);
        }

        /// <summary>
        /// 检查账户名是否已经存在
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckAccountNameExist(String param)
        {
            var response = new ResponseModel<dynamic>();
            var result = AccountServices.CheckAccountNameExist(param);
            response.IsSuccess = true;
            response.Model = result ? new { status = "y", info = "" } : new { status = "n", info = "用户名已存在" };

            return Json(response);
        }

        /// <summary>
        /// 移除账户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult RemoveAccount(Int32 accountId)
        {
            var response = new ResponseModel<String>();
            AccountServices.RemoveAccount(accountId);
            response.IsSuccess = true;
            response.Message = "移除账户成功";

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改账户为禁用状态
        /// </summary>
        /// <returns></returns>
        public ActionResult ChangeAccountDisableStatus(String isDisable)
        {
            var response = new ResponseModel<String>();
            if (Boolean.Parse(isDisable))
            {
                AccountServices.Disable(Account.Id);

                response.IsSuccess = true;
                response.Message = "禁用账户成功";
            }
            else
            {
                AccountServices.Enable(Account.Id);

                response.IsSuccess = true;
                response.Message = "启用账户成功";
            }


            return Json(response);
        }

        #region private method

        private AccountDto WapperAccountDto(FormCollection forms)
        {
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
                Id = Account.Id,
                Name = forms["val_accountname"],
                Password = forms["val_password"],
                IsAdmin = Int32.Parse(forms["val_type"]) == 1,
                Roles = roleIds
            };
        }

        #endregion
    }
}