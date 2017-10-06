using System;
using System.Web;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomHelper;
using NewCRM.Web.Controllers.ControllerHelper;
using Newtonsoft.Json;

namespace NewCRM.Web.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IAccountApplicationServices _accountServices;

        public LoginController(IAccountApplicationServices accountServices)
        {
            _accountServices = accountServices;
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

        #endregion

        /// <summary>
        /// 登陆
        /// </summary>
        /// <returns></returns>
        public ActionResult Landing(String accountName, String passWord, Boolean isRememberPasswrod)
        {
            var response = new ResponseModel<AccountDto>();

            #region 参数验证
            Parameter.Validate(accountName).Validate(passWord);
            #endregion

            var account = _accountServices.Login(accountName, passWord);
            if (account != null)
            {
                response.Message = "登陆成功";
                response.Model = account;
                response.IsSuccess = true;

                Response.SetCookie(new HttpCookie("Account")
                {
                    Name = account.Id.ToString(),
                    Value = JsonConvert.SerializeObject(account),
                    Expires = isRememberPasswrod ? DateTime.Now.AddDays(7) : DateTime.Now.AddMinutes(30)
                });
            }

            return Json(response);
        }
    }
}