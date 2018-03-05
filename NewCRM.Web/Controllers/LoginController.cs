using System;
using System.Web;
using System.Web.Mvc;
using NewCRM.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;
using NewLib;
using Newtonsoft.Json;

namespace NewCRM.Web.Controllers
{
    public class LoginController : BaseController
    {

        #region 页面
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Index()
        { 
            var accountId = Request.Cookies["memberID"];
            if (accountId != null)
            {
                return RedirectToAction("Desktop", "Index");
            }

            return View();
        }

        #endregion

        /// <summary>
        /// 登陆
        /// </summary>
        [HttpPost]
        public ActionResult Landing(LoginParameter loginParameter)
        {
            var response = new ResponseModel<AccountDto>();

            #region 参数验证
            Parameter.Validate(loginParameter);
            #endregion

            var account = AccountServices.Login(loginParameter.Name, loginParameter.Password, Request.ServerVariables["REMOTE_ADDR"]);
            if (account != null)
            {
                response.Message = "登陆成功";
                response.IsSuccess = true;
                Response.Cookies.Add(new HttpCookie("memberID")
                {
                    Value = account.Id.ToString(),
                    Expires = loginParameter.IsRememberPasswrod ? DateTime.Now.AddDays(7) : DateTime.Now.AddMinutes(30)
                });

                Response.Cookies.Add(new HttpCookie("Account")
                {
                    Value = JsonConvert.SerializeObject(new { AccountFace = ProfileManager.FileUrl + account.AccountFace, account.Name }),
                    Expires = loginParameter.IsRememberPasswrod ? DateTime.Now.AddDays(7) : DateTime.Now.AddMinutes(30)
                });
            }
            return Json(response);
        }
    }

    public class LoginParameter
    {
        public String Name { get; set; }

        public String Password { get; set; }

        public Boolean IsRememberPasswrod { get; set; }
    }
}