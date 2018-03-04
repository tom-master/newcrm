using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain.Entitys.Security;
using NewCRM.Domain.Services;
using NewCRM.Domain.Services.Interface;
using NewCRM.Dto;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Application.Services
{
    public class SecurityServices : BaseServiceContext, ISecurityServices
    {
        private readonly ISecurityContext _securityContext;

        public SecurityServices(ISecurityContext securityContext) => _securityContext = securityContext;

        public RoleDto GetRole(Int32 roleId)
        {
            Parameter.Validate(roleId);

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
            Parameter.Validate(roleName).Validate(pageIndex).Validate(pageIndex);
            var result = _securityContext.GetRoles(roleName, pageIndex, pageSize, out totalCount);
            return result.Select(s => new RoleDto
            {
                Name = s.Name,
                Id = s.Id,
                RoleIdentity = s.RoleIdentity
            }).ToList();
        }

        public Boolean CheckPermissions(Int32 accessAppId, params Int32[] roleIds)
        {
            Parameter.Validate(accessAppId).Validate(roleIds);
            return _securityContext.CheckPermissions(accessAppId, roleIds);
        }

        public bool CheckRoleName(string name)
        {
            Parameter.Validate(name);
            return _securityContext.CheckRoleName(name);
        }

        public void RemoveRole(Int32 roleId)
        {
            Parameter.Validate(roleId);
            _securityContext.RemoveRole(roleId);
        }

        public void AddNewRole(RoleDto roleDto)
        {
            Parameter.Validate(roleDto);
            _securityContext.AddNewRole(roleDto.ConvertToModel<RoleDto, Role>());
        }

        public void ModifyRole(RoleDto roleDto)
        {
            Parameter.Validate(roleDto);
            _securityContext.ModifyRole(roleDto.ConvertToModel<RoleDto, Role>());
        }

        public void AddPowerToCurrentRole(Int32 roleId, IEnumerable<Int32> powerIds)
        {
            Parameter.Validate(roleId);
            _securityContext.AddPowerToCurrentRole(roleId, powerIds);
        }

    }
}
