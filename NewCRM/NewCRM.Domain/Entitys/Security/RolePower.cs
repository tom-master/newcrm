using System;

namespace NewCRM.Domain.Entitys.Security
{
    public class RolePower : DomainModelBase
    {

        #region public property

        public Int32 RoleId { get; private set; }

        public Int32 PowerId { get; private set; }

        public virtual Power Power { get; private set; }

        #endregion


        #region ctor

        public RolePower(Int32 roleId, Int32 powerId):this()
        {
            RoleId = roleId;
            PowerId = powerId;
        }

        public RolePower() { AddTime = DateTime.Now; }

        /// <summary>
        /// 移除角色权限
        /// </summary>
        public void Remove()
        {
            IsDeleted = true;
        }

        #endregion
    }
}
