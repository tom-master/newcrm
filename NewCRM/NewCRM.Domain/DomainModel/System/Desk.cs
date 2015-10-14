using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NewCRM.Domain.DomainModel.Account;

namespace NewCRM.Domain.DomainModel.System
{
    public class Desk : EntityBase<Int32>
    {
        #region private field

        private String _deskName;

        private User _user;

        private ICollection<App> _apps;
        #endregion

        public Desk()
        {
        }


        #region public attribute

        [StringLength(100)]
        public String DeskName
        {
            get { return _deskName; }
            set { _deskName = value; }
        }

        public virtual ICollection<App> Apps
        {
            get { return _apps; }
            set { _apps = value; }
        }

        public virtual User User
        {
            get { return _user; }
            set { _user = value; }
        }
        #endregion

    }
}