using System;
using System.ComponentModel.Composition;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Services;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomHelper;

namespace NewCRM.Application.Services
{
    [Export(typeof(IAccountApplicationServices))]
    internal class AccountApplicationServices : BaseApplicationServices, IAccountApplicationServices
    {
        public UserDto Login(String userName, String password)
        {
            ValidateParameter.Validate(userName).Validate(password);
            return AccountServices.Validate(userName, password).ConvertToDto<User, UserDto>();

        }

        public UserDto GetUserConfig(Int32 userId)
        {
            return AccountServices.GetUserConfig(userId).ConvertToDto<User, UserDto>();
        }

        public void Logout(Int32 userId)
        {
            ValidateParameter.Validate(userId);
            AccountServices.Logout(userId);
        }

        public void Enable(Int32 userId)
        {
            ValidateParameter.Validate(userId);
            AccountServices.Enable(userId);
        }

        public void Disable(Int32 userId)
        {
            ValidateParameter.Validate(userId);
            AccountServices.Disable(userId);
        }
    }
}
