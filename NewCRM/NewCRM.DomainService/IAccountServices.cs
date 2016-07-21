using System;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.System;

namespace NewCRM.Domain.Services
{
    public interface IAccountServices
    {
        User Validate(String userName, String password);

        void Logout(Int32 userId);

        void Disable(Int32 userId);

        void Enable(Int32 userId);


        User GetUserConfig(Int32 userId);

    }
}
