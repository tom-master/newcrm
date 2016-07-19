using System;
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
    public class DeskServices : IDeskServices
    {
        [Import]
        private IUserRepository _userRepository;

        [Import]
        private IDeskRepository _deskRepository;


        public void ModifyDefaultShowDesk(Int32 userId, Int32 newDefaultDeskNumber)
        {
            var userResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);

            userResult.Config.ModifyDefaultDesk(newDefaultDeskNumber);

            _userRepository.Update(userResult);
        }

        public void ModifyDockPosition(Int32 userId, Int32 defaultDeskNumber, String newPosition)
        {
            var userResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);

            DockPostion dockPostion;
            if (Enum.TryParse(newPosition, true, out dockPostion))
            {
                if (dockPostion == DockPostion.None)
                {
                    var deskResult = _deskRepository.Entities.FirstOrDefault(desk => desk.DeskNumber == userResult.Config.DefaultDeskNumber);

                    var dockMembers = deskResult.Members.Where(member => member.IsOnDock == true);

                    if (dockMembers.Any())
                    {
                        dockMembers.ToList().ForEach(
                        f =>
                        {
                            f.MoveOutDock();
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

            _userRepository.Update(userResult);
        }

        public Member GetMember(Int32 userId, Int32 memberId, Boolean isFolder = default(Boolean))
        {
            var configResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId).Config;

            var members = configResult.Desks.FirstOrDefault(c => c.DeskNumber == configResult.DefaultDeskNumber).Members;

            return isFolder ? members.FirstOrDefault(member => member.Id == memberId && member.MemberType == MemberType.Folder) : members.FirstOrDefault(member => member.AppId == memberId && member.MemberType == MemberType.App);
        }

        public void MemberInDock(Int32 userId, Int32 memberId)
        {
            var configResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId).Config;

            var desk = configResult.Desks.FirstOrDefault(c => c.DeskNumber == configResult.DefaultDeskNumber);

            desk.Members.FirstOrDefault(member => member.Id == memberId).MoveInDock();

            _deskRepository.Update(desk);
        }

        public void MemberOutDock(Int32 userId, Int32 memberId)
        {
            var configResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId).Config;

            var desk = configResult.Desks.FirstOrDefault(c => c.DeskNumber == configResult.DefaultDeskNumber);

            desk.Members.FirstOrDefault(member => member.Id == memberId).MoveOutDock();

            _deskRepository.Update(desk);
        }

        public void DockToFolder(Int32 userId, Int32 memberId, Int32 folderId)
        {
            var configResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId).Config;

            var desk = configResult.Desks.FirstOrDefault(c => c.DeskNumber == configResult.DefaultDeskNumber);

            desk.Members.FirstOrDefault(member => member.Id == memberId).MoveOutDock().MemberInFolder(folderId);

            _deskRepository.Update(desk);
        }
    }
}
