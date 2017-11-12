using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace NewCRM.Web.Controllers
{
    public class AccountSettingController : BaseController
    {
        #region 页面

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View(AccountServices.GetAccount(Account.Id));
        }

        #endregion

        /// <summary>
        ///上传账户头像
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadFace()
        {
            return null;
            //var msg = "";
            //var success = false;
            //var files = new List<String>();
            //if (Request.Files.Count != 0)
            //{
            //    var icon = Request.Files[0];

            //    var fileUpLoadHelper = new FileUpLoadHelper(ConfigurationManager.AppSettings["UploadIconPath"], false, true);
            //    if (fileUpLoadHelper.SaveFile(icon))
            //    {
            //        var filePath = fileUpLoadHelper.FilePath + fileUpLoadHelper.NewFileName+".jpg";
            //        AccountServices.ModifyAccountFace(Account.Id, filePath);
            //        files.Add(filePath);
            //        msg = "头像上传成功";
            //        success = true;
            //    }
            //    else
            //    {
            //        msg = "头像上传失败";
            //    }
            //}
            //else
            //{
            //    msg = "请选择一张图片后再进行上传";
            //}
            //return Json(new { avatarUrls = files, msg, success });
        }

        /// <summary>
        /// 修改账户登陆密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyAccountPassword(FormCollection forms)
        {
            #region 参数验证
            Parameter.Validate(forms);
            #endregion

            var response = new ResponseModel();
            AccountServices.ModifyPassword(Account.Id, forms["password"]);
            response.Message = "账户密码修改成功";
            response.IsSuccess = true;

            return Json(response);
        }

        /// <summary>
        /// 修改锁屏密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyLockScreenPassword(FormCollection forms)
        {
            #region 参数验证
            Parameter.Validate(forms);
            #endregion

            var response = new ResponseModel();
            AccountServices.ModifyLockScreenPassword(Account.Id, forms["lockpassword"]);

            response.Message = "锁屏密码修改成功";
            response.IsSuccess = true;

            return Json(response);
        }

        /// <summary>
        /// 检查旧密码和输入的密码是否一致
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckPassword(String param)
        {
            #region 参数验证
            Parameter.Validate(param);
            #endregion

            var response = new ResponseModel<dynamic>();
            var result = AccountServices.CheckPassword(Account.Id, param);

            response.IsSuccess = true;
            response.Model = result ? new { status = "y", info = "" } : new { status = "n", info = "原始密码错误" };

            return Json(response);
        }
    }
}