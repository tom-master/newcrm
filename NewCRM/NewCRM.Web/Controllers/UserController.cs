using System;
using System.ComponentModel.Composition;
using System.Web.Mvc;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    [Export]
    public class UserController : BaseController
    {
        #region 页面
        public ActionResult UserManage()
        {
            return View();
        }

        public ActionResult CreateNewUser(Int32 userId)
        {
            if (userId != 0)
            {

            }
            return View();
        }

        #endregion

        public ActionResult GetAllUsers(String userName, String userType, Int32 pageIndex, Int32 pageSize)
        {
            Int32 totalCount = 0;

            var users = AccountApplicationServices.GetAllUsers(userName, userType, pageIndex, pageSize, out totalCount);

            return Json(new
            {
                users,
                totalCount
            }, JsonRequestBehavior.AllowGet);
        }
    }
}