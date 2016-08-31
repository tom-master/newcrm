using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.Security;
using NewCRM.Infrastructure.CommonTools.CustemException;
using NewCRM.Infrastructure.CommonTools.CustomExtension;

namespace NewCRM.Domain.Services.Impl
{
    [Export(typeof(IRoleServices))]
    internal sealed class RoleServices : BaseService, IRoleServices
    {
        public List<dynamic> GetAllRoles(String roleName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            var roles = RoleRepository.Entities;

            if ((roleName + "").Length > 0)
            {
                roles = roles.Where(role => role.Name.Contains(roleName));
            }

            totalCount = roles.Count();

            return roles.PageBy(pageIndex, pageSize, d => d.AddTime).Select(s => new
            {
                s.Name,
                s.Id,
                s.RoleIdentity
            }).ToList<dynamic>();
        }

        public void RemoveRole(Int32 roleId)
        {
            var roleResult = RoleRepository.Entities.FirstOrDefault(role => role.Id == roleId);

            if (roleResult == null)
            {
                throw new BusinessException("该角色可能已不存在，请刷新后再试");
            }

            if (roleResult.Powers.Any())
            {
                roleResult.Powers.ToList().ForEach(rolePower => rolePower.Remove());
            }

            roleResult.Remove();

            RoleRepository.Update(roleResult);
        }

        public dynamic GetRole(Int32 roleId)
        {
            var roleResult = RoleRepository.Entities.FirstOrDefault(role => role.Id == roleId);

            if (roleResult == null)
            {
                throw new BusinessException($"角色可能已被删除，请刷新后再试");
            }

            return new
            {
                roleResult.Name,
                roleResult.RoleIdentity,
                roleResult.Remark,
                Powers = roleResult.Powers.Select(s => new { Id = s.PowerId })
            };
        }

        public void AddNewRole(Role role)
        {
            RoleRepository.Add(role);
        }

        public void ModifyRole(Role role)
        {
            var roleResult = RoleRepository.Entities.FirstOrDefault(internalRole => internalRole.Id == role.Id);

            if (roleResult == null)
            {
                throw new BusinessException("该角色可能已被删除，请刷新后再试");
            }

            roleResult.ModifyRoleName(role.Name).ModifyRoleIdentity(role.RoleIdentity);

            RoleRepository.Update(roleResult);
        }

        public void AddPowerToCurrentRole(Int32 roleId, IEnumerable<Int32> powerIds)
        {
            var roleResult = RoleRepository.Entities.FirstOrDefault(role => role.Id == roleId);
            if (roleResult == null)
            {
                throw new BusinessException("该角色可能已被删除，请刷新后再试");
            }

            roleResult.Powers.ToList().ForEach(f => f.Remove());

            roleResult.AddPower(powerIds.ToArray());

            RoleRepository.Update(roleResult);
        }

        public List<dynamic> GetAllRoles()
        {
            return RoleRepository.Entities.Select(role => new
            {
                role.Name,
                role.Id
            }).ToList<dynamic>();
        }
    }
}
