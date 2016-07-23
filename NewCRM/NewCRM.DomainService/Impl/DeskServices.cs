using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.Repositories.IRepository.Account;
using NewCRM.Domain.Entities.Repositories.IRepository.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.Impl
{
    [Export(typeof(IDeskServices))]
    public class DeskServices : BaseService, IDeskServices
    {
        [Import]
        private IDeskRepository _deskRepository;

        public void ModifyDefaultShowDesk(Int32 userId, Int32 newDefaultDeskNumber)
        {
            var userResult = GetUser(userId);

            userResult.Config.ModifyDefaultDesk(newDefaultDeskNumber);

            UserRepository.Update(userResult);
        }

        public void ModifyDockPosition(Int32 userId, Int32 defaultDeskNumber, String newPosition)
        {
            var userResult = GetUser(userId);

            DockPostion dockPostion;
            if (Enum.TryParse(newPosition, true, out dockPostion))
            {
                if (dockPostion == DockPostion.None)
                {
                    var deskResult = _deskRepository.Entities.FirstOrDefault(desk => desk.DeskNumber == userResult.Config.DefaultDeskNumber);

                    var dockMembers = deskResult.Members.Where(member => member.IsOnDock && member.IsDeleted == false).ToList();

                    if (dockMembers.Any())
                    {
                        dockMembers.ToList().ForEach(
                        f =>
                        {
                            f.OutDock();
                        });
                        _deskRepository.Update(deskResult);
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
            var userConfig = GetUser(userId).Config;


            foreach (var desk in userConfig.Desks)
            {
                var members = desk.Members.Where(member => member.IsDeleted == false);
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
            var userConfig = GetUser(userId).Config;

            foreach (var desk in userConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);
                if (memberResult != null)
                {
                    memberResult.InDock();
                    _deskRepository.Update(desk);
                    break;
                }
            }
        }

        public void MemberOutDock(Int32 userId, Int32 memberId, Int32 deskId)
        {
            var userConfig = GetUser(userId).Config;
            var realDeskId = userConfig.Desks.FirstOrDefault(desk => desk.DeskNumber == deskId).Id;
            foreach (var desk in userConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);
                if (memberResult != null)
                {
                    memberResult.OutDock().ToOtherDesk(realDeskId);
                    _deskRepository.Update(desk);
                    break;
                }
            }
        }

        public void DockToFolder(Int32 userId, Int32 memberId, Int32 folderId)
        {
            var configResult = GetUser(userId).Config;

            var desk = configResult.Desks.FirstOrDefault(c => c.DeskNumber == configResult.DefaultDeskNumber);

            InternalDeskMember(memberId, desk).OutDock().InFolder(folderId);

            _deskRepository.Update(desk);
        }

        public void FolderToDock(Int32 userId, Int32 memberId)
        {
            var configResult = GetUser(userId).Config;

            var desk = configResult.Desks.FirstOrDefault(c => c.DeskNumber == configResult.DefaultDeskNumber);

            InternalDeskMember(memberId, desk).InDock().OutFolder();

            _deskRepository.Update(desk);
        }

        public void DeskToFolder(Int32 userId, Int32 memberId, Int32 folderId)
        {
            var userConfig = GetUser(userId).Config;
            foreach (var desk in userConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);
                if (memberResult != null)
                {
                    memberResult.InFolder(folderId);
                    _deskRepository.Update(desk);
                    break;
                }
            }
        }

        public void FolderToDesk(Int32 userId, Int32 memberId)
        {
            var userConfig = GetUser(userId).Config;
            foreach (var desk in userConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);
                if (memberResult != null)
                {
                    memberResult.OutFolder();
                    _deskRepository.Update(desk);
                    break;
                }
            }
        }

        public void FolderToOtherFolder(Int32 userId, Int32 memberId, Int32 folderId)
        {
            var configResult = GetUser(userId).Config;

            var desk = configResult.Desks.FirstOrDefault(c => c.DeskNumber == configResult.DefaultDeskNumber);

            InternalDeskMember(memberId, desk).OutFolder().InFolder(folderId);

            _deskRepository.Update(desk);
        }

        public void DeskToOtherDesk(Int32 userId, Int32 memberId, Int32 deskId)
        {
            var userConfig = GetUser(userId).Config;

            var realDeskId = userConfig.Desks.FirstOrDefault(desk => desk.DeskNumber == deskId).Id;

            foreach (var desk in userConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);
                if (memberResult != null)
                {
                    memberResult.ToOtherDesk(realDeskId);
                    _deskRepository.Update(desk);
                    break;
                }
            }
        }

        public void ModifyFolderInfo(String memberName, String memberIcon, Int32 memberId, Int32 userId)
        {
            var userConfig = GetUser(userId).Config;

            foreach (var desk in userConfig.Desks)
            {
                var memberResult = InternalDeskMember(memberId, desk);
                if (memberResult != null)
                {
                    memberResult.ModifyMemberName(memberName).ModifyMemberIcon(memberIcon);
                    _deskRepository.Update(desk);
                    break;
                }
            }
        }


        public void RemoveMember(Int32 userId, Int32 memberId)
        {
            var userConfig = GetUser(userId).Config;

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
                            desk1.Members.Where(d => d.FolderId == memberId && d.IsDeleted == false).ToList().ForEach(m => m.RemoveMember());
                        }
                    }

                    memberResult.RemoveMember();
                    _deskRepository.Update(desk);
                    break;
                }
            }
        }

        private Member InternalDeskMember(Int32 memberId, Desk desk)
        {
            var memberResult = desk.Members.FirstOrDefault(member => member.Id == memberId && member.IsDeleted == false);
            return memberResult;
        }
    }
}
