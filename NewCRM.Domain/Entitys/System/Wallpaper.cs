using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NewCRM.Domain.ValueObject;

namespace NewCRM.Domain.Entitys.System
{
    [Serializable, Description("壁纸")]
    public partial class Wallpaper : DomainModelBase
    {
        #region private field

        private String _title;

        private String _url;

        private String _shortUrl;

        private WallpaperSource _source;

        private String _description;

        private Int32 _width;

        private Int32 _height;

        private Int32 _accountId;

        private String _md5;
        #endregion

        #region public property

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        public String Title
        {
            get { return _title; }
            private set
            {
                if(_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        /// <summary>
        /// 图片地址
        /// </summary>
        [Required]
        public String Url
        {
            get { return _url; }
            private set
            {
                if(_url != value)
                {
                    _url = value;
                    OnPropertyChanged(nameof(Url));
                }
            }
        }

        /// <summary>
        /// 短地址
        /// </summary> 
        public String ShortUrl
        {
            get { return _shortUrl; }
            private set
            {
                if(_shortUrl != value)
                {
                    _shortUrl = value;
                    OnPropertyChanged(nameof(ShortUrl));
                }
            }
        }

        /// <summary>
        /// 来源
        /// </summary>
        public WallpaperSource Source
        {
            get { return _source; }
            private set
            {
                if(_source != value)
                {
                    _source = value;
                    OnPropertyChanged(nameof(Source));
                }
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(50)]
        public String Description
        {
            get { return _description; }
            private set
            {
                if(_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        /// <summary>
        /// 图片的宽
        /// </summary>
        public Int32 Width
        {
            get { return _width; }
            private set
            {
                if(_width != value)
                {
                    _width = value;
                    OnPropertyChanged(nameof(Width));
                }
            }
        }

        /// <summary>
        /// 图片的高
        /// </summary>
        public Int32 Height
        {
            get { return _height; }
            private set
            {
                if(_height != value)
                {
                    _height = value;
                    OnPropertyChanged(nameof(Height));
                }
            }
        }

        /// <summary>
        /// 上传者（用户）
        /// </summary>
        public Int32 AccountId
        {
            get { return _accountId; }
            private set
            {
                if(_accountId != value)
                {
                    _accountId = value;
                    OnPropertyChanged(nameof(AccountId));
                }
            }
        }

        /// <summary>
        /// md5
        /// </summary>
        public String Md5
        {
            get { return _md5; }
            private set
            {
                if(_md5 != value)
                {
                    _md5 = value;
                    OnPropertyChanged(nameof(Md5));
                }
            }
        }

        #endregion

        #region ctor

        /// <summary>
        /// 实例化一个壁纸对象
        /// </summary>
        public Wallpaper(String title, String url, String description, Int32 width, Int32 height, String md5, Int32 accountId = default(Int32), WallpaperSource wallpaperSource = default(WallpaperSource))
        {
            Title = title;
            Url = url;
            Description = description;
            Width = width;
            Height = height;
            Source = wallpaperSource;
            AccountId = accountId;
            Md5 = md5;
        }

        public Wallpaper()
        {

        }

        #endregion
    }

    /// <summary>
    /// WallpaperExtension
    /// </summary>
    public partial class Wallpaper
    {

    }
}
