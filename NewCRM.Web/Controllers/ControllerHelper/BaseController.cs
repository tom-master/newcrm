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
	public class BaseController: Controller
	{
		protected ParameterValidate Parameter => new ParameterValidate();

		[Dependency]
		protected IAccountServices AccountServices { get; set; }

		protected Int32 AccountId
		{
			get
			{
				var accountId = Request.Cookies["MemberID"];
				return Request.Cookies["memberID"] == null ? AsyncContext.Run(() => AccountServices.GetAccountAsync(Int32.Parse(accountId.Value))).Id : Int32.Parse(accountId.Value);
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
