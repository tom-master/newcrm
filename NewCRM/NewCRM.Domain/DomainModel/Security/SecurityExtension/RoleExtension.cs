using System;
using System.Linq;

namespace NewCRM.Domain.Entities.DomainModel.Security
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
        public Role AddPower(params Power[] powers)
        {
            return AddPower(Powers.Select(p => p.PowerId).ToArray());
        }

        public Role AddPower(params Int32[] powerIds)
        {
            if (powerIds == null)
            {
                throw new ArgumentNullException($"{nameof(powerIds)}不能为空");
            }
            if (powerIds.Length <= 0)
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
        public Role RemovePower(params Power[] powers)
        {
            return RemovePower(Powers.Select(p => p.PowerId).ToArray());
        }

        public Role RemovePower(params Int32[] powerIds)
        {
            if (powerIds == null)
            {
                throw new ArgumentNullException($"{nameof(powerIds)}不能为空");
            }
            if (powerIds.Length <= 0)
            {
                throw new ArgumentNullException($"{nameof(powerIds)}不能为0");
            }

            foreach (var powerId in powerIds)
            {
                Powers.FirstOrDefault(p => p.PowerId == powerId).Remove();
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


        #endregion
    }
}
