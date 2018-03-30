using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;
using NewCRM.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;
using NewLib;

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
        [HttpGet]
        public async Task<ActionResult> EditMember(Int32 memberId)
        {
            #region 参数验证
            Parameter.Validate(memberId);
            #endregion

            var result = await _deskServices.GetMemberAsync(Account.Id, memberId);
            return View(result);
        }

        #endregion

        /// <summary>
        /// 桌面成员移动
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> MemberMove(String moveType, Int32 memberId, Int32 from, Int32 to)
        {
            #region 参数验证
            Parameter.Validate(moveType).Validate(memberId).Validate(from).Validate(to);
            #endregion

            switch (moveType)
            {
                case "desk-dock": //成员从桌面移动到码头
                    await _deskServices.MemberInDockAsync(Account.Id, memberId);
                    break;
                case "dock-desk": //成员从码头移动到桌面
                    await _deskServices.MemberOutDockAsync(Account.Id, memberId, to);
                    break;
                case "dock-folder": //成员从码头移动到桌面文件夹中
                    await _deskServices.DockToFolderAsync(Account.Id, memberId, to);
                    break;
                case "folder-dock": //成员从文件夹移动到码头
                    await _deskServices.FolderToDockAsync(Account.Id, memberId);
                    break;
                case "desk-folder": //成员从桌面移动到文件夹
                    await _deskServices.DeskToFolderAsync(Account.Id, memberId, to);
                    break;
                case "folder-desk": //成员从文件夹移动到桌面
                    await _deskServices.FolderToDeskAsync(Account.Id, memberId, to);
                    break;
                case "folder-folder": //成员从文件夹移动到另一个文件夹中
                    await _deskServices.FolderToOtherFolderAsync(Account.Id, memberId, to);
                    break;
                case "desk-desk": //桌面移动到另一个桌面
                    await _deskServices.DeskToOtherDeskAsync(Account.Id, memberId, to);
                    break;
                case "dock-otherdesk"://应用码头移动到另一个桌面
                    await _deskServices.DockToOtherDeskAsync(Account.Id, memberId, to);
                    break;
            }
            var response = new ResponseModel
            {
                IsSuccess = true,
                Message = "移动成功"
            };

            return Json(response);
        }

        /// <summary>
        /// 修改文件夹的信息
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> ModifyFolderInfo(String name, String icon, Int32 memberId)
        {
            #region 参数验证
            Parameter.Validate(name).Validate(icon).Validate(memberId);
            #endregion

            var response = new ResponseModel();
            await _deskServices.ModifyFolderInfoAsync(Account.Id, name, icon, memberId);
            response.IsSuccess = true;
            response.Message = "修改成功";

            return Json(response);
        }

        /// <summary>
        /// 修改成员信息
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> ModifyMemberInfo(FormCollection forms)
        {
            #region 参数验证
            Parameter.Validate(forms);
            #endregion

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
                MemberType = forms["membertype"],
                IsIconByUpload = Int32.Parse(forms["isIconByUpload"]) == 1
            };

            var response = new ResponseModel();
            await _deskServices.ModifyMemberInfoAsync(Account.Id, memberDto);
            response.IsSuccess = true;
            response.Message = "修改成员信息成功";

            return Json(response);
        }

        /// <summary>
        /// 更新图标
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> ModifyMemberIcon(Int32 memberId, String newIcon)
        {
            #region 参数验证
            Parameter.Validate(memberId).Validate(newIcon);
            #endregion

            var response = new ResponseModel<String>();
            await _deskServices.ModifyMemberIconAsync(Account.Id, memberId, newIcon);

            response.IsSuccess = true;
            response.Message = "更新图标成功";
            response.Model = ProfileManager.FileUrl + newIcon;

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 卸载桌面的成员
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> UninstallMember(Int32 memberId)
        {
            #region 参数验证
            Parameter.Validate(memberId);
            #endregion

            var response = new ResponseModel();
            await _deskServices.UninstallMemberAsync(Account.Id, memberId);
            response.IsSuccess = true;
            response.Message = "卸载成功";

            return Json(response);
        }

        /// <summary>
        /// 新建文件夹
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateNewFolder(String folderName, String folderImg, Int32 deskId)
        {
            #region 参数验证
            Parameter.Validate(folderName).Validate(folderImg).Validate(deskId);
            #endregion

            var response = new ResponseModel();
            await _deskServices.CreateNewFolderAsync(folderName, folderImg, deskId, Account.Id);
            response.IsSuccess = true;
            response.Message = "新建文件夹成功";

            return Json(response);
        }

        /// <summary>
        /// 检查成员名称
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> CheckMemberName(String param)
        {
            Parameter.Validate(param);
            var result = await _deskServices.CheckMemberNameAsync(param);
            return Json(!result ? new { status = "y", info = "" } : new { status = "n", info = "成员名称已存在" });
        }
    }
}