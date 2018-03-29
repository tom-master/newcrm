using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;
using NewCRM.Dto;
using NewLib.Validate;
using Nito.AsyncEx;
using Unity.Attributes;

namespace NewCRM.Web.Controllers.ControllerHelper
{
	public class BaseController : Controller
	{
		protected ParameterValidate Parameter => new ParameterValidate();

		[Dependency]
		protected IAccountServices AccountServices { get; set; }

		protected async Task<AccountDto> AccountAsync()
		{
			var accountId = Request.Cookies["memberID"];
			if(accountId != null)
			{
				return await AccountServices.GetAccountAsync(Int32.Parse(accountId.Value));
			}
			return null;
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
