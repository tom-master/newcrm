using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.Impl
{
    [Export(typeof(IAccountConfigServices))]
    internal sealed class AccountConfigServices : BaseService, IAccountConfigServices
    {
        public void ModifyAppDirection(Int32 accountId, String direction)
        {
            var accountResult = GetAccountInfoService(accountId);

            if (direction.ToLower() == "x")
            {
                accountResult.Config.ModifyAppDirectionToX();
            }
            else if (direction.ToLower() == "y")
            {
                accountResult.Config.ModifyAppDirectionToY();
            }
            else
            {
                throw new BusinessException($"未能识别的App排列方向:{direction.ToLower()}");
            }
            AccountRepository.Update(accountResult);
        }

        public void ModifyAppIconSize(Int32 accountId, Int32 newSize)
        {
            var accountResult = GetAccountInfoService(accountId);
            accountResult.Config.ModifyDisplayIconLength(newSize);
            AccountRepository.Update(accountResult);
        }

        public void ModifyAppVerticalSpacing(Int32 accountId, Int32 newSize)
        {
            var accountResult = GetAccountInfoService(accountId);
            accountResult.Config.ModifyAppVerticalSpacingLength(newSize);
            AccountRepository.Update(accountResult);
        }

        public void ModifyAppHorizontalSpacing(Int32 accountId, Int32 newSize)
        {
            var accountResult = GetAccountInfoService(accountId);
            accountResult.Config.ModifyAppHorizontalSpacingLength(newSize);
            AccountRepository.Update(accountResult);
        }

        public void ModifyDefaultShowDesk(Int32 accountId, Int32 newDefaultDeskNumber)
        {
            var accountResult = GetAccountInfoService(accountId);
            accountResult.Config.ModifyDefaultDesk(newDefaultDeskNumber);
            AccountRepository.Update(accountResult);
        }

        public void ModifyDockPosition(Int32 accountId, Int32 defaultDeskNumber, String newPosition)
        {
            var accountResult = GetAccountInfoService(accountId);

            DockPostion dockPostion;
            if (Enum.TryParse(newPosition, true, out dockPostion))
            {
                if (dockPostion == DockPostion.None)
                {
                    var deskResult = DeskRepository.Entities.FirstOrDefault(desk => desk.DeskNumber == accountResult.Config.DefaultDeskNumber);

                    var dockMembers = deskResult.Members.Where(member => member.IsOnDock).ToList();

                    if (dockMembers.Any())
                    {
                        dockMembers.ToList().ForEach(
                        f =>
                        {
                            f.OutDock();
                        });
                        DeskRepository.Update(deskResult);
                    }
                    accountResult.Config.ModifyDockPostion(dockPostion);
                }
                else
                {
                    accountResult.Config.ModifyDockPostion(dockPostion);
                }
            }
            else
            {
                throw new BusinessException($"未识别出的码头位置:{newPosition}");
            }
            accountResult.Config.ModifyDefaultDesk(defaultDeskNumber);

            AccountRepository.Update(accountResult);
        }

        public Member GetMember(Int32 accountId, Int32 memberId, Boolean isFolder = default(Boolean))
        {
            var accountConfig = GetAccountInfoService(accountId).Config;


            foreach (var desk in accountConfig.Desks)
            {
                var members = desk.Members;
                if (isFolder)
                {
                    var folderMember = members.FirstOrDefault(member => member.Id == memberId && member.MemberType == MemberType.Folder);
                    if (folderMember != null)
                    {
                        return folderMember;
                    }
                }
                else
                {
                    var appMember = members.FirstOrDefault(member => member.AppId == memberId && member.MemberType == MemberType.App);
                    if (appMember != null)
                    {
                        return appMember;
                    }
                }
            }
            throw new BusinessException($"未找到app");

        }

        public void MemberInDock(Int32 accountId, Int32 memberId)
        {
            var accountConfig = GetAccountInfoService(accountId).Config;

            foreach (var desk in accountConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);
                if (memberResult != null)
                {
                    memberResult.InDock();
                    DeskRepository.Update(desk);
                    break;
                }
            }
        }

        public void MemberOutDock(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            var accountConfig = GetAccountInfoService(accountId).Config;
            var realDeskId = GetRealDeskIdService(deskId, accountConfig);
            foreach (var desk in accountConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);
                if (memberResult != null)
                {
                    memberResult.OutDock().ToOtherDesk(realDeskId);
                    DeskRepository.Update(desk);
                    break;
                }
            }
        }

        public void DockToFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            var configResult = GetAccountInfoService(accountId).Config;

            var desk = configResult.Desks.FirstOrDefault(c => c.DeskNumber == configResult.DefaultDeskNumber);

            InternalDeskMember(memberId, desk).OutDock().InFolder(folderId);

            DeskRepository.Update(desk);
        }

        public void FolderToDock(Int32 accountId, Int32 memberId)
        {
            var configResult = GetAccountInfoService(accountId).Config;

            var desk = configResult.Desks.FirstOrDefault(c => c.DeskNumber == configResult.DefaultDeskNumber);

            InternalDeskMember(memberId, desk).InDock().OutFolder();

            DeskRepository.Update(desk);
        }

        public void DeskToFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            var accountConfig = GetAccountInfoService(accountId).Config;
            foreach (var desk in accountConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);
                if (memberResult != null)
                {
                    memberResult.InFolder(folderId);
                    DeskRepository.Update(desk);
                    break;
                }
            }
        }

        public void FolderToDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            var accountConfig = GetAccountInfoService(accountId).Config;
            var realDeskId = GetRealDeskIdService(deskId, accountConfig);
            foreach (var desk in accountConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);
                if (memberResult != null)
                {
                    if (memberResult.DeskId == realDeskId)
                    {
                        memberResult.OutFolder();
                    }
                    else
                    {
                        memberResult.OutFolder().ToOtherDesk(realDeskId);
                    }
                    DeskRepository.Update(desk);
                    break;
                }
            }
        }

        public void FolderToOtherFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            var configResult = GetAccountInfoService(accountId).Config;

            var desk = configResult.Desks.FirstOrDefault(c => c.DeskNumber == configResult.DefaultDeskNumber);

            InternalDeskMember(memberId, desk).OutFolder().InFolder(folderId);

            DeskRepository.Update(desk);
        }

        public void DeskToOtherDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            var accountConfig = GetAccountInfoService(accountId).Config;

            var realDeskId = GetRealDeskIdService(deskId, accountConfig);

            foreach (var desk in accountConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);
                if (memberResult != null)
                {
                    memberResult.ToOtherDesk(realDeskId);
                    DeskRepository.Update(desk);
                    break;
                }
            }
        }

        public void ModifyFolderInfo(String memberName, String memberIcon, Int32 memberId, Int32 accountId)
        {
            var accountConfig = GetAccountInfoService(accountId).Config;

            foreach (var desk in accountConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);
                if (memberResult != null)
                {
                    memberResult.ModifyName(memberName).ModifyIcon(memberIcon);
                    DeskRepository.Update(desk);
                    break;
                }
            }
        }

        public void RemoveMember(Int32 accountId, Int32 memberId)
        {
            var accountConfig = GetAccountInfoService(accountId).Config;
            App appResult = null;
            foreach (var desk in accountConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);

                if (memberResult != null)
                {
                    if (memberResult.MemberType == MemberType.Folder)
                    {
                        //移除文件夹中的内容
                        foreach (var desk1 in accountConfig.Desks)
                        {
                            desk1.Members.Where(d => d.FolderId == memberId).ToList().ForEach(m => m.Remove());
                        }
                    }
                    else
                    {
                        appResult = AppRepository.Entities.FirstOrDefault(app => app.Id == memberResult.AppId);
                        appResult.SubtractUseCount();
                        appResult.SubtractStar(accountId);
                    }

                    memberResult.Remove();
                    DeskRepository.Update(desk);

                    if (appResult != null)
                    {
                        AppRepository.Update(appResult);
                    }
                    break;
                }
            }
        }

        public void ModifyMemberInfo(Int32 accountId, Member member)
        {
            var accountResult = GetAccountInfoService(accountId);

            foreach (var desk in accountResult.Config.Desks)
            {
                var memberResult = InternalDeskMember(member.Id, desk);
                if (memberResult != null)
                {
                    memberResult.ModifyIcon(member.IconUrl)
                    .ModifyName(member.Name)
                    .ModifyWidth(member.Width)
                    .ModifyHeight(member.Height)
                    .ModifyIsResize(member.IsResize)
                    .ModifyIsOpenMax(member.IsOpenMax)
                    .ModifyIsFlash(member.IsFlash);
                    DeskRepository.Update(desk);
                    break;
                }
            }
        }

        public void ModifySkin(Int32 accountId, String newSkin)
        {
            var accountResult = GetAccountInfoService(accountId);

            accountResult.Config.ModifySkin(newSkin);

            AccountRepository.Update(accountResult);
        }

        private Member InternalDeskMember(Int32 memberId, Desk desk)
        {
            var memberResult = desk.Members.FirstOrDefault(member => member.Id == memberId);
            return memberResult;
        }
    }
}
