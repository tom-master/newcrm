using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.Security;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.Impl
{

    [Export(typeof(ISecurityServices))]
    internal sealed class SecurityServices : BaseService, ISecurityServices
    {
        public List<dynamic> GetAllRoles(String roleName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            var roles = RoleRepository.Entities;

            if ((roleName + "").Length > 0)
            {
                roles = roles.Where(role => role.Name.Contains(roleName));
            }

            totalCount = roles.Count();

            return roles.OrderByDescending(o => o.AddTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(s => new
            {
                s.Name,
                s.Id
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
        }

        public List<dynamic> GetSystemRoleApps()
        {
            var apps = AppRepository.Entities.Where(app => app.IsSystem);

            return apps.OrderByDescending(o => o.AddTime).Select(app => new
            {
                app.Id,
                app.Name,
                app.IconUrl
            }).ToList<dynamic>();
        }

        public dynamic GetRoleInfo(Int32 roleId)
        {
            var roleResult = RoleRepository.Entities.FirstOrDefault(role => role.Id == roleId);

            if (roleResult == null)
            {
                throw new BusinessException($"角色可能已被删除，请刷新后再试");
            }

            var rolePowerIds = roleResult.Powers.Select(rolePower => rolePower.PowerId);

            return new
            {
                Name = roleResult.Name,
                Powers = AppRepository.Entities.Where(app => rolePowerIds.Contains(app.Id)).Select(s => new { s.Id, s.Name, s.IconUrl })
            };
        }
    }
}
