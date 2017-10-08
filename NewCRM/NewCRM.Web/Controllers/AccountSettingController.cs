using System;
using System.Configuration;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    public class AccountSettingController : BaseController
    {
        private readonly IAccountApplicationServices _accountServices;

        public AccountSettingController(IAccountApplicationServices accountServices)
        {
            _accountServices = accountServices;
        }

        #region 页面
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index(Int32 accountId)
        {
            return View(_accountServices.GetAccount(accountId));
        }

        #endregion

        /// <summary>
        ///上传账户头像
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadFace(Int32 accountId)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var response = new ResponseModel<dynamic>();
            if (Request.Files.Count != 0)
            {
                var icon = Request.Files[0];

                var fileUpLoadHelper = new FileUpLoadHelper(ConfigurationManager.AppSettings["UploadIconPath"], false, true);
                if (fileUpLoadHelper.SaveFile(icon))
                {
                    _accountServices.ModifyAccountFace(accountId, fileUpLoadHelper.FilePath + fileUpLoadHelper.NewFileName);

                    response.Message = "头像上传成功";
                    response.IsSuccess = true;
                }
                else
                {
                    response.Message = "头像上传失败";
                }
            }
            else
            {
                response.Message = "请选择一张图片后再进行上传";
            }
            return Json(response);
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
            _accountServices.ModifyPassword(Int32.Parse(forms["accountId"]), forms["password"]);
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
            _accountServices.ModifyLockScreenPassword(Int32.Parse(forms["accountId"]), forms["lockpassword"]);

            response.Message = "锁屏密码修改成功";
            response.IsSuccess = true;

            return Json(response);
        }

        /// <summary>
        /// 检查旧密码和输入的密码是否一致
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckPassword(Int32 accountId, String param)
        {
            #region 参数验证
            Parameter.Validate(accountId).Validate(param);
            #endregion

            var response = new ResponseModel<dynamic>();
            var result = _accountServices.CheckPassword(accountId, param);
            response.IsSuccess = true;
            response.Model = result ? new { status = "y", info = "" } : new { status = "n", info = "原始密码错误" };

            return Json(response);
        }
    }
}