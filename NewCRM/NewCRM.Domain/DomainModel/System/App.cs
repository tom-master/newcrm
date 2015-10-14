using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Domain.DomainModel.Security;

namespace NewCRM.Domain.DomainModel.System
{

    [Serializable]
    [Description("应用")]
    public class App : EntityBase<Int32>, IAggregationRoot
    {
        #region private field
        private String _name;
        private String _imageUrl;
        private String _navigateUrl;
        private String _remark;
        private String _appInternalClass;
        private Int32 _sortIndex;
        private Int32 _width;
        private Int32 _height;
        private Int32 _userCount;
        private Int32 _startNumber;
        private Boolean _isMax;
        private Boolean _isMin;
        private Boolean _isFull;
        private Boolean _isSetbar;
        private Boolean _isOpenMax;
        private Boolean _isLock;
        private Boolean _isSystem;

        private AppType _appType;
        private Desk _desk;

        private ICollection<Power> _powers;
        private Folder _folder;
        private ICollection<User> _users;
        #endregion

        #region ctor



        public App()
        {

        }
        #endregion

        #region public attribute



        [Required, StringLength(50)]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [StringLength(200)]
        public String ImageUrl
        {
            get { return _imageUrl; }
            set { _imageUrl = value; }
        }

        [StringLength(200)]
        public String NavigateUrl
        {
            get { return _navigateUrl; }
            set { _navigateUrl = value; }
        }

        [StringLength(500)]
        public String Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        [Required]
        public int SortIndex
        {
            get { return _sortIndex; }
            set { _sortIndex = value; }
        }

        [Required]
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        [Required]
        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        public Boolean IsMax
        {
            get { return _isMax; }
            set { _isMax = value; }
        }

        public Boolean IsMin
        {
            get { return _isMin; }
            set { _isMin = value; }
        }

        public Boolean IsFull
        {
            get { return _isFull; }
            set { _isFull = value; }
        }

        public Boolean IsSetbar
        {
            get { return _isSetbar; }
            set { _isSetbar = value; }
        }

        public Boolean IsOpenMax
        {
            get { return _isOpenMax; }
            set { _isOpenMax = value; }
        }

        public Boolean IsLock
        {
            get { return _isLock; }
            set { _isLock = value; }
        }

        public Int32 UserCount
        {
            get { return _userCount; }
            set { _userCount = value; }
        }

        public Int32 StartNumber
        {
            get { return _startNumber; }
            set { _startNumber = value; }
        }

        public Boolean IsSystem
        {
            get { return _isSystem; }
            set { _isSystem = value; }
        }

        public String AppInternalClass
        {
            get { return _appInternalClass; }
            set { _appInternalClass = value; }
        }


        public virtual Desk Desk
        {
            get { return _desk; }
            set { _desk = value; }
        }

        public virtual AppType AppType
        {
            get { return _appType; }
            set { _appType = value; }
        }

        public virtual ICollection<Power> Powers
        {
            get { return _powers; }
            set { _powers = value; }
        }

        public virtual Folder Folder
        {
            get { return _folder; }
            set { _folder = value; }
        }


        public virtual ICollection<User> Users
        {
            get { return _users; }
            set { _users = value; }
        }

        #endregion
    }
}
