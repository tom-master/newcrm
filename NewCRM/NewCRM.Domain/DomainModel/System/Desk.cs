using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.DomainModel.System
{
    [Description("桌面")]
    [Serializable]
    public class Desk : EntityBase<Int32>, IAggregationRoot
    {
        #region private field

        private String _deskName;

        private Boolean _isDefault;

        private ICollection<UserConfigure> _userConfigures;

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


        public Boolean IsDefault
        {
            get
            {
                return _isDefault;
            }
            set
            {
                _isDefault = value;
            }
        }

        public virtual ICollection<UserConfigure> UserConfigures
        {
            get { return _userConfigures; }
            set { _userConfigures = value; }
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