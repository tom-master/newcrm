using System;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys.Security
{
    public class RolePower : DomainModelBase
    {
        #region private field
        private Int32 _roleId;

        private Int32 _appId;
        #endregion

        #region public property
        [Required]
        public Int32 RoleId
        {
            get { return _roleId; }
            private set
            {
                if(_roleId != value)
                {
                    _roleId = value;
                    OnPropertyChanged(nameof(RoleId));
                }
            }
        }

        [Required]
        public Int32 AppId
        {
            get { return _appId; }
            private set
            {
                if(_appId != value)
                {
                    _appId = value;
                    OnPropertyChanged(nameof(AppId));
                }
            }
        }
        #endregion

        #region ctor

        public RolePower(Int32 roleId, Int32 appId)
        {
            RoleId = roleId;
            AppId = appId;
        }

        public RolePower()
        {

        }

        /// <summary>
        /// 移除角色权限
        /// </summary>
        public void Remove() => IsDeleted = true;

        #endregion
    }
}
