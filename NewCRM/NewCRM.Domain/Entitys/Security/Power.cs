﻿using System;
using System.ComponentModel;

namespace NewCRM.Domain.Entitys.Security
{
    [Description("权限"), Serializable]
    public partial class Power : DomainModelBase, IAggregationRoot
    {
        #region public property

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; private set; }

        public String PowerIdentity { get; private set; }

        /// <summary>
        /// 备注
        /// </summary>
        public String Remark { get; private set; }

        /// <summary>
        /// 父权限Id
        /// </summary>
        public Int32? ParentId { get; private set; }

        #endregion

        #region ctor

        /// <summary>
        /// 实例化一个权限对象
        /// </summary>
        /// <param name="name"></param>
        /// <param name="powerIdentity"></param>
        /// <param name="remark"></param>
        /// <param name="parentId"></param>
        public Power(String name, String powerIdentity, String remark = default(String), Int32? parentId = null)
        {
            Name = name;

            PowerIdentity = powerIdentity;

            Remark = remark;

            ParentId = parentId;
        }
        public Power()
        {

        }
        #endregion

    }
}