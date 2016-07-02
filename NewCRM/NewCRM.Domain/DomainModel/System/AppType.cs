using System;
using System.ComponentModel;

namespace NewCRM.Domain.Entities.DomainModel.System
{
    [Description("应用类型"),Serializable]
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
        #endregion

        #region ctor
        /// <summary>
        /// 实例化一个app类型对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="remark"></param>
        public AppType(String name, String remark = default(String))
        {
            Name = name;
            Remark = remark;
        }

        public AppType()
        {

        }
        #endregion


    }
}
