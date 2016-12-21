using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    [Export]
    public class AccountSettingController : BaseController
    {
        public ActionResult Index()
        {
            return View(Account);
        }


        public ActionResult UploadFace()
        {
            if (Request.Files.Count != 0)
            {
                var icon = Request.Files[0];

                var fileUpLoadHelper = new FileUpLoadHelper(ConfigurationManager.AppSettings["UploadIconPath"], false, true);
                if (fileUpLoadHelper.SaveFile(icon))
                {
                    AccountApplicationServices.ModifyAccountFace(Account.Id, fileUpLoadHelper.FilePath + fileUpLoadHelper.NewFileName);

                    return Json(new { success = true, msg = "" });
                }
                return Json(new { msg = "上传失败" });
            }
            return Json(new { msg = "请上传一个图片" });
        }


        public ActionResult ModifyAccountPassword(FormCollection forms)
        {
            AccountApplicationServices.ModifyPassword(Account.Id, forms["password"]);

            return Json(new
            {
                success = 1
            });
        }

        public ActionResult ModifyLockScreenPassword(FormCollection forms)
        {
            AccountApplicationServices.ModifyLockScreenPassword(Account.Id, forms["lockpassword"]);

            return Json(new
            {
                success = 1
            });
        }


        public ActionResult CheckPassword(String param)
        {

            var result = AccountApplicationServices.CheckPassword(Account.Id, param);

            return Json(
                result ? new
                {
                    status = "y",
                    info = ""
                } : new
                {
                    status = "n",
                    info = "原始密码错误"
                });
        }


    }
}