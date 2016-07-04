using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entities.Repositories.IRepository.Account;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.Impl
{
    [Export(typeof(IUserSettingServices))]
    public class UserSettingServices : IUserSettingServices
    {
        [Import]
        private IUserRepository _userRepository;

        public void ModifyDefaultShowDesk(Int32 userId, Int32 newDefaultDeskNumber)
        {
            var userResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);

            userResult.Config.ModifyDefaultDesk(newDefaultDeskNumber);

            _userRepository.Update(userResult);
        }

        public void ModifyAppDirection(Int32 userId, String direction)
        {
            var userResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);

            if (direction.ToLower() == "x")
            {
                userResult.Config.ModifyAppDirectionToX();
            }
            else if (direction.ToLower() == "y")
            {
                userResult.Config.ModifyAppDirectionToY();
            }
            else
            {
                throw new BusinessException($"未能识别的App排列方向:{direction.ToLower()}");
            }


            _userRepository.Update(userResult);
        }
    }
}
