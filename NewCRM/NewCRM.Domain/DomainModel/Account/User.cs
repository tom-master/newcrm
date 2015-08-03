using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NewCRM.Domain.DomainModel.Security;
using NewCRM.Domain.DomainModel.System;
namespace NewCRM.Domain.DomainModel.Account
{
    [Description("用户")]
    [Serializable]
    public class User : EntityBase<Int32>, IAggregationRoot
    {

        #region private field
        private String _name;
        private String _password;

        private String _appXy;
        private String _dockPosition;
        private String _skin;
        private String _dock;

        private Int32 _appSize;
        private Int32 _appVerticalSpacing;
        private Int32 _appHorizontalSpacing;


        private String _wallpaperShowType;
        private String _userFace;
        private Boolean _enabled;
        private DateTime _lastLoginTime;
        private Int32 _defaultDesk;
        private Configs _config;
        private Title _title;
        private Depts _dept;
        private Wallpaper _wallpaper;
        private ICollection<App> _apps;
        private ICollection<Folder> _folders;
        private ICollection<Role> _roles;
        private ICollection<Desk> _desks;


        #endregion

        #region ctor

        public User()
        {
        }

        #endregion

        #region public attirbute




        [Required, StringLength(50)]
        public String Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }


        [Required, StringLength(50)]
        public String Password
        {
            get { return _password; }
            set { _password = value; }
        }

        [Required]
        public bool Enabled
        {
            get { return _enabled; }
            set { _enabled = value; }
        }

        public DateTime LastLoginTime
        {
            get { return _lastLoginTime; }
            set { _lastLoginTime = value; }
        }
        //新增字段
        [StringLength(10)]
        public String AppXy
        {
            get { return _appXy; }
            set { _appXy = value; }
        }

        [StringLength(10)]
        public String DockPosition
        {
            get { return _dockPosition; }
            set { _dockPosition = value; }
        }

        [StringLength(10)]
        public String Skin
        {
            get { return _skin; }
            set { _skin = value; }
        }

        [StringLength(500)]
        public String Dock
        {
            get { return _dock; }
            set { _dock = value; }
        }

        [StringLength(50)]
        public String WallpaperShowType
        {
            get { return _wallpaperShowType; }
            set { _wallpaperShowType = value; }
        }

        public String UserFace
        {
            get { return _userFace; }
            set { _userFace = value; }
        }

        public Configs Config
        {
            get { return _config; }
            set { _config = value; }
        }

        public Int32 AppSize
        {
            get { return _appSize; }
            set { _appSize = value; }
        }

        public Int32 AppVerticalSpacing
        {
            get { return _appVerticalSpacing; }
            set { _appVerticalSpacing = value; }
        }

        public Int32 AppHorizontalSpacing
        {
            get { return _appHorizontalSpacing; }
            set { _appHorizontalSpacing = value; }
        }




        public Int32 DefaultDesk
        {
            get { return _defaultDesk; }
            set { _defaultDesk = value; }
        }

        public virtual Depts Dept
        {
            get { return _dept; }
            set { _dept = value; }
        }

        public virtual Wallpaper Wallpaper
        {
            get { return _wallpaper; }
            set { _wallpaper = value; }
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

        public virtual ICollection<Role> Roles
        {
            get { return _roles; }
            set { _roles = value; }
        }

        public virtual Title Title
        {
            get { return _title; }
            set { _title = value; }
        }
        public virtual ICollection<Desk> Desks
        {
            get { return _desks; }
            set { _desks = value; }
        }


        #endregion
    }
}
