using System;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;
using NewCRM.Dto;
using NewCRM.Infrastructure.CommonTools.CustomHelper;
using NewLib.Validate;
using Unity.Attributes;

namespace NewCRM.Web.Controllers.ControllerHelper
{
    public class BaseController : Controller
    {
        protected ParameterValidate Parameter => new  ParameterValidate();

        [Dependency]
        protected IAccountServices AccountServices { get; set; }

        protected AccountDto Account
        {
            get
            {
                var accountId = Request.Cookies["memberID"];
                if (accountId != null)
                {
                    return AccountServices.GetAccount(Int32.Parse(accountId.Value));
                }
                return null;
            }
        }
    }
}
