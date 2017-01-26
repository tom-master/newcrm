﻿using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Web.Mvc;
using NewCRM.Application.Interface;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    public class AccountSettingController : BaseController
    {

        private readonly IAccountApplicationServices _accountApplicationServices;

        [ImportingConstructor]
        public AccountSettingController(IAccountApplicationServices accountApplicationServices)
        {
            _accountApplicationServices = accountApplicationServices;

        }




        #region 页面
        public ActionResult Index()
        {
            return View(_accountApplicationServices.GetAccount(AccountDto.Id));
        }
        #endregion

        /// <summary>
        ///上传账户头像
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadFace()
        {
            if (Request.Files.Count != 0)
            {
                var icon = Request.Files[0];

                var fileUpLoadHelper = new FileUpLoadHelper(ConfigurationManager.AppSettings["UploadIconPath"], false, true);
                if (fileUpLoadHelper.SaveFile(icon))
                {
                    _accountApplicationServices.ModifyAccountFace(fileUpLoadHelper.FilePath + fileUpLoadHelper.NewFileName);

                    return Json(new { success = true, msg = "" });
                }
                return Json(new { msg = "上传失败" });
            }
            return Json(new { msg = "请上传一个图片" });
        }

        /// <summary>
        /// 修改账户登陆密码
        /// </summary>
        /// <param name="forms"></param>
        /// <returns></returns>
        public ActionResult ModifyAccountPassword(FormCollection forms)
        {
            _accountApplicationServices.ModifyPassword(forms["password"]);

            return Json(new
            {
                success = 1
            });
        }

        /// <summary>
        /// 修改锁屏密码
        /// </summary>
        /// <param name="forms"></param>
        /// <returns></returns>
        public ActionResult ModifyLockScreenPassword(FormCollection forms)
        {
            _accountApplicationServices.ModifyLockScreenPassword(forms["lockpassword"]);

            return Json(new
            {
                success = 1
            });
        }

        /// <summary>
        /// 检查旧密码和输入的密码是否一致
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public ActionResult CheckPassword(String param)
        {

            var result = _accountApplicationServices.CheckPassword(param);

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