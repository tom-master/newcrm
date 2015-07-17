using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NewCRM.Web.Controllers
{
    public class AuthorityManageController : Controller
    {
        // GET: SystemAuthority
        public ActionResult RoleManage()
        {
            return View();
        }
    }
}