using System;
using NewCRM.ApplicationService.IApplicationService;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.DomainService;
using NewCRM.DomainService.Impl;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomHelper;

namespace NewCRM.ApplicationService
{
    public class PlantformApplicationService : IPlantformApplicationService
    {
        private readonly IPlatformDomainService _platformDomainService = new PlatformDomainService();

        public ResponseInformation<UserDto> Login(String userName, String passWord)
        {
            Parameter.Vaildate(userName, passWord);
            var userResult = _platformDomainService.VaildateUser(userName, passWord);
            if (userResult != null)
            {
                return new ResponseInformation<UserDto>(ResponseType.Success, userResult.ConvertToDto<User, UserDto>());
            }
            return new ResponseInformation<UserDto>(ResponseType.PasswordInvalid | ResponseType.QueryNull);
        }
    }
}