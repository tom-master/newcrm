using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
    [Export]
    public class DeskController : BaseController
    {
        // GET: Desk
        public ActionResult Index()
        {
            return View();
        }

        [Import]
        private IDeskApplicationServices _deskApplicationServices;

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
                    _deskApplicationServices.FolderToDesk(CurrentUser.Id, memberId);
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



        public ActionResult ModifyMemberInfoOfFolder(String name, String icon, Int32 memberId)
        {
            _deskApplicationServices.ModifyFolderInfo(name, icon, memberId, CurrentUser.Id);
            return new EmptyResult();
        }

        public ActionResult UnInstallMemberOfFolder(Int32 memberId)
        {
            _deskApplicationServices.RemoveMemberOfFolder(CurrentUser.Id, memberId);
            return new EmptyResult();
        }

        public ActionResult UnInstallMemberOfApp(Int32 memberId)
        {
            _deskApplicationServices.RemoveMemberOfApp(CurrentUser.Id, memberId);
            return new EmptyResult();
        }
    }
}