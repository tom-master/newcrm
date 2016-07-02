using System;

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
        /// 为角色添加权限
        /// </summary>
        /// <param name="powers"></param>
        /// <returns></returns>
        public Role AddPower(params Power[] powers)
        {
            if (powers == null)
            {
                throw new ArgumentNullException($"{nameof(powers)}不能为空");
            }
            if (powers.Length <= 0)
            {
                throw new ArgumentNullException($"{nameof(powers)}不能为0");
            }
            foreach (var power in powers)
            {
                Powers.Add(new RolePower(Id, power.Id));
            }

            return this;
        }


        
        #endregion
    }
}
