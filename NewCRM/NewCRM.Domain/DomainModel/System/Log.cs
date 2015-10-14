using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NewCRM.Domain.DomainModel.Account;

namespace NewCRM.Domain.DomainModel.System
{
    [Description("日志")]
    [Serializable]
    public class Log : EntityBase<Int32>
    {
        #region private field
        private String _level;
        private String _logger;
        private String _message;
        private String _exception;

        private User _user;
        #endregion

        #region ctor
        public Log()
        {
        }

        #endregion

        #region public attribute

        [StringLength(20)]
        public String Levels
        {
            get { return _level; }
            set { _level = value; }
        }

        [StringLength(200)]
        public String Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        [StringLength(4000)]
        public String Message
        {
            get { return _message; }
            set { _message = value; }
        }

        [StringLength(4000)]
        public String Exception
        {
            get { return _exception; }
            set { _exception = value; }
        }


        public virtual User User
        {
            get { return _user; }
            set { _user = value; }
        }

        #endregion
    }
}
