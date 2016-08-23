using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.Impl
{
    [Export(typeof(IDeskServices))]
    internal class DeskServices : BaseService, IDeskServices
    {

        public void ModifyDefaultShowDesk(Int32 userId, Int32 newDefaultDeskNumber)
        {
            var userResult = GetUserInfoService(userId);

            userResult.Config.ModifyDefaultDesk(newDefaultDeskNumber);

            UserRepository.Update(userResult);
        }

        public void ModifyDockPosition(Int32 userId, Int32 defaultDeskNumber, String newPosition)
        {
            var userResult = GetUserInfoService(userId);

            DockPostion dockPostion;
            if (Enum.TryParse(newPosition, true, out dockPostion))
            {
                if (dockPostion == DockPostion.None)
                {
                    var deskResult = DeskRepository.Entities.FirstOrDefault(desk => desk.DeskNumber == userResult.Config.DefaultDeskNumber);

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
                    userResult.Config.ModifyDockPostion(dockPostion);
                }
                else
                {
                    userResult.Config.ModifyDockPostion(dockPostion);
                }
            }
            else
            {
                throw new BusinessException($"未识别出的码头位置:{newPosition}");
            }
            userResult.Config.ModifyDefaultDesk(defaultDeskNumber);

            UserRepository.Update(userResult);
        }

        public Member GetMember(Int32 userId, Int32 memberId, Boolean isFolder = default(Boolean))
        {
            var userConfig = GetUserInfoService(userId).Config;


            foreach (var desk in userConfig.Desks)
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

        public void MemberInDock(Int32 userId, Int32 memberId)
        {
            var userConfig = GetUserInfoService(userId).Config;

            foreach (var desk in userConfig.Desks)
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

        public void MemberOutDock(Int32 userId, Int32 memberId, Int32 deskId)
        {
            var userConfig = GetUserInfoService(userId).Config;
            var realDeskId = GetRealDeskIdService(deskId, userConfig);
            foreach (var desk in userConfig.Desks)
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

        public void DockToFolder(Int32 userId, Int32 memberId, Int32 folderId)
        {
            var configResult = GetUserInfoService(userId).Config;

            var desk = configResult.Desks.FirstOrDefault(c => c.DeskNumber == configResult.DefaultDeskNumber);

            InternalDeskMember(memberId, desk).OutDock().InFolder(folderId);

            DeskRepository.Update(desk);
        }

        public void FolderToDock(Int32 userId, Int32 memberId)
        {
            var configResult = GetUserInfoService(userId).Config;

            var desk = configResult.Desks.FirstOrDefault(c => c.DeskNumber == configResult.DefaultDeskNumber);

            InternalDeskMember(memberId, desk).InDock().OutFolder();

            DeskRepository.Update(desk);
        }

        public void DeskToFolder(Int32 userId, Int32 memberId, Int32 folderId)
        {
            var userConfig = GetUserInfoService(userId).Config;
            foreach (var desk in userConfig.Desks)
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

        public void FolderToDesk(Int32 userId, Int32 memberId, Int32 deskId)
        {
            var userConfig = GetUserInfoService(userId).Config;
            var realDeskId = GetRealDeskIdService(deskId, userConfig);
            foreach (var desk in userConfig.Desks)
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

        public void FolderToOtherFolder(Int32 userId, Int32 memberId, Int32 folderId)
        {
            var configResult = GetUserInfoService(userId).Config;

            var desk = configResult.Desks.FirstOrDefault(c => c.DeskNumber == configResult.DefaultDeskNumber);

            InternalDeskMember(memberId, desk).OutFolder().InFolder(folderId);

            DeskRepository.Update(desk);
        }

        public void DeskToOtherDesk(Int32 userId, Int32 memberId, Int32 deskId)
        {
            var userConfig = GetUserInfoService(userId).Config;

            var realDeskId = GetRealDeskIdService(deskId, userConfig);

            foreach (var desk in userConfig.Desks)
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

        public void ModifyFolderInfo(String memberName, String memberIcon, Int32 memberId, Int32 userId)
        {
            var userConfig = GetUserInfoService(userId).Config;

            foreach (var desk in userConfig.Desks)
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

        public void RemoveMember(Int32 userId, Int32 memberId)
        {
            var userConfig = GetUserInfoService(userId).Config;
            App appResult = null;
            foreach (var desk in userConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);

                if (memberResult != null)
                {
                    if (memberResult.MemberType == MemberType.Folder)
                    {
                        //移除文件夹中的内容
                        foreach (var desk1 in userConfig.Desks)
                        {
                            desk1.Members.Where(d => d.FolderId == memberId).ToList().ForEach(m => m.Remove());
                        }
                    }
                    else
                    {
                        appResult = AppRepository.Entities.FirstOrDefault(app => app.Id == memberResult.AppId);
                        appResult.SubtractUserCount();
                        appResult.SubtractStar(userId);
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

        public void ModifyMemberInfo(Int32 userId, Member member)
        {
            var userResult = GetUserInfoService(userId);

            foreach (var desk in userResult.Config.Desks)
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

        private Member InternalDeskMember(Int32 memberId, Desk desk)
        {
            var memberResult = desk.Members.FirstOrDefault(member => member.Id == memberId);
            return memberResult;
        }


    }
}
