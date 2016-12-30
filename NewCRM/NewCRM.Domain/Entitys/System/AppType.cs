using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NewCRM.Domain.Entitys.System
{
    [Description("应用类型"), Serializable]
    public partial class AppType : DomainModelBase, IAggregationRoot
    {
        #region public proptery
        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; private set; }


        public virtual ICollection<App> Apps { get; set; }

        #endregion

        #region ctor
        /// <summary>
        /// 实例化一个app类型对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="remark"></param>
        public AppType(String name, String remark = default(String)):this()
        {
            Name = name;
            Remark = remark;
        }

        public AppType()
        {
            AddTime = DateTime.Now;
        }
        #endregion


    }
}
