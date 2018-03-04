using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys.Security
{
    [Description("角色"), Serializable]
    public partial class Role : DomainModelBase
    {
        #region public property

        /// <summary>
        /// 名称
        /// </summary>
        [Required, MaxLength(6)]
        public String Name { get; private set; }

        /// <summary>
        /// 角色标识
        /// </summary>
        [Required, MaxLength(20)]
        public String RoleIdentity { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(50)]
        public String Remark { get; private set; }

        public Boolean IsAllowDisable { get; set; }

        public IList<RolePower> Powers { get; private set; }

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

        public Role ToList() => throw new NotImplementedException();

        #endregion
    }
}
