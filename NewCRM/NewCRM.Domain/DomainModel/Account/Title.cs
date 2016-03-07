using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace NewCRM.Domain.DomainModel.Account
{
    [Description("头衔")]
    [Serializable]
    public class Title : EntityBase<Int32>, IAggregationRoot
    {
        #region private field
        private String _name;
        private String _remark;

        private ICollection<User> _users;

        #endregion

        #region ctor

        public Title()
        {
        }

        #endregion

        #region public attribute
        [Required, StringLength(50)]
        public String Name
        {
            get
            {
                return _name;
            }
            set { _name = value; }
        }

        [StringLength(500)]
        public String Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }


        public ICollection <User> Users
        {
            get
            {
                return _users;
            }
            set
            {
                _users = value;
            }
        }

        #endregion
    }
}
