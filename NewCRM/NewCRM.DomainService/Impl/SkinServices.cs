using System;
using System.ComponentModel.Composition;

namespace NewCRM.Domain.Services.Impl
{
    [Export(typeof(ISkinServices))]
    internal class SkinServices : BaseService, ISkinServices
    {
        public void ModifySkin(Int32 userId, String newSkin)
        {
            var userResult = GetUserInfoService(userId);

            userResult.Config.ModifySkin(newSkin);

            UserRepository.Update(userResult);
        }
    }
}
