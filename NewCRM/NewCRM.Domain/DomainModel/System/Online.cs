using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NewCRM.Domain.DomainModel.Account;

namespace NewCRM.Domain.DomainModel.System
{
    [Description("在线人数")]
    [Serializable]
    public class Online : EntityBase<Int32>, IAggregationRoot
    {
        #region private field
        private String _ipAdddress;
        private Int32 _userId;
        #endregion

        #region ctor

        public Online(String ipAddress, Int32 userId)
            : this()
        {
            _ipAdddress = ipAddress;
            _userId = userId;
        }

        public Online()
        {
            // TODO: Complete member initialization
        }

        #endregion

        #region public attribute
        [StringLength(50)]
        public String IpAdddress
        {
            get { return _ipAdddress; }
            set { _ipAdddress = value; }
        }

        public Int32 UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        #endregion
    }
}
