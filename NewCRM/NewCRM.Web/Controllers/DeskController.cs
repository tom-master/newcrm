using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Web.Mvc;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Dto.Dto;
using NewCRM.Web.Controllers.ControllerHelper;
using NewCRM.Infrastructure.CommonTools;
using Newtonsoft.Json;

namespace NewCRM.Web.Controllers
{
    [Export]
    public class DeskController : BaseController
    {
        [Import]
        private IDeskApplicationServices _deskApplicationServices;

     

        // GET: Desks
        public ActionResult EditMember(Int32 memberId)
        {
            var memberResult = _deskApplicationServices.GetMember(CurrentUser.Id, memberId);
            return View(memberResult);
        }

        /// <summary>
        /// 桌面元素移动
        /// </summary>
        /// <returns></returns>
        public ActionResult MemberMove(String moveType, Int32 memberId = 0, Int32 from = 0, Int32 to = 0)
        {
            switch (moveType)
            {
                case "desk-dock": //成员从桌面移动到码头
                    _deskApplicationServices.MemberInDock(CurrentUser.Id, memberId);
                    break;
                case "dock-desk": //成员从码头移动到桌面
                    _deskApplicationServices.MemberOutDock(CurrentUser.Id, memberId, to);
                    break;
                case "dock-folder": //成员从码头移动到桌面文件夹中
                    _deskApplicationServices.DockToFolder(CurrentUser.Id, memberId, to);
                    break;
                case "folder-dock": //成员从文件夹移动到码头
                    _deskApplicationServices.FolderToDock(CurrentUser.Id, memberId);
                    break;
                case "desk-folder": //成员从桌面移动到文件夹
                    _deskApplicationServices.DeskToFolder(CurrentUser.Id, memberId, to);
                    break;
                case "folder-desk": //成员从文件夹移动到桌面
                    _deskApplicationServices.FolderToDesk(CurrentUser.Id, memberId, to);
                    break;
                case "folder-folder": //成员从文件夹移动到另一个文件夹中
                    _deskApplicationServices.FolderToOtherFolder(CurrentUser.Id, memberId, to);
                    break;
                case "desk-desk":
                    _deskApplicationServices.DeskToOtherDesk(CurrentUser.Id, memberId, to);
                    break;
            }
            return new EmptyResult();
        }

        /// <summary>
        /// 修改文件夹的信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="icon"></param>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public ActionResult ModifyFolderInfo(String name, String icon, Int32 memberId)
        {
            _deskApplicationServices.ModifyFolderInfo(name, icon, memberId, CurrentUser.Id);
            return new EmptyResult();
        }

        /// <summary>
        /// 修改app信息
        /// </summary>
        /// <param name="forms"></param>
        /// <returns></returns>
        public ActionResult ModifyAppInfo(FormCollection forms)
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
                IsFlash = Int32.Parse(forms["val_isflash"]) == 1
            };

            _deskApplicationServices.ModifyMemberInfo(CurrentUser.Id, memberDto);

            return Json(new
            {
                status = "y"
            });
        }

        /// <summary>
        /// 更新图标
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadIcon()
        {
            if (Request.Files.Count != 0)
            {
                var icon = Request.Files[0];

                var fileUpLoadHelper = new FileUpLoadHelper(ConfigurationManager.AppSettings["UploadIconPath"], false, false);
                if (fileUpLoadHelper.SaveFile(icon))
                {
                    return Json(new { iconPath = fileUpLoadHelper.FilePath + fileUpLoadHelper.OldFileName });
                }
                return Json(new { msg = "上传失败" });
            }
            return Json(new { msg = "请上传一个图片" });
        }

        /// <summary>
        /// 卸载桌面的成员
        /// </summary>
        /// <param name="memberId"></param>
        /// <returns></returns>
        public ActionResult UnInstallMember(Int32 memberId)
        {
            _deskApplicationServices.RemoveMember(CurrentUser.Id, memberId);
            return new EmptyResult();
        }


    }
}