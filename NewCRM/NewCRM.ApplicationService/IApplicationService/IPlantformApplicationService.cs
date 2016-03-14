using System;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;

namespace NewCRM.ApplicationService.IApplicationService
{
    public interface IPlantformApplicationService
    {
        ResponseInformation<UserDto> Login(String userName, String passWord);
    }
}