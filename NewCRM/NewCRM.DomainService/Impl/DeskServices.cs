using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
