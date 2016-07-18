using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entities.Repositories.IRepository.Account;

namespace NewCRM.Domain.Services.Impl
{
    [Export(typeof(ISkinServices))]
    public class SkinServices : ISkinServices
    {
        [Import]
        private IUserRepository _userRepository;


        public void ModifySkin(Int32 userId, String newSkin)
        {
            var userResult = _userRepository.Entities.FirstOrDefault(user => user.Id == userId);

            userResult.Config.ModifySkin(newSkin);

            _userRepository.Update(userResult);
        }
    }
}
