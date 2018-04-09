using System;
using System.Web;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;
using NewLib.Validate;
using Unity.Attributes;

namespace NewCRM.Web.Controllers.ControllerHelper
{
	public class BaseController : Controller
	{
		[Dependency]
		protected IAccountServices AccountServices { get; set; }

		protected Int32 AccountId
		{
			get
			{
				var accountId = Request.Cookies["MemberID"];

				if(accountId != null)
				{
					return Int32.Parse(accountId.Value);
				}
				return 0;
			}
		}

		protected void InternalLogout()
		{
			Response.Cookies.Add(new HttpCookie("memberID")
			{
				Expires = DateTime.Now.AddDays(-1)
			});
		}
	}
}
