using System;
using System.Web.Mvc;
using System.Text;
using System.ComponentModel.Composition;
using NewCRM.Dto.Dto;

namespace NewCRM.Web.Controllers.ControllerHelper
{
    public class BaseController : Controller
    {

        public static UserDto CurrentUser { get; set; }


        //public IPlantformApplicationService PlantformApplicationService => new PlantformApplicationService();


        protected override JsonResult Json(object data, String contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new Jsons
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior
            };
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            Response.Write("<script>alert('ww')</script>");
            //base.OnException(filterContext);
        }
    }
}
