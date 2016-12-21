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
    public class AccountController : BaseController
    {
        #region 页面
        public ActionResult AccountManage()
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

        public ActionResult CheckAccountNameExist(String param)
        {
            var result = AccountApplicationServices.CheckAccountNameExist(param);

            return Json(result ? new { status = "y", info = "" } : new { status = "n", info = "用户名已存在" });
        }

        #region private method

        private static AccountDto WapperAccountDto(FormCollection forms)
        {
            Int32 accountId = 0;
            if ((forms["accountId"] + "").Length > 0)
            {
                accountId = Int32.Parse(forms["accountId"]);
            }

            List<RoleDto> roleIds;

            if ((forms["val_roleIds"] + "").Length > 0)
            {
                roleIds = forms["val_roleIds"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(role => new RoleDto
                {
                    Id = Int32.Parse(role)
                }).ToList();
            }
            else
            {
                throw new BusinessException("所选的角色列表不能为空");
            }

            return new AccountDto
            {
                Id = accountId,
                Name = forms["val_accountname"],
                Password = forms["val_password"],
                AccountType = forms["val_type"],
                Roles = roleIds
            };
        }

        #endregion
    }
}