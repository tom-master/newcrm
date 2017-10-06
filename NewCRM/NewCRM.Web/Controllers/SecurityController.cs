using System;
using System.Linq;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Web.Controllers.ControllerHelper;
using NewCRM.Infrastructure.CommonTools;
using System.Collections.Generic;

namespace NewCRM.Web.Controllers
{
    public class SecurityController : BaseController
    {
        private readonly ISecurityApplicationServices _securityApplicationServices;

        private readonly IAppApplicationServices _appApplicationServices;

        public SecurityController(ISecurityApplicationServices securityApplicationServices, IAppApplicationServices appApplicationServices)
        {
            _securityApplicationServices = securityApplicationServices;
            _appApplicationServices = appApplicationServices;
        }


        #region 页面

        #region 角色

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 创建新角色
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateNewRole(Int32 roleId = 0)
        {
            if (roleId != 0)
            {
                ViewData["RoleResult"] = _securityApplicationServices.GetRole(roleId);
            }

            return View();
        }

        /// <summary>
        /// 向角色附加权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public ActionResult AttachmentPower(Int32 roleId = 0)
        {
            var role = new RoleDto();
            if (roleId != 0)
            {
                role = _securityApplicationServices.GetRole(roleId);
                ViewData["RolePowerResult"] = role;
            }

            var result = _appApplicationServices.GetSystemApp(role.Powers.Select(s => s.Id).ToArray());

            return View(result);
        }

        #endregion

        #region 权限

        /// <summary>
        /// 添加系统app到权限
        /// </summary>
        /// <returns></returns>
        public ActionResult AddSystemAppGotoPower()
        {
            ViewData["SystemApp"] = _appApplicationServices.GetSystemApp();

            return View();
        }


        #endregion

        #endregion

        #region 角色

        /// <summary>
        /// 获取所有的角色
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllRoles(String roleName, Int32 pageIndex, Int32 pageSize)
        {
            #region 参数验证
            Parameter.Validate(roleName);
            #endregion

            var totalCount = 0;
            var response = new ResponseModels<IList<RoleDto>>();
            var result = _securityApplicationServices.GetAllRoles(roleName, pageIndex, pageSize, out totalCount);
            response.IsSuccess = true;
            response.Message = "获取角色列表成功";
            response.Model = result;
            response.TotalCount = totalCount;

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <returns></returns>
        public ActionResult RemoveRole(Int32 roleId)
        {
            #region 参数验证
            Parameter.Validate(roleId);
            #endregion

            var response = new ResponseModel();
            _securityApplicationServices.RemoveRole(roleId);
            response.IsSuccess = true;
            response.Message = "移除角色成功";

            return Json(response);
        }

        /// <summary>
        /// 添加角色
        /// </summary>
        /// <returns></returns>
        public ActionResult NewRole(FormCollection forms, Int32 roleId = 0)
        {
            #region 参数验证
            Parameter.Validate(forms);
            #endregion

            if (roleId != 0)
            {
                _securityApplicationServices.ModifyRole(WapperRoleDto(forms));
            }
            else
            {
                _securityApplicationServices.AddNewRole(WapperRoleDto(forms));
            }
            var response = new ResponseModel();
            response.IsSuccess = true;
            response.Message = "添加角色成功";

            return Json(response);
        }

        /// <summary>
        /// 将权限附加到角色中
        /// </summary>
        /// <param name="forms"></param>
        /// <returns></returns>
        public ActionResult AddPowerToRole(FormCollection forms)
        {
            #region 参数验证
            Parameter.Validate(forms);
            #endregion

            Int32[] powerIds;
            if ((forms["val_apps_id"] + "").Length > 0)
            {
                powerIds = forms["val_apps_id"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();
            }
            else
            {
                throw new BusinessException("所选的权限列表不能为空");
            }
            var response = new ResponseModel();
            _securityApplicationServices.AddPowerToCurrentRole(Int32.Parse(forms["val_roleId"]), powerIds);
            response.IsSuccess = true;
            response.Message = "将权限附加到角色中成功";

            return Json(response);
        }

        /// <summary>
        /// 选择系统app
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectSystemApp(String appIds = "")
        {
            var response = new ResponseModel<IList<AppDto>>();
            var internalAppIds = appIds.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray();
            var result = _appApplicationServices.GetSystemApp(internalAppIds);
            response.IsSuccess = true;
            response.Message = "选择系统app成功";
            response.Model = result;

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region private method

        private static RoleDto WapperRoleDto(FormCollection forms)
        {
            Int32 roleId = 0;
            if ((forms["roleId"] + "").Length > 0)
            {
                roleId = Int32.Parse(forms["roleId"]);
            }

            return new RoleDto
            {
                RoleIdentity = forms["val_roleIdentity"],
                Id = roleId,
                Name = forms["val_roleName"],
                Remark = forms["val_roleRemake"]
            };
        }

        #endregion
    }
}