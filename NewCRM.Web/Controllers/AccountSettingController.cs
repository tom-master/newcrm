using System;
using System.Web.Mvc;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    public class AccountSettingController : BaseController
    {
        #region 页面

        /// <summary>
        /// 首页
        /// </summary>
        public ActionResult Index() => View(AccountServices.GetAccount(Account.Id));

        #endregion

        /// <summary>
        ///上传账户头像
        /// </summary>
        [HttpGet]
        public ActionResult ModifyAccountFace(String accountFace)
        {
            var response = new ResponseModel();
            AccountServices.ModifyAccountFace(Account.Id, accountFace);
            response.IsSuccess = true;
            response.Model = "头像上传成功";

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改账户登陆密码
        /// </summary>
        [HttpPost]
        public ActionResult ModifyAccountPassword(FormCollection forms)
        {
            #region 参数验证
            Parameter.Validate(forms);
            #endregion

            var response = new ResponseModel();
            AccountServices.ModifyPassword(Account.Id, forms["password"], Int32.Parse(forms["lockPwdIsEqLoginPwd"]) == 1);
            response.Message = "账户密码修改成功";
            response.IsSuccess = true;
            InternalLogout();
            return Json(response);
        }

        /// <summary>
        /// 修改锁屏密码
        /// </summary>
        [HttpPost]
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
        [HttpPost]
        public ActionResult CheckPassword(String param)
        {
            #region 参数验证
            Parameter.Validate(param);
            #endregion

            var result = AccountServices.CheckPassword(Account.Id, param);
            return Json(result ? new { status = "y", info = "" } : new { status = "n", info = "原始密码错误" }, JsonRequestBehavior.AllowGet);
        }
    }
}