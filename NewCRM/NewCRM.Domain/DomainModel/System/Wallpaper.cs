using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NewCRM.Domain.DomainModel.Account;

namespace NewCRM.Domain.DomainModel.System
{
    [Serializable]
    [Description("壁纸")]
    public class Wallpaper : EntityBase<Int32>, IAggregationRoot
    {
        #region private fiele
        private String _title;
        private String _url;
        private String _smallUrl;
        private String _wallpaperType;
        private String _wallpaperWebUr;
        private String _description;
        private Int32 _width;
        private Int32 _heigth;
        private Boolean _isSystem;
        private Int32 _uploaderId;

        private ICollection<User> _users;


        #endregion

        #region ctor

        public Wallpaper(String title,
            String url,
            String smallUrl,
            String wallpaperType,
            String wallpaperWebUr,
            Int32 width,
            Int32 heigth,
            String description,
            Boolean isSystem,
            Int32 uploaderId)
            : this()
        {
            _title = title;
            _url = url;
            _smallUrl = smallUrl;
            _width = width;
            _heigth = heigth;
            _description = description;
            _isSystem = isSystem;
            _uploaderId = uploaderId;
            _wallpaperType = wallpaperType;
            _wallpaperWebUr = wallpaperWebUr;
            _users = new List<User>();
        }

        public Wallpaper()
        {
            // TODO: Complete member initialization
        }

        #endregion

        #region public attribute

        [Required, StringLength(50)]
        public String Title
        {
            get { return _title; }
            set { _title = value; }
        }

        [Required, StringLength(100)]
        public String Url
        {
            get { return _url; }
            set { _url = value; }
        }

        [Required, StringLength(100)]
        public String SmallUrl
        {
            get { return _smallUrl; }
            set { _smallUrl = value; }
        }

        public Int32 Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public Int32 Heigth
        {
            get { return _heigth; }
            set { _heigth = value; }
        }

        [StringLength(250)]
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public Boolean IsSystem
        {
            get { return _isSystem; }
            set { _isSystem = value; }
        }

        [StringLength(10)]
        public String WallpaperType
        {
            get { return _wallpaperType; }
            set { _wallpaperType = value; }
        }

        [StringLength(150)]
        public String WallpaperWebUrl
        {
            get { return _wallpaperWebUr; }
            set { _wallpaperWebUr = value; }
        }
        public virtual ICollection<User> Users
        {
            get { return _users; }
            set { _users = value; }
        }
        public Int32 UploaderId
        {
            get { return _uploaderId; }
            set { _uploaderId = value; }
        }
        #endregion
    }
}
