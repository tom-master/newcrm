using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NewCRM.Domain.DomainModel.Security;
using NewCRM.Domain.DomainModel.System;
namespace NewCRM.Domain.DomainModel.Account
{
    [Description("用户")]
    [Serializable]
    public class User : EntityBase<Int32>, IAggregationRoot
    {

        #region private field
        private String _name;
        private String _password;

        private Boolean _isDisable;
        private DateTime _lastLoginTime;

        private Title _title;
        private Department _department;
        private UserConfigure _userConfigure;
        private ICollection<Role> _roles;
        private ICollection<Log> _logs;

        #endregion

        #region ctor

        public User()
        {
        }

        #endregion

        #region public attirbute

        [Required, StringLength(50)]
        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }


        [Required, StringLength(50)]
        public String Password
        {
            get { return _password; }
            set { _password = value; }
        }

        [Required]
        public Boolean IsDisable
        {
            get { return _isDisable; }
            set { _isDisable = value; }
        }

        public DateTime LastLoginTime
        {
            get { return _lastLoginTime; }
            set { _lastLoginTime = value; }
        }

        public virtual UserConfigure UserConfigure
        {
            get { return _userConfigure; }
            set { _userConfigure = value; }
        }

        public virtual Department Department
        {
            get { return _department; }
            set { _department = value; }
        }

        public virtual Title Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public virtual ICollection<Role> Roles
        {
            get { return _roles; }
            set { _roles = value; }
        }

        public virtual ICollection<Log> Logs
        {
            get { return _logs; }
            set { _logs = value; }
        }
        #endregion
    }
}
