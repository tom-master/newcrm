using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NewCRM.Domain.DomainModel.Account;

namespace NewCRM.Domain.DomainModel.System
{
    [Serializable]
    public class Desk : EntityBase<Int32>
    {
        #region private field

        private String _deskName;

        private ICollection<User> _users;

        private ICollection<App> _apps;

        private ICollection<Folder> _folders;
        #endregion

        #region ctor
        public Desk()
        {
        }
        #endregion

        #region public attribute

        [StringLength(100)]
        public String DeskName
        {
            get { return _deskName; }
            set { _deskName = value; }
        }

        public virtual ICollection<User> Users
        {
            get { return _users; }
            set { _users = value; }
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
        #endregion

    }
}