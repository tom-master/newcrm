using System;

namespace NewCRM.Domain.Services
{
    public interface IAccountServices
    {
        Boolean Validate(String userName, String password);

        void Logout(Int32 userId);

        void Disable(Int32 userId);

        void Enable(Int32 userId);

    }
}
