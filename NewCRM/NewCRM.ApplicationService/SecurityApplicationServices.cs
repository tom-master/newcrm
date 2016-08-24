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
        #region Role

        public void RemoveRole(Int32 roleId)
        {
            ValidateParameter.Validate(roleId);

            SecurityContext.RoleServices.RemoveRole(roleId);
        }

        public void AddNewRole(RoleDto role)
        {

            ValidateParameter.Validate(role);

            SecurityContext.RoleServices.AddNewRole(role.ConvertToModel<RoleDto, Role>());
        }

        public void ModifyRole(RoleDto role)
        {
            ValidateParameter.Validate(role);

            SecurityContext.RoleServices.ModifyRole(role.ConvertToModel<RoleDto, Role>());
        }

        public List<RoleDto> GetAllRoles()
        {
            return SecurityContext.RoleServices.GetAllRoles().ConvertDynamicToDtos<RoleDto>().ToList();
        }

        public RoleDto GetRole(Int32 roleId)
        {
            ValidateParameter.Validate(roleId);

            return DtoConfiguration.ConvertDynamicToDto<RoleDto>(SecurityContext.RoleServices.GetRole(roleId));
        }

        public void AddPowerToCurrentRole(Int32 roleId, IEnumerable<Int32> powerIds)
        {
            ValidateParameter.Validate(roleId).Validate(powerIds);

            SecurityContext.RoleServices.AddPowerToCurrentRole(roleId, powerIds);
        }

        public List<RoleDto> GetAllRoles(String roleName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(roleName).Validate(pageIndex).Validate(pageIndex);

            return SecurityContext.RoleServices.GetAllRoles(roleName, pageIndex, pageSize, out totalCount).ConvertDynamicToDtos<RoleDto>().ToList();
        }

        #endregion

        #region Power

        public List<PowerDto> GetAllPowers()
        {
            return SecurityContext.PowerServices.GetAllPowers().ConvertDynamicToDtos<PowerDto>().ToList();
        }

        public void AddNewPower(PowerDto power)
        {
            ValidateParameter.Validate(power);
            SecurityContext.PowerServices.AddNewPower(power.ConvertToModel<PowerDto, Power>());
        }

        public PowerDto GetPower(Int32 powerId)
        {
            ValidateParameter.Validate(powerId);

            return SecurityContext.PowerServices.GetPower(powerId).ConvertToDto<Power, PowerDto>();
        }

        public void ModifyPower(PowerDto power)
        {
            ValidateParameter.Validate(power);

            SecurityContext.PowerServices.ModifyPower(power.ConvertToModel<PowerDto, Power>());
        }

        public void RemovePower(Int32 powerId)
        {
            ValidateParameter.Validate(powerId);

            SecurityContext.PowerServices.RemovePower(powerId);
        }

        public List<PowerDto> GetAllPowers(String powerName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(powerName).Validate(pageIndex).Validate(pageSize);
            return SecurityContext.PowerServices.GetAllPowers(powerName, pageIndex, pageSize, out totalCount).ConvertToDtos<Power, PowerDto>().ToList();
        }

        #endregion
    }
}
