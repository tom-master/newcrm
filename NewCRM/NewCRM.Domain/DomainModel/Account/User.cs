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

        private Boolean _enabled;
        private DateTime _lastLoginTime;

        private UserConfig _userConfig;
        private Title _title;
        private Depts _dept;

        private ICollection<Role> _roles;
        private ICollection<App> _apps;
        private ICollection<Folder> _folders;
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
        public Boolean Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public DateTime LastLoginTime
        {
            get { return _lastLoginTime; }
            set { _lastLoginTime = value; }
        }

        public UserConfig UserConfig
        {
            get { return _userConfig; }
            set { _userConfig = value; }
        }

        public virtual Depts Dept
        {
            get { return _dept; }
            set { _dept = value; }
        }

        public virtual Title Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public virtual ICollection<App> Apps
        {
            get { return _apps; }
            set { _apps = value; }
        }

        public virtual ICollection<Folder> Folders
        {
            get { return _folders; }
            set { _folders = value; }
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
