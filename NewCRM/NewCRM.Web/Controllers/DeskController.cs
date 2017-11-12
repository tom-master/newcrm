using NewCRM.Application.Services.Interface;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;
using System;
using System.Configuration;
using System.Web.Mvc;

namespace NewCRM.Web.Controllers
{
    public class DeskController : BaseController
    {
        private readonly IDeskServices _deskServices;

        public DeskController(IDeskServices deskServices)
        {
            _deskServices = deskServices;
        }

        #region 页面

        /// <summary>
        /// 页面
        /// </summary>
        /// <returns></returns>
        public ActionResult EditMember(Int32 accountId, Int32 memberId)
        {
            #region 参数验证
            Parameter.Validate(accountId);
            #endregion

            var result = _deskServices.GetMember(accountId, memberId);
            return View(result);
        }

        #endregion

        /// <summary>
        /// 桌面元素移动
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberMove(String moveType, Int32 memberId, Int32 from, Int32 to)
        {
            #region 参数验证
            Parameter.Validate(moveType).Validate(memberId);
            #endregion

            switch (moveType)
            {
                case "desk-dock": //成员从桌面移动到码头
                    _deskServices.MemberInDock(Account.Id, memberId);
                    break;
                case "dock-desk": //成员从码头移动到桌面
                    _deskServices.MemberOutDock(Account.Id, memberId, to);
                    break;
                case "dock-folder": //成员从码头移动到桌面文件夹中
                    _deskServices.DockToFolder(Account.Id, memberId, to);
                    break;
                case "folder-dock": //成员从文件夹移动到码头
                    _deskServices.FolderToDock(Account.Id, memberId);
                    break;
                case "desk-folder": //成员从桌面移动到文件夹
                    _deskServices.DeskToFolder(Account.Id, memberId, to);
                    break;
                case "folder-desk": //成员从文件夹移动到桌面
                    _deskServices.FolderToDesk(Account.Id, memberId, to);
                    break;
                case "folder-folder": //成员从文件夹移动到另一个文件夹中
                    _deskServices.FolderToOtherFolder(Account.Id, memberId, to);
                    break;
                case "desk-desk": //桌面移动到另一个桌面
                    _deskServices.DeskToOtherDesk(Account.Id, memberId, to);
                    break;
                case "dock-otherdesk"://应用码头移动到另一个桌面
                    _deskServices.DockToOtherDesk(Account.Id, memberId, to);
                    break;
            }
            var response = new ResponseModel();
            response.IsSuccess = true;
            response.Message = "移动成功";

            return Json(response);
        }

        /// <summary>
        /// 修改文件夹的信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyFolderInfo(String name, String icon, Int32 memberId)
        {
            #region 参数验证
            Parameter.Validate(name).Validate(icon).Validate(memberId);
            #endregion

            var response = new ResponseModel();
            _deskServices.ModifyFolderInfo(Account.Id, name, icon, memberId);
            response.IsSuccess = true;
            response.Message = "修改成功";

            return Json(response);
        }

        /// <summary>
        /// 修改成员信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ModifyMemberInfo(FormCollection forms)
        {
            var memberDto = new MemberDto
            {
                Id = Int32.Parse(forms["id"]),
                IconUrl = forms["val_icon"],
                Name = forms["val_name"],
                Width = Int32.Parse(forms["val_width"]),
                Height = Int32.Parse(forms["val_height"]),
                IsResize = Int32.Parse(forms["val_isresize"]) == 1,
                IsOpenMax = Int32.Parse(forms["val_isopenmax"]) == 1,
                IsFlash = Int32.Parse(forms["val_isflash"]) == 1,
                MemberType = forms["membertype"]
            };

            var response = new ResponseModel();
            _deskServices.ModifyMemberInfo(Int32.Parse(forms["accountId"]), memberDto);
            response.IsSuccess = true;
            response.Message = "修改成员信息成功";

            return Json(response);
        }

        /// <summary>
        /// 更新图标
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UploadIcon(Int32 memberId, String newIcon)
        {
            #region 参数验证
            Parameter.Validate(memberId).Validate(newIcon);
            #endregion

            var response = new ResponseModel();
            _deskServices.ModifyMemberIcon(Account.Id, memberId, newIcon);

            response.IsSuccess = true;
            response.Message = "更新图标成功";

            return Json(response);
        }

        /// <summary>
        /// 卸载桌面的成员
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public ActionResult UnInstallMember(Int32 memberId)
        {
            #region 参数验证
            Parameter.Validate(memberId);
            #endregion

            var response = new ResponseModel();
            _deskServices.RemoveMember(Account.Id, memberId);
            response.IsSuccess = true;
            response.Message = "卸载成功";

            return Json(response);
        }

        /// <summary>
        /// 新建文件夹
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateNewFolder(String folderName, String folderImg, Int32 deskId)
        {
            #region 参数验证
            Parameter.Validate(folderName).Validate(folderImg).Validate(deskId);
            #endregion

            var response = new ResponseModel();
            _deskServices.CreateNewFolder(folderName, folderImg, deskId);
            response.IsSuccess = true;
            response.Message = "新建文件夹成功";

            return Json(response);
        }
    }
}