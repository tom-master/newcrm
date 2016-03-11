using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        private Department _department;
        private UserConfigure _userConfigure;

        private ICollection<App> _apps;
        private ICollection<Role> _roles;
        private ICollection<Log> _logs;
        private ICollection<Title> _titles;
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
            get
            {
                return _department;
            }
            set
            {
                _department = value;
            }
        }

        public virtual ICollection<App> Apps
        {
            get { return _apps; }
            set
            {
                _apps = value;
            }
        }

        public virtual ICollection<Title> Titles
        {
            get { return _titles; }
            set { _titles = value; }
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
