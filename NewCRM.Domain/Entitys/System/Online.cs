using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys.System
{
    [Description("在线人数"), Serializable]
    public partial class Online : DomainModelBase
    {
        #region private field

        private String _ipAddress;

        private Int32 _accountId;
        #endregion

        #region public property

        /// <summary>
        /// Ip地址
        /// </summary>
        [Required]
        public String IpAddress
        {
            get { return _ipAddress; }
            private set
            {
                if(_ipAddress != value)
                {
                    _ipAddress = value;
                    OnPropertyChanged(IpAddress.GetType());
                }
            }
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Required]
        public Int32 AccountId
        {
            get { return _accountId; }
            private set
            {
                if(_accountId != value)
                {
                    _accountId = value;
                    OnPropertyChanged(AccountId.GetType());
                }
            }
        }
        #endregion

        #region ctor

        /// <summary>
        /// 实例化一个在线状态的对象
        /// </summary>
        public Online(String ipAddress, Int32 accountId)
        {
            IpAddress = ipAddress;
            AccountId = accountId;
        }


        /// <summary>
        /// 实例化一个在线状态的对象
        /// </summary>
        public Online()
        {
        }

        #endregion
    }
}
