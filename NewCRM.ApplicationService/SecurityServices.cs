using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Factory.DomainSpecification;
using NewCRM.Domain.Repositories.IRepository.Security;
using NewCRM.Dto;
using NewCRM.Infrastructure.CommonTools.CustomException;

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

            var filter = FilterFactory.Create<Account>(account => account.Roles.Any(ro => ro.RoleId == roleId));
            if (DatabaseQuery.Find(filter).Any())
            {
                throw new BusinessException("当前角色已绑定了账户，无法删除");
            }

            var result = DatabaseQuery.FindOne(FilterFactory.Create<Role>(role => role.Id == roleId));
            if (result == null)
            {
                throw new BusinessException("该角色可能已不存在，请刷新后再试");
            }

            if (result.Powers.Any())
            {
                result.Powers.ToList().ForEach(rolePower => rolePower.Remove());
            }

            result.Remove();
            _roleRepository.Update(result);

            UnitOfWork.Commit();
        }

        public void AddNewRole(RoleDto roleDto)
        {
            ValidateParameter.Validate(roleDto);

            var filter = FilterFactory.Create<Role>(role => role.Name.ToLower() == roleDto.Name.ToLower() || role.RoleIdentity.ToLower() == roleDto.RoleIdentity.ToLower());
            var result = DatabaseQuery.FindOne(filter);
            if (result != null)
            {
                throw new BusinessException($@"角色:{roleDto.Name} 已经存在");
            }
            var roleModel = roleDto.ConvertToModel<RoleDto, Role>();
            _roleRepository.Add(roleModel);

            UnitOfWork.Commit();
        }

        public void ModifyRole(RoleDto roleDto)
        {
            ValidateParameter.Validate(roleDto);

            var filter = FilterFactory.Create<Role>(role => role.Name.ToLower() == roleDto.Name.ToLower() || role.RoleIdentity.ToLower() == roleDto.RoleIdentity.ToLower());
            var result = DatabaseQuery.FindOne(filter);
            if (result != null)
            {
                throw new BusinessException("已经存在一个相同名称的角色");
            }

            var roleModel = roleDto.ConvertToModel<RoleDto, Role>();
            var roleResult = DatabaseQuery.FindOne(FilterFactory.Create<Role>(internalRole => internalRole.Id == roleModel.Id));

            if (roleResult == null)
            {
                throw new BusinessException("该角色可能已被删除，请刷新后再试");
            }

            roleResult.ModifyRoleName(roleModel.Name).ModifyRoleIdentity(roleModel.RoleIdentity);
            _roleRepository.Update(roleResult);

            UnitOfWork.Commit();
        }

        public void AddPowerToCurrentRole(Int32 roleId, IEnumerable<Int32> powerIds)
        {
            ValidateParameter.Validate(roleId);
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
