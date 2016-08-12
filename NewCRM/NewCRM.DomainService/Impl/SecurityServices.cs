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

        public void AddNewPower(Power power)
        {
            PowerRepository.Add(power);
        }

        public List<Power> GetAllPowers(String powerName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            var powers = PowerRepository.Entities;

            if ((powerName + "").Length > 0)
            {
                powers = powers.Where(power => power.Name.Contains(powerName));
            }

            totalCount = powers.Count();

            return powers.OrderByDescending(o => o.AddTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

        }

        public Power GetPower(Int32 powerId)
        {
            var powerResult = PowerRepository.Entities.FirstOrDefault(power => power.Id == powerId);

            if (powerResult == null)
            {
                throw new BusinessException($"该权限可能已被删除，请刷新后再试");
            }
            return powerResult;
        }

        public void ModifyPower(Power power)
        {
            var powerResult = PowerRepository.Entities.FirstOrDefault(p => p.Id == power.Id);
            if (powerResult == null)
            {
                throw new BusinessException("该权限可能已被删除，请刷新后再试");
            }

            powerResult.ModifyPowerIdentity(power.PowerIdentity);

            powerResult.ModifyPowerName(power.Name);

            PowerRepository.Update(powerResult);
        }

        public void RemovePower(Int32 powerId)
        {
            var powerResult = PowerRepository.Entities.FirstOrDefault(power => power.Id == powerId);

            if (powerResult == null)
            {
                throw new BusinessException("该权限可能已被删除，请刷新后再试");
            }

            powerResult.Remove();

            PowerRepository.Update(powerResult);
        }
    }
}
