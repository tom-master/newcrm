using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Domain.DomainModel.Security;
namespace NewCRM.Domain.DomainModel.System
{
    [Description("文件夹")]
    [Serializable]
    public class Folder : EntityBase<Int32>, IAggregationRoot
    {
        #region private field
        private String _name;
        private String _icon;
        private String _remake;
        private ICollection<App> _apps;

        private Desk _desk;

        #endregion

        #region ctor
        public Folder()
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

        [StringLength(100)]
        public String Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }

        [StringLength(250)]
        public String Remake
        {
            get { return _remake; }
            set { _remake = value; }
        }

        public virtual ICollection<App> Apps
        {
            get { return _apps; }
            set { _apps = value; }
        }

        public virtual Desk Desk
        {
            get
            {
                return _desk;
            }
            set
            {
                _desk = value;
            }
        }
        #endregion
    }
}
