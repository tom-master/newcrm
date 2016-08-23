using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web.Mvc;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustemException;
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

        public ActionResult CreateNewUser(Int32 userId = 0)
        {
            if (userId != 0)
            {
                ViewData["User"] = AccountApplicationServices.GetUser(userId);
            }

            ViewData["Roles"] = SecurityApplicationServices.GetAllRoles();

            return View();
        }

        #endregion

        public ActionResult GetAllUsers(String userName, String userType, Int32 pageIndex, Int32 pageSize)
        {
            Int32 totalCount;

            var users = AccountApplicationServices.GetAllUsers(userName, userType, pageIndex, pageSize, out totalCount);

            return Json(new
            {
                users,
                totalCount
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult NewUser(FormCollection forms, Int32 userId)
        {
            var userDto = WapperUserDto(forms);

            if (userId == 0)
            {
                AccountApplicationServices.AddNewUser(userDto);
            }
            else
            {
                AccountApplicationServices.ModifyUser(userDto);
            }

            return Json(new { success = 1 });
        }



        public ActionResult ValidUserExist(String param)
        {
            var value = AccountApplicationServices.ValidSameUserNameExist(param) ? 'y' : 'n';
            return Json(new { status = value });
        }

        #region private method

        private static UserDto WapperUserDto(FormCollection forms)
        {
            Int32 userId = 0;
            if ((forms["userId"] + "").Length > 0)
            {
                userId = Int32.Parse(forms["userId"]);
            }

            List<RoleDto> roleIds;

            if ((forms["val_roleIds"] + "").Length > 0)
            {
                roleIds = forms["val_roleIds"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(role => new RoleDto
                {
                    Id = Int32.Parse(role)
                }).ToList();
            }
            else
            {
                throw new BusinessException("所选的角色列表不能为空");
            }

            return new UserDto
            {
                Id = userId,
                Name = forms["val_username"],
                Password = forms["val_password"],
                UserType = forms["val_type"],
                Roles = roleIds
            };
        }

        #endregion
    }
}