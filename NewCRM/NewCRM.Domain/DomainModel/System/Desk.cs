using System;
using NewCRM.Domain.DomainModel.Account;

namespace NewCRM.Domain.DomainModel.System
{
    public class Desk : EntityBase<Int32>
    {
        private Int32 _userId;

        private String _app;

        private User _user;


        public Desk()
        {
        }

        public Int32 UserId
        {
            get
            {
                return _userId;
            }
            set { _userId = value; }
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


    }
}