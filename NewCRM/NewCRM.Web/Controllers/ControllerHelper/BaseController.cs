using System;
using System.Web.Mvc;
using NewCRM.Infrastructure.CommonTools.CustomHelper;

namespace NewCRM.Web.Controllers.ControllerHelper
{
	public class BaseController : Controller
	{
		protected Parameter Parameter => new Parameter();
	}
}
