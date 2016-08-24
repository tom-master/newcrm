using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Dto;
using NewCRM.Dto.Dto;

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

        public UserDto GetConfig(Int32 userId)
        {
            return AccountServices.GetUserConfig(userId).ConvertToDto<User, UserDto>();
        }

        public List<UserDto> GetAllUsers(String userName, String userType, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(userName).Validate(pageIndex).Validate(pageSize);

            return SecurityContext.GetAllUsers(userName, userType, pageIndex, pageSize, out totalCount).ConvertDynamicToDtos<UserDto>().ToList();
        }

        public UserDto GetUser(Int32 userId)
        {
            ValidateParameter.Validate(userId);

            return DtoConfiguration.ConvertDynamicToDto<UserDto>(SecurityContext.GetUser(userId));
        }

        public void AddNewUser(UserDto userDto)
        {
            ValidateParameter.Validate(userDto);

            SecurityContext.AddNewUser(userDto.ConvertToModel<UserDto, User>());
        }

        public Boolean ValidSameUserNameExist(String userName)
        {
            ValidateParameter.Validate(userName);

            return SecurityContext.ValidSameUserNameExist(userName);
        }

        public void ModifyUser(UserDto user)
        {
            ValidateParameter.Validate(user);
            SecurityContext.ModifyUser(user.ConvertToModel<UserDto, User>());
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
