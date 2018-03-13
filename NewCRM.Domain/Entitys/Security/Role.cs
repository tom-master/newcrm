using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys.Security
{
    [Description("角色"), Serializable]
    public partial class Role : DomainModelBase
    {
        #region private field

        private String _name;

        private String _roleIdentity;

        private String _remark;

        private Boolean _isAllowDisable;

        private IList<RolePower> _powers;

        #endregion

        #region public property

        /// <summary>
        /// 名称
        /// </summary>
        [Required, MaxLength(6)]
        public String Name
        {
            get { return _name; }
            private set
            {
                if(_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        /// <summary>
        /// 角色标识
        /// </summary>
        [Required, MaxLength(20)]
        public String RoleIdentity
        {
            get { return _roleIdentity; }
            private set
            {
                if(_roleIdentity != value)
                {
                    _roleIdentity = value;
                    OnPropertyChanged(nameof(RoleIdentity));
                }
            }
        }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(50)]
        public String Remark
        {
            get { return _remark; }
            private set
            {
                if(_remark != value)
                {
                    _remark = value;
                    OnPropertyChanged(nameof(Remark));
                }
            }
        }

        public Boolean IsAllowDisable
        {
            get { return _isAllowDisable; }
            private set
            {
                if(_isAllowDisable != value)
                {
                    _isAllowDisable = value;
                    OnPropertyChanged(nameof(IsAllowDisable));
                }
            }
        }

        public IList<RolePower> Powers
        {
            get { return _powers; }
            private set
            {
                if(_powers != value)
                {
                    _powers = value;
                    OnPropertyChanged(nameof(Powers));
                }
            }
        }

        #endregion

        #region ctor
        /// <summary>
        /// 实例化一个角色对象
        /// </summary>
        public Role(String name, String roleIdentity, String remark = default(String), Boolean isAllowDisable = default(Boolean))
        {
            Name = name;
            Remark = remark;
            RoleIdentity = roleIdentity;
            IsAllowDisable = isAllowDisable;
            Powers = new List<RolePower>();
        }

        /// <summary>
        /// 实例化一个角色对象
        /// </summary>
        public Role()
        {
        }

        #endregion
    }

    /// <summary>
    /// RoleExtension
    /// </summary>
    public partial class Role
    {

    }
}
