using System;
using System.Web.Mvc;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomHelper;
using Newtonsoft.Json;

namespace NewCRM.Web.Controllers.ControllerHelper
{
    public class BaseController : Controller
    {
        protected Parameter Parameter => new Parameter();

        protected AccountDto Account
        {
            get
            {
                var accountCookie = Request.Cookies["Account"];
                if (accountCookie != null)
                {
                    return JsonConvert.DeserializeObject<AccountDto>(accountCookie.Value);
                }

                RedirectToAction("Landing", "Login");
                return null;
            }
        }
    }
}
