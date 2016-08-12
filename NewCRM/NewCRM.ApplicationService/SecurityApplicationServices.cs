using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Domain.Entities.DomainModel.Security;
using NewCRM.Dto;
using NewCRM.Dto.Dto;

namespace NewCRM.Application.Services
{
    [Export(typeof(ISecurityApplicationServices))]
    internal class SecurityApplicationServices : BaseApplicationServices, ISecurityApplicationServices
    {
        public List<RoleDto> GetAllRoles(String roleName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(roleName).Validate(pageIndex).Validate(pageIndex);

            return SecurityServices.GetAllRoles(roleName, pageIndex, pageSize, out totalCount).ConvertDynamicToDtos<RoleDto>().ToList();
        }

        public void RemoveRole(Int32 roleId)
        {
            ValidateParameter.Validate(roleId);

            SecurityServices.RemoveRole(roleId);
        }

        public List<AppDto> GetSystemRoleApps()
        {
            return SecurityServices.GetSystemRoleApps().ConvertDynamicToDtos<AppDto>().ToList();
        }

        public RoleDto GetRoleInfo(Int32 roleId)
        {
            ValidateParameter.Validate(roleId);

            return SecurityServices.GetRoleInfo(roleId).ConvertToDto<Role, RoleDto>();
        }

        public void AddNewPower(PowerDto power)
        {
            ValidateParameter.Validate(power);
            SecurityServices.AddNewPower(power.ConvertToModel<PowerDto, Power>());
        }

        public List<PowerDto> GetAllPowers(String powerName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(powerName).Validate(pageIndex).Validate(pageSize);
            return
                SecurityServices.GetAllPowers(powerName, pageIndex, pageSize, out totalCount)
                    .ConvertToDtos<Power, PowerDto>().ToList();
        }

        public PowerDto GetPower(Int32 powerId)
        {
            ValidateParameter.Validate(powerId);

            return SecurityServices.GetPower(powerId).ConvertToDto<Power, PowerDto>();
        }

        public void ModifyPower(PowerDto power)
        {
            ValidateParameter.Validate(power);

            SecurityServices.ModifyPower(power.ConvertToModel<PowerDto, Power>());
        }

        public void RemovePower(Int32 powerId)
        {
            ValidateParameter.Validate(powerId);

            SecurityServices.RemovePower(powerId);
        }
    }
}
