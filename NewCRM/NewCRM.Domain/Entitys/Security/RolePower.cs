using System;

namespace NewCRM.Domain.Entitys.Security
{
    public class RolePower : DomainModelBase
    {

        #region public property

        public Int32 RoleId { get; private set; }

        public Int32 AppId { get; private set; }

        #endregion


        #region ctor

        public RolePower(Int32 roleId, Int32 appId) : this()
        {
            RoleId = roleId;
            AppId = appId;
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
