using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using NewCRM.Domain.DomainModel.Account;

namespace NewCRM.Domain.DomainModel.Security
{
    [Description("脚色")]
    [Serializable]
    public class Role : EntityBase<Int32>, IAggregationRoot
    {

        #region private field
        private String _name;
        private String _remark;
        private ICollection<User> _users;
        private ICollection<Power> _powers;
        #endregion

        #region ctor

        public Role(String name, String remake)
            : this()
        {
            _name = name;
            _remark = remake;
        }

        public Role()
        {
            // TODO: Complete member initialization
        }

        #endregion

        #region public attribute

        [Required, StringLength(50)]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [StringLength(500)]
        public String Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }


        public virtual ICollection<User> Users
        {
            get { return _users; }
            set { _users = value; }
        }


        public virtual ICollection<Power> Powers
        {
            get { return _powers; }
            set { _powers = value; }
        }

        #endregion
    }
}
