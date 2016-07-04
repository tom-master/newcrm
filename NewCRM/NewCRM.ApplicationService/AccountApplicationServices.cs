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
    internal class AccountApplicationServices : IAccountApplicationServices
    {
        [Import]
        private IAccountServices _accountServices;

        private readonly Parameter _validateParameter = new Parameter();


        public Int32 Login(String userName, String password)
        {
            _validateParameter.Validate(userName).Validate(password);
            return _accountServices.Validate(userName, password);

        }

        public UserDto GetUserConfig(Int32 userId)
        {
            return _accountServices.GetUserConfig(userId).ConvertToDto<User, UserDto>();
        }

        public void Logout(Int32 userId)
        {
            _validateParameter.Validate(userId);
            _accountServices.Logout(userId);
        }

        public void Enable(Int32 userId)
        {
            _validateParameter.Validate(userId);
            _accountServices.Enable(userId);
        }

        public void Disable(Int32 userId)
        {
            _validateParameter.Validate(userId);
            _accountServices.Disable(userId);
        }
    }
}
