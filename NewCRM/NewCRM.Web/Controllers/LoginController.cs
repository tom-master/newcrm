using System;
using System.Web;
using System.Web.Mvc;
using NewCRM.Application.Interface;
using Newtonsoft.Json;

namespace NewCRM.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAccountApplicationServices _accountApplicationServices;

        public LoginController(IAccountApplicationServices accountApplicationServices)
        {
            _accountApplicationServices = accountApplicationServices;
        }




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
            var account = _accountApplicationServices.Login(accountName, passWord);

            Response.SetCookie(new HttpCookie("Account")
            {
                Value = JsonConvert.SerializeObject(account),
                Expires = isRememberPasswrod ? DateTime.Now.AddDays(7) : DateTime.Now.AddMinutes(30)
            });

            return Json(new { success = 1 });
        }
    }
}