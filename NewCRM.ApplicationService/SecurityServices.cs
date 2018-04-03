using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<RoleDto> GetRoleAsync(Int32 roleId)
		{
			Parameter.Validate(roleId);

			var result = await _securityContext.GetRoleAsync(roleId);
			if(result == null)
			{
				throw new BusinessException("角色可能已被删除，请刷新后再试");
			}
			var powers = await _securityContext.GetPowersAsync();
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
			Parameter.Validate(roleName);
			var result = _securityContext.GetRoles(roleName, pageIndex, pageSize, out totalCount);
			return result.Select(s => new RoleDto
			{
				Name = s.Name,
				Id = s.Id,
				RoleIdentity = s.RoleIdentity
			}).ToList();
		}

		public async Task<Boolean> CheckPermissionsAsync(Int32 accessAppId, params Int32[] roleIds)
		{
			Parameter.Validate(accessAppId).Validate(roleIds);
			return await _securityContext.CheckPermissionsAsync(accessAppId, roleIds);
		}

		public async Task<Boolean> CheckRoleNameAsync(String name)
		{
			Parameter.Validate(name);
			return await _securityContext.CheckRoleNameAsync(name);
		}

		public async Task RemoveRoleAsync(Int32 roleId)
		{
			Parameter.Validate(roleId);
			await _securityContext.RemoveRoleAsync(roleId);
		}

		public async Task AddNewRoleAsync(RoleDto roleDto)
		{
			Parameter.Validate(roleDto);
			await _securityContext.AddNewRoleAsync(roleDto.ConvertToModel<RoleDto, Role>());
		}

		public async Task ModifyRoleAsync(RoleDto roleDto)
		{
			Parameter.Validate(roleDto);
			await _securityContext.ModifyRoleAsync(roleDto.ConvertToModel<RoleDto, Role>());
		}

		public async Task AddPowerToCurrentRoleAsync(Int32 roleId, IEnumerable<Int32> powerIds)
		{
			Parameter.Validate(roleId);
			await _securityContext.AddPowerToCurrentRoleAsync(roleId, powerIds);
		}

	}
}
