using NewCRM.Application.Services.Interface;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;

namespace NewCRM.Web.Controllers
{
    public class LoginController : BaseController
    {
        private readonly IAccountServices _accountServices;

        public LoginController(IAccountServices accountServices)
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
        [HttpGet]
        public ActionResult Landing(String accountName, String passWord, Boolean isRememberPasswrod = default(Boolean))
        {
            var response = new ResponseModel<AccountDto>();

            #region 参数验证
            Parameter.Validate(accountName).Validate(passWord);
            #endregion

            var account = _accountServices.Login(accountName, passWord);
            if (account != null)
            {
                response.Message = "登陆成功";
                response.IsSuccess = true;

                Response.Cookies.Add(new HttpCookie("Account")
                {
                    Value = JsonConvert.SerializeObject(account),
                    Expires = isRememberPasswrod ? DateTime.Now.AddDays(7) : DateTime.Now.AddMinutes(30)
                });
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 解锁屏幕
        /// </summary>
        /// <returns></returns>
        public ActionResult UnlockScreen(String unlockPassword)
        {
            #region 参数验证
            Parameter.Validate(unlockPassword);
            #endregion


        }
    }
}