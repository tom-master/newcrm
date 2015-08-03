using System;
using System.ComponentModel.DataAnnotations;
using NewCRM.Domain.DomainModel.Account;

namespace NewCRM.Domain.DomainModel.System
{
    public class Desk : EntityBase<Int32>
    {
        #region private field
       
        private String _deskName;

      

        private String _app;

        private User _user;
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


        public String App
        {
            get { return _app; }
            set { _app = value; }
        }

        public virtual User User
        {
            get { return _user; }
            set { _user = value; }
        }
        #endregion

    }
}