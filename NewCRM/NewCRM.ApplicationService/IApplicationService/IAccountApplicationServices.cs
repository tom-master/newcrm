using System;

namespace NewCRM.Application.Services.IApplicationService
{
    public interface IAccountApplicationServices
    {
        void Login(String userName, String password);

        void Logout(Int32 userId);

        void Enable(Int32 userId);

        void Disable(Int32 userId);

    }
}
