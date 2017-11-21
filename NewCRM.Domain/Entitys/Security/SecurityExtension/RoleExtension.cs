using System;
using System.Linq;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Domain.Entitys.Security
{
    public partial class Role
    {
        #region public method

        /// <summary>
        /// 修改角色名称
        /// </summary>
        public Role ModifyRoleName(String newRoleName)
        {
            Name = newRoleName;
            return this;
        }

        /// <summary>
        /// 修改角色标识
        /// </summary>
        /// <returns></returns>
        public Role ModifyRoleIdentity(String newRoleIdentity)
        {
            RoleIdentity = newRoleIdentity;
            return this;
        }

        public override String KeyGenerator()
        {
            return $"NewCRM:{nameof(Role)}:Id:{Id}";
        }

        #endregion
    }
}
