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
    public class SkinServices : BaseService, ISkinServices
    {
        public void ModifySkin(Int32 userId, String newSkin)
        {
            var userResult = GetUser(userId);

            userResult.Config.ModifySkin(newSkin);

            UserRepository.Update(userResult);
        }
    }
}
