using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewCRM.Application.Interface;
using NewCRM.Web.Controllers.ControllerHelper;
using Newtonsoft.Json;

namespace NewCRM.Web.Controllers
{
    [Export]
    public class LoginController : BaseController
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="accountName"></param>
        /// <param name="passWord"></param>
        /// <param name="isRememberPasswrod"></param>
        /// <returns></returns>
        public ActionResult Landing(String accountName, String passWord, Boolean isRememberPasswrod = false)
        {
            var accountResult = AccountApplicationServices.Login(accountName, passWord);

            Response.SetCookie(new HttpCookie("Account")
            {
                Value = JsonConvert.SerializeObject(accountResult),
                Expires = isRememberPasswrod ? DateTime.Now.AddDays(7) : DateTime.Now.AddMinutes(30)
            });

            AccountId = accountResult.Id;
            AccountName = accountResult.Name;

            return Json(new { success = 1 });
        }
    }
}