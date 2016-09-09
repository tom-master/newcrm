using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Domain.Entities.DomainModel.Security;
using NewCRM.Domain.Entities.DomainSpecification;
using NewCRM.Domain.Entities.DomainSpecification.ConcreteSpecification;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustemException;

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
            return QueryFactory.Create<Role>().Find(SpecificationFactory.Create<Role>()).Select(role => new
            {
                role.Name,
                role.Id
            }).ConvertDynamicToDtos<RoleDto>().ToList();
        }

        public RoleDto GetRole(Int32 roleId)
        {
            ValidateParameter.Validate(roleId);

            var roleResult = QueryFactory.Create<Role>().FindOne(SpecificationFactory.Create<Role>(role => role.Id == roleId));

            if (roleResult == null)
            {
                throw new BusinessException($"角色可能已被删除，请刷新后再试");
            }


            return DtoConfiguration.ConvertDynamicToDto<RoleDto>(new
            {
                roleResult.Name,
                roleResult.RoleIdentity,
                roleResult.Remark,
                Powers = roleResult.Powers.Select(s => new { Id = s.PowerId })
            });
        }

        public void AddPowerToCurrentRole(Int32 roleId, IEnumerable<Int32> powerIds)
        {
            ValidateParameter.Validate(roleId).Validate(powerIds);

            SecurityContext.RoleServices.AddPowerToCurrentRole(roleId, powerIds);
        }

        public List<RoleDto> GetAllRoles(String roleName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(roleName).Validate(pageIndex).Validate(pageIndex);

            var roleSpecification = SpecificationFactory.Create<Role>();

            if ((roleName + "").Length > 0)
            {
                roleSpecification.And(role => role.Name.Contains(roleName));
            }

            return QueryFactory.Create<Role>().PageBy(roleSpecification, pageIndex, pageSize, out totalCount).Select(s => new
            {
                s.Name,
                s.Id,
                s.RoleIdentity
            }).ConvertDynamicToDtos<RoleDto>().ToList();

        }

        #endregion

        #region Power

        public List<PowerDto> GetAllPowers()
        {
            return QueryFactory.Create<Power>().Find(SpecificationFactory.Create<Power>()).Select(power => new
            {
                power.Name,
                power.Id,
                power.PowerIdentity
            }).ConvertDynamicToDtos<PowerDto>().ToList();
        }

        public void AddNewPower(PowerDto power)
        {
            ValidateParameter.Validate(power);
            SecurityContext.PowerServices.AddNewPower(power.ConvertToModel<PowerDto, Power>());
        }

        public PowerDto GetPower(Int32 powerId)
        {
            ValidateParameter.Validate(powerId);

            var powerResult = QueryFactory.Create<Power>().FindOne(SpecificationFactory.Create<Power>());

            if (powerResult == null)
            {
                throw new BusinessException($"该权限可能已被删除，请刷新后再试");
            }

            return powerResult.ConvertToDto<Power, PowerDto>();
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


            var powerSpecification = SpecificationFactory.Create<Power>();

            if ((powerName + "").Length > 0)
            {
                powerSpecification.And(power => power.Name.Contains(powerName));
            }

            return QueryFactory.Create<Power>().PageBy(powerSpecification, pageIndex, pageSize, out totalCount).ConvertToDtos<Power, PowerDto>().ToList();
        }

        #endregion
    }
}
