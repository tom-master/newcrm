using System;
using System.Collections.Generic;
using System.Linq;

namespace NewCRM.Domain.Entitys.Security
{
    public partial class Role
    {
        #region public method

        /// <summary>
        /// 修改角色名称
        /// </summary>
        /// <param name="newRoleName"></param>
        public Role ModifyRoleName(String newRoleName)
        {
            Name = newRoleName;
            return this;
        }

        /// <summary>
        /// 修改角色标识
        /// </summary>
        /// <param name="newRoleIdentity"></param>
        /// <returns></returns>
        public Role ModifyRoleIdentity(String newRoleIdentity)
        {
            RoleIdentity = newRoleIdentity;
            return this;
        }

        /// <summary>
        /// 为角色添加权限
        /// </summary>
        /// <param name="powers"></param>
        /// <returns></returns>
        public Role AddPower(IEnumerable<Power> powers)
        {
            return AddPower(powers.Select(p => p.Id).ToArray());
        }

        /// <summary>
        /// 为角色添加权限
        /// </summary>
        /// <param name="powerIds"></param>
        /// <returns></returns>
        public Role AddPower(IEnumerable<Int32> powerIds)
        {
            if (powerIds == null)
            {
                throw new ArgumentNullException($"{nameof(powerIds)}不能为空");
            }
            if (!powerIds.Any())
            {
                throw new ArgumentNullException($"{nameof(powerIds)}不能为0");
            }
            foreach (var powerid in powerIds)
            {
                Powers.Add(new RolePower(Id, powerid));
            }

            return this;
        }

        /// <summary>
        /// 移除角色权限
        /// </summary>
        /// <param name="powers"></param>
        /// <returns></returns>
        public Role RemovePower(IEnumerable<Power> powers)
        {
            return RemovePower(powers.Select(p => p.Id).ToArray());
        }

        /// <summary>
        /// 移除角色权限
        /// </summary>
        /// <param name="powerIds"></param>
        /// <returns></returns>
        public Role RemovePower(IEnumerable<Int32> powerIds)
        {
            if (powerIds == null)
            {
                throw new ArgumentNullException($"{nameof(powerIds)}不能为空");
            }

            if (!powerIds.Any())
            {
                throw new ArgumentNullException($"{nameof(powerIds)}不能为0");
            }

            foreach (var powerId in powerIds)
            {
                Powers.FirstOrDefault(p => p.PowerId == powerId)?.Remove();
            }
            return this;
        }

        /// <summary>
        /// 移除角色
        /// </summary>
        public void Remove()
        {
            if (Powers.Any())
            {
                Powers.ToList().ForEach(p =>
                {
                    p.Remove();
                });
            }

            IsDeleted = true;
        }


        /// <summary>
        /// 检查权限是否存在
        /// </summary>
        /// <param name="powerIds"></param>
        /// <returns></returns>
        public Boolean CheckPower(IEnumerable<Int32> powerIds)
        {
            if (!powerIds.Any())
            {
                throw new ArgumentException($"对不起，您没有访问的权限！");
            }

            return powerIds.Any(powerId =>
            {
                var internalPower = Powers.FirstOrDefault(power => power.PowerId == powerId);

                if (internalPower == null)
                {
                    return false;
                }

                if (internalPower.Power.ParentId == null)
                {
                    return true;
                }

                return internalPower.PowerId == powerId;
            });

        }

        #endregion
    }
}
