using System;
using System.ComponentModel;

namespace NewCRM.Domain.Entitys.Agent
{
    [Description("职称"), Serializable]
    public partial class Title : DomainModelBase, IAggregationRoot
    {
        #region public property

        /// <summary>
        ///名称
        /// </summary>
        public String Name { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; private set; }

        #endregion

        #region ctor
        /// <summary>
        /// 实例化一个职称对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="remark"></param>
        public Title(String name, String remark = default(String))
        {
            Name = name;
            Remark = remark; 
        }

        public Title()
        {

        }

        #endregion


    }
}
