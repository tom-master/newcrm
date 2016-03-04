using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.DomainModel.System
{
    [Description("在线人数")]
    [Serializable]
    public class Online : EntityBase<Int32>
    {
        #region private field
        private String _ipAdddress;
        private Int32 _userId;
        #endregion

        #region ctor
        public Online()
        {

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
