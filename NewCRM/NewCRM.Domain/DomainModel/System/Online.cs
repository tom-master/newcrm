using System;
using System.ComponentModel;

namespace NewCRM.Domain.Entities.DomainModel.System
{
    [Description("在线人数"),Serializable]
    public  class Online : DomainModelBase, IAggregationRoot
    {
        #region public property

        /// <summary>
        /// Ip地址
        /// </summary>
        public String IpAddress { get; private set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public Int32 UserId { get; private set; }
        #endregion

        #region ctor

        /// <summary>
        /// 实例化一个在线状态的对象
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="userId"></param>
        public Online(String ipAddress, Int32 userId)
        {
            IpAddress = ipAddress;
            UserId = userId;
            AddTime = DateTime.Now;
        }

        public Online() { }


        #endregion

    }
}
