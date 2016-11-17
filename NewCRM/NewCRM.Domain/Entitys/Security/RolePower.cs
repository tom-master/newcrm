using System;

namespace NewCRM.Domain.Entitys.Security
{
    public class RolePower : DomainModelBase
    {

        #region public property

        public Int32 RoleId { get; private set; }


        public Int32 PowerId { get; private set; }
        #endregion


        #region ctor

        public RolePower(Int32 roleId, Int32 powerId)
        {
            RoleId = roleId;
            PowerId = powerId;
            AddTime = DateTime.Now;
        }

        public RolePower() { }

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
