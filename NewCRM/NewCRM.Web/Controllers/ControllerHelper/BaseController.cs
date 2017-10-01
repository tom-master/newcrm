using System;
using System.ComponentModel.Composition;
using System.Web.Mvc;

namespace NewCRM.Web.Controllers.ControllerHelper
{
    public class BaseController : Controller
    {
        protected static dynamic AccountDto { get; set; }

        protected static Int32 GetAccountId()
        {
            return AccountDto.Id;
        }
    }
}
