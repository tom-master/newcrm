using System;
using NewCRM.Dto.Dto;

namespace NewCRM.Application.Services.IApplicationService
{
    public interface IAccountApplicationServices
    {
        UserDto Login(String userName, String password);

        void Logout(Int32 userId);

        void Enable(Int32 userId);

        void Disable(Int32 userId);

        UserDto GetUserConfig(Int32 userId);

    }
}
