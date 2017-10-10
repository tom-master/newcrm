using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys.Security
{
    [Description("角色"), Serializable]
    public partial class Role : DomainModelBase, IAggregationRoot
    {
        #region public property

        /// <summary>
        /// 名称
        /// </summary>
        [Required(), MaxLength(6)]
        public String Name { get; private set; }

        /// <summary>
        /// 角色标识
        /// </summary>
        [Required(), MaxLength(20)]
        public String RoleIdentity { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(50)]
        public String Remark { get; private set; }

        /// <summary>
        /// 角色对应的权限
        /// </summary>
        public virtual ICollection<RolePower> Powers { get; private set; }

        #endregion

        #region ctor
        /// <summary>
        /// 实例化一个角色对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="remark"></param>
        public Role(String name, String remark = default(String)) 
        {
            Name = name;
            Remark = remark;
            Powers = new List<RolePower>();

        }

        #endregion
    }
}
