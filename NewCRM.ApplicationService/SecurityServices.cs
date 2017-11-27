using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Services.Interface;
using NewCRM.Dto;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Application.Services
{
    public class SecurityServices : BaseServiceContext, ISecurityServices
    {
        private readonly ISecurityContext _securityContext;

        public SecurityServices(ISecurityContext securityContext)
        {
            _securityContext = securityContext;
        }

        #region Role

        public RoleDto GetRole(Int32 roleId)
        {
            ValidateParameter.Validate(roleId);

            var result = _securityContext.GetRole(roleId);
            if(result == null)
            {
                throw new BusinessException("角色可能已被删除，请刷新后再试");
            }
            var powers = _securityContext.GetPowers();
            return new RoleDto
            {
                Name = result.Name,
                RoleIdentity = result.RoleIdentity,
                Remark = result.Remark,
                Powers = powers.Where(w => w.RoleId == result.Id).Select(s => new PowerDto { Id = s.AppId }).ToList()
            };
        }

        public List<RoleDto> GetRoles(String roleName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            ValidateParameter.Validate(roleName).Validate(pageIndex).Validate(pageIndex);
            var result = _securityContext.GetRoles(roleName, pageIndex, pageSize, out totalCount);
            return result.Select(s => new RoleDto
            {
                Name = s.Name,
                Id = s.Id,
                RoleIdentity = s.RoleIdentity
            }).ToList();
        }

        public void RemoveRole(Int32 roleId)
        {
            ValidateParameter.Validate(roleId);
            _securityContext.RemoveRole(roleId);
        }

        public void AddNewRole(RoleDto roleDto)
        {
            ValidateParameter.Validate(roleDto);

            var filter = FilterFactory.Create<Role>(role => role.Name.ToLower() == roleDto.Name.ToLower() || role.RoleIdentity.ToLower() == roleDto.RoleIdentity.ToLower());
            var result = DatabaseQuery.FindOne(filter);
            if(result != null)
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
            if(result != null)
            {
                throw new BusinessException("已经存在一个相同名称的角色");
            }

            var roleModel = roleDto.ConvertToModel<RoleDto, Role>();
            var roleResult = DatabaseQuery.FindOne(FilterFactory.Create<Role>(internalRole => internalRole.Id == roleModel.Id));

            if(roleResult == null)
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

            if(roleResult == null)
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
