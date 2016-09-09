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
        public void RemoveRole(Int32 roleId)
        {
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

            RoleRepository.Update(roleResult);
        }

        public void AddNewRole(Role role)
        {
            RoleRepository.Add(role);
        }

        public void ModifyRole(Role role)
        {

            var roleResult = QueryFactory.Create<Role>().FindOne(SpecificationFactory.Create<Role>(internalRole => internalRole.Id == role.Id));

            if (roleResult == null)
            {
                throw new BusinessException("该角色可能已被删除，请刷新后再试");
            }

            roleResult.ModifyRoleName(role.Name).ModifyRoleIdentity(role.RoleIdentity);

            RoleRepository.Update(roleResult);
        }

        public void AddPowerToCurrentRole(Int32 roleId, IEnumerable<Int32> powerIds)
        {
            var roleResult = QueryFactory.Create<Role>().FindOne(SpecificationFactory.Create<Role>(role => role.Id == roleId));

            if (roleResult == null)
            {
                throw new BusinessException("该角色可能已被删除，请刷新后再试");
            }

            roleResult.Powers.ToList().ForEach(f => f.Remove());

            roleResult.AddPower(powerIds.ToArray());

            RoleRepository.Update(roleResult);
        }

    }
}
