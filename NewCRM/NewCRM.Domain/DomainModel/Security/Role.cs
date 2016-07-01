using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NewCRM.Domain.DomainModel.Security
{
    [Description("角色"),Serializable]
    public partial class Role : DomainModelBase, IAggregationRoot
    {
        #region public property

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
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

        public Role() { }

        #endregion
    }
}
