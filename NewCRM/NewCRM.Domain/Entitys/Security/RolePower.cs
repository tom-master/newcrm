using System;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys.Security
{
    public class RolePower : DomainModelBase
    {
        #region public property
        [Required()]
        public Int32 RoleId { get; private set; }

        [Required()]
        public Int32 AppId { get; private set; }

        #endregion

        #region ctor

        public RolePower(Int32 roleId, Int32 appId)
        {
            RoleId = roleId;
            AppId = appId;
        }

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
