using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
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

            return roles.Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(s => new
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
    }
}
