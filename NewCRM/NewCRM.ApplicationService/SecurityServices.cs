using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Factory.DomainSpecification;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Domain.Repositories.IRepository.Security;

namespace NewCRM.Application.Services
{
    public class SecurityServices : BaseServiceContext, ISecurityServices
    {
        private readonly IRoleRepository _roleRepository;

        public SecurityServices(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }


        #region Role

        public List<RoleDto> GetAllRoles()
        {
            return DatabaseQuery.Find(FilterFactory.Create<Role>()).Select(role => new
            {
                role.Name,
                role.Id
            }).ConvertDynamicToDtos<RoleDto>().ToList();
        }

        public RoleDto GetRole(Int32 roleId)
        {
            ValidateParameter.Validate(roleId);

            var roleResult = DatabaseQuery.FindOne(FilterFactory.Create<Role>(role => role.Id == roleId));
            if (roleResult == null)
            {
                throw new BusinessException("角色可能已被删除，请刷新后再试");
            }

            return DtoConfiguration.ConvertDynamicToDto<RoleDto>(new
            {
                roleResult.Name,
                roleResult.RoleIdentity,
                roleResult.Remark,
                Powers = roleResult.Powers.Select(s => new { Id = s.AppId })
            });
        }

        public List<RoleDto> GetAllRoles(String roleName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(roleName).Validate(pageIndex).Validate(pageIndex);

            var filter = FilterFactory.Create<Role>();
            if (!String.IsNullOrEmpty(roleName))
            {
                filter.And(role => role.Name.Contains(roleName));
            }

            return DatabaseQuery.PageBy(filter, pageIndex, pageSize, out totalCount).Select(s => new
            {
                s.Name,
                s.Id,
                s.RoleIdentity
            }).ConvertDynamicToDtos<RoleDto>().ToList();
        }


        public void RemoveRole(Int32 roleId)
        {
            ValidateParameter.Validate(roleId);

            var roleResult = DatabaseQuery.FindOne(FilterFactory.Create<Role>(role => role.Id == roleId));
            if (roleResult == null)
            {
                throw new BusinessException("该角色可能已不存在，请刷新后再试");
            }

            if (roleResult.Powers.Any())
            {
                roleResult.Powers.ToList().ForEach(rolePower => rolePower.Remove());
            }

            roleResult.Remove();
            _roleRepository.Update(roleResult);

            UnitOfWork.Commit();
        }

        public void AddNewRole(RoleDto roleDto)
        {
            ValidateParameter.Validate(roleDto);

            var role = roleDto.ConvertToModel<RoleDto, Role>();
            _roleRepository.Add(role);

            UnitOfWork.Commit();
        }

        public void ModifyRole(RoleDto roleDto)
        {
            ValidateParameter.Validate(roleDto);

            var role = roleDto.ConvertToModel<RoleDto, Role>();
            var roleResult = DatabaseQuery.FindOne(FilterFactory.Create<Role>(internalRole => internalRole.Id == role.Id));

            if (roleResult == null)
            {
                throw new BusinessException("该角色可能已被删除，请刷新后再试");
            }

            roleResult.ModifyRoleName(role.Name).ModifyRoleIdentity(role.RoleIdentity);
            _roleRepository.Update(roleResult);

            UnitOfWork.Commit();
        }

        public void AddPowerToCurrentRole(Int32 roleId, IEnumerable<Int32> powerIds)
        {
            ValidateParameter.Validate(roleId).Validate(powerIds);
            var roleResult = DatabaseQuery.FindOne(FilterFactory.Create<Role>(role => role.Id == roleId));

            if (roleResult == null)
            {
                throw new BusinessException("该角色可能已被删除，请刷新后再试");
            }

            roleResult.Powers.ToList().ForEach(f => f.Remove());
            roleResult.AddPower(powerIds.ToArray());
            _roleRepository.Update(roleResult);

            UnitOfWork.Commit();
        }

        #endregion

        public Boolean CheckPermissions(Int32 accessAppId, params Int32[] roleIds)
        {
            var roles = DatabaseQuery.Find(FilterFactory.Create<Role>(role => roleIds.Contains(role.Id))).ToArray();
            return roles.Any(role => role.CheckPower(accessAppId));
        }
    }
}
