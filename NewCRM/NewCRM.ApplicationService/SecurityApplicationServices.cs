using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Application.Interface;
using NewCRM.Application.Services.Services;
using NewCRM.Domain.DomainSpecification;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Application.Services
{
    [Export(typeof(ISecurityApplicationServices))]
    internal class SecurityApplicationServices : BaseService, ISecurityApplicationServices
    {
        #region Role

        public void RemoveRole(Int32 roleId)
        {
            ValidateParameter.Validate(roleId);

            var roleResult = QueryFactory.Create<Role>().FindOne(SpecificationFactory.Create<Role>(role => role.Id == roleId));

            if (roleResult == null)
            {
                throw new BusinessException("该角色可能已不存在，请刷新后再试");
            }

            if (roleResult.Powers.Any())
            {
                roleResult.Powers.ToList().ForEach(rolePower => rolePower.Remove());
            }

            roleResult.Remove();

            Repository.Create<Role>().Update(roleResult);

            UnitOfWork.Commit();
        }

        public void AddNewRole(RoleDto roleDto)
        {
            ValidateParameter.Validate(roleDto);

            var role = roleDto.ConvertToModel<RoleDto, Role>();

            Repository.Create<Role>().Add(role);

            UnitOfWork.Commit();
        }

        public void ModifyRole(RoleDto roleDto)
        {
            ValidateParameter.Validate(roleDto);

            var role = roleDto.ConvertToModel<RoleDto, Role>();

            var roleResult = QueryFactory.Create<Role>().FindOne(SpecificationFactory.Create<Role>(internalRole => internalRole.Id == role.Id));

            if (roleResult == null)
            {
                throw new BusinessException("该角色可能已被删除，请刷新后再试");
            }

            roleResult.ModifyRoleName(role.Name).ModifyRoleIdentity(role.RoleIdentity);

            Repository.Create<Role>().Update(roleResult);

            UnitOfWork.Commit();
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

            var roleResult = QueryFactory.Create<Role>().FindOne(SpecificationFactory.Create<Role>(role => role.Id == roleId));

            if (roleResult == null)
            {
                throw new BusinessException("该角色可能已被删除，请刷新后再试");
            }

            roleResult.Powers.ToList().ForEach(f => f.Remove());

            roleResult.AddPower(powerIds.ToArray());

            Repository.Create<Role>().Update(roleResult);

            UnitOfWork.Commit();
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

        public void AddNewPower(PowerDto powerDto)
        {
            ValidateParameter.Validate(powerDto);

            var power = powerDto.ConvertToModel<PowerDto, Power>();

            Repository.Create<Power>().Add(power);

            UnitOfWork.Commit();
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

        public void ModifyPower(PowerDto powerDto)
        {
            ValidateParameter.Validate(powerDto);

            var power = powerDto.ConvertToModel<PowerDto, Power>();

            var powerResult = QueryFactory.Create<Power>().FindOne(SpecificationFactory.Create<Power>(p => p.Id == power.Id));

            if (powerResult == null)
            {
                throw new BusinessException("该权限可能已被删除，请刷新后再试");
            }

            powerResult.ModifyPowerIdentity(power.PowerIdentity);

            powerResult.ModifyPowerName(power.Name);

            Repository.Create<Power>().Update(powerResult);

            UnitOfWork.Commit();
        }

        public void RemovePower(Int32 powerId)
        {
            ValidateParameter.Validate(powerId);

            var powerResult = QueryFactory.Create<Power>().FindOne(SpecificationFactory.Create<Power>(power => power.Id == powerId));

            if (powerResult == null)
            {
                throw new BusinessException("该权限可能已被删除，请刷新后再试");
            }

            powerResult.Remove();

            Repository.Create<Power>().Update(powerResult);

            UnitOfWork.Commit();
        }

        public List<PowerDto> GetAllPowers(String powerName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(powerName).Validate(pageIndex).Validate(pageSize);

            var powerSpecification = SpecificationFactory.Create<Power>();

            if ((powerName + "").Length > 0)
            {
                powerSpecification.And(power => power.Name.Contains(powerName));
            }

            return QueryFactory.Create<Power>().PageBy(powerSpecification, pageIndex, pageSize, out totalCount)
                .ConvertToDtos<Power, PowerDto>().ToList();

        }

        #endregion

        public Boolean CheckPermissions(Int32 userId, params Int32[] roles)
        {
            throw new NotImplementedException();
        }
    }
}
