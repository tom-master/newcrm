using System;
using System.Web.Mvc;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    public class SecurityController : BaseController
    {

        #region 页面

        // GET: SystemAuthority
        public ActionResult RoleManage()
        {
            return View();
        }


        public ActionResult CreateNewRole()
        {
            return View();
        }

        #endregion

        public ActionResult GetAllRoles(String roleName, Int32 pageIndex, Int32 pageSize)
        {
            var totalCount = 0;

            var roles = SecurityApplicationServices.GetAllRoles(roleName, pageIndex, pageSize, out totalCount);

            return Json(new { roles, totalCount }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult RemoveRole(Int32 roleId)
        {
            SecurityApplicationServices.RemoveRole(roleId);

            return Json(new
            {
                success = 1
            });
        }
    }
}