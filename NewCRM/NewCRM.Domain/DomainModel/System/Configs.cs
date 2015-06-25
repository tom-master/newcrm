using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NewCRM.Domain.DomainModel.Account;

namespace NewCRM.Domain.DomainModel.System
{
    [Description("系统配置")]
    [Serializable]
    public class Configs : EntityBase<Int32>
    {

        #region private field
        private String _configKey;
        private String _configValue;
        private String _remark;
        private User _user;
        #endregion

        #region ctor
        public Configs(String configKey, String configValue, String remake)
            : this()
        {
            _configKey = configKey;
            _configValue = configValue;
            _remark = remake;
        }

        public Configs()
        {
            // TODO: Complete member initialization
        }

        #endregion

        #region public attribute


        public User User
        {
            get { return _user; }
            set { _user = value; }
        }

        [Required, StringLength(50)]
        public String ConfigKey
        {
            get { return _configKey; }
            set { _configKey = value; }
        }

        [Required, StringLength(4000)]
        public String ConfigValue
        {
            get { return _configValue; }
            set { _configValue = value; }
        }

        [StringLength(500)]
        public String Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        #endregion
    }
}
