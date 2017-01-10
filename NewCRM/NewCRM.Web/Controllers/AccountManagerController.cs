using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    [Export]
    public class AccountManagerController : BaseController
    {
        #region 页面
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateNewAccount(Int32 accountId = 0)
        {
            if (accountId != 0)
            {
                ViewData["Account"] = AccountApplicationServices.GetAccount(accountId);
            }

            ViewData["Roles"] = SecurityApplicationServices.GetAllRoles();

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
            Int32 totalCount;

            var accounts = AccountApplicationServices.GetAccounts(accountName, accountType, pageIndex, pageSize, out totalCount);

            return Json(new
            {
                accounts,
                totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 创建新账户
        /// </summary>
        /// <param name="forms"></param>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public ActionResult NewAccount(FormCollection forms, Int32 accountId)
        {
            var accountDto = WapperAccountDto(forms);

            if (accountId == 0)
            {
                AccountApplicationServices.AddNewAccount(accountDto);
            }
            else
            {
                AccountApplicationServices.ModifyAccount(accountDto);
            }

            return Json(new { success = 1 });
        }

        /// <summary>
        /// 检查账户名是否已经存在
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult CheckAccountNameExist(String param)
        {
            var result = AccountApplicationServices.CheckAccountNameExist(param);

            return Json(result ? new { status = "y", info = "" } : new { status = "n", info = "用户名已存在" });
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
                IsAdmin = Int32.Parse(forms["val_type"])==1,
                Roles = roleIds
            };
        }

        #endregion
    }
}