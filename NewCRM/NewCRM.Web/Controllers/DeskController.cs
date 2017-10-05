using System;
using System.Configuration;
using System.Web.Mvc;
using NewCRM.Application.Services.Interface;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Web.Controllers.ControllerHelper;

namespace NewCRM.Web.Controllers
{
	public class DeskController : BaseController
	{
		private readonly IDeskApplicationServices _deskApplicationServices;

		public DeskController(IDeskApplicationServices deskApplicationServices)
		{
			_deskApplicationServices = deskApplicationServices;
		}

		#region 页面

		/// <summary>
		/// 页面
		/// </summary>
		/// <param name="memberId"></param>
		/// <returns></returns>
		public ActionResult EditMember(Int32 accountId, Int32 memberId)
		{
			#region 参数验证
			Parameter.Validate(accountId);
			#endregion

			var result = _deskApplicationServices.GetMember(accountId, memberId);
			return View(result);
		}

		#endregion

		/// <summary>
		/// 桌面元素移动
		/// </summary>
		/// <returns></returns>
		public ActionResult MemberMove(Int32 accountId, String moveType, Int32 memberId = 0, Int32 from = 0, Int32 to = 0)
		{
			switch (moveType)
			{
				case "desk-dock": //成员从桌面移动到码头
					_deskApplicationServices.MemberInDock(accountId, memberId);
					break;
				case "dock-desk": //成员从码头移动到桌面
					_deskApplicationServices.MemberOutDock(accountId, memberId, to);
					break;
				case "dock-folder": //成员从码头移动到桌面文件夹中
					_deskApplicationServices.DockToFolder(accountId, memberId, to);
					break;
				case "folder-dock": //成员从文件夹移动到码头
					_deskApplicationServices.FolderToDock(accountId, memberId);
					break;
				case "desk-folder": //成员从桌面移动到文件夹
					_deskApplicationServices.DeskToFolder(accountId, memberId, to);
					break;
				case "folder-desk": //成员从文件夹移动到桌面
					_deskApplicationServices.FolderToDesk(accountId, memberId, to);
					break;
				case "folder-folder": //成员从文件夹移动到另一个文件夹中
					_deskApplicationServices.FolderToOtherFolder(accountId, memberId, to);
					break;
				case "desk-desk": //桌面移动到另一个桌面
					_deskApplicationServices.DeskToOtherDesk(accountId, memberId, to);
					break;
				case "dock-otherdesk"://应用码头移动到另一个桌面
					_deskApplicationServices.DockToOtherDesk(accountId, memberId, to);
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
		/// <param name="name"></param>
		/// <param name="icon"></param>
		/// <param name="memberId"></param>
		/// <returns></returns>
		public ActionResult ModifyFolderInfo(Int32 accountId, String name, String icon, Int32 memberId)
		{
			#region 参数验证
			Parameter.Validate(accountId).Validate(name).Validate(icon).Validate(memberId);
			#endregion

			var response = new ResponseModel();
			_deskApplicationServices.ModifyFolderInfo(accountId, name, icon, memberId);
			response.IsSuccess = true;
			response.Message = "修改成功";

			return Json(response);
		}

		/// <summary>
		/// 修改成员信息
		/// </summary>
		/// <param name="forms"></param>
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
			_deskApplicationServices.ModifyMemberInfo(Int32.Parse(forms["accountId"]), memberDto);
			response.IsSuccess = true;
			response.Message = "修改成员信息成功";

			return Json(response);
		}

		/// <summary>
		/// 更新图标
		/// </summary>
		/// <returns></returns>
		public ActionResult UploadIcon()
		{
			var response = new ResponseModel();
			if (Request.Files.Count != 0)
			{
				var icon = Request.Files[0];
				var fileUpLoadHelper = new FileUpLoadHelper(ConfigurationManager.AppSettings["UploadIconPath"], false, false);
				if (fileUpLoadHelper.SaveFile(icon))
				{
					response.IsSuccess = true;
					response.Model = fileUpLoadHelper.FilePath + fileUpLoadHelper.OldFileName;
					response.Message = "更新图标成功";
				}
				else
				{
					response.Message = "上传失败";
				}
			}
			else
			{
				response.Message = "请选择一张图片后再上传";
			}

			return Json(response);
		}

		/// <summary>
		/// 卸载桌面的成员
		/// </summary>
		/// <param name="memberId"></param>
		/// <returns></returns>
		public ActionResult UnInstallMember(Int32 accountId,Int32 memberId)
		{
			#region 参数验证
			Parameter.Validate(accountId).Validate(memberId);
			#endregion

			var response = new ResponseModel();
			_deskApplicationServices.RemoveMember(accountId, memberId);
			response.IsSuccess = true;
			response.Message = "卸载成功";

			return Json(response);
		}

		/// <summary>
		/// 新建文件夹
		/// </summary>
		/// <param name="folderName"></param>
		/// <param name="folderImg"></param>
		/// <param name="deskId"></param>
		/// <returns></returns>
		public ActionResult CreateNewFolder(String folderName, String folderImg, Int32 deskId)
		{
			#region 参数验证
			Parameter.Validate(folderName).Validate(folderImg).Validate(deskId);
			#endregion

			var response = new ResponseModel();
			_deskApplicationServices.CreateNewFolder(folderName, folderImg, deskId);
			response.IsSuccess = true;
			response.Message = "新建文件夹成功";

			return Json(response);
		}
	}
}