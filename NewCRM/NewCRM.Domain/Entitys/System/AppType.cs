using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys.System
{
    [Description("应用类型"), Serializable]
    public partial class AppType : DomainModelBase, IAggregationRoot
    {
        #region public proptery

        /// <summary>
        /// 名称
        /// </summary>
        [Required(), MaxLength(6)]
        public String Name { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(50)]
        public String Remark { get; private set; }

        public virtual ICollection<App> Apps { get; set; }

        #endregion

        #region ctor
        /// <summary>
        /// 实例化一个app类型对象
        /// </summary>
        public AppType(String name, String remark = default(String)) 
        {
            Name = name;
            Remark = remark;
        }

        #endregion
    }
}
