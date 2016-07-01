
namespace NewCRM.Web.Controllers.ControllerHelper
{
    using System;
    using System.Web.Mvc;
    using System.Text;
    public class BaseController : Controller
    {

        //public static UserDto CurrentUser { get; set; }


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
    }
}
