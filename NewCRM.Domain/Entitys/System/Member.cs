using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NewCRM.Domain.ValueObject;

namespace NewCRM.Domain.Entitys.System
{
    /// <summary>
    /// 成员
    /// </summary>
    [Serializable, Description("成员")]
    public partial class Member : DomainModelBase
    {
        #region private field

        private Int32 _appId;

        private Int32 _width;

        private Int32 _height;

        private Int32 _folderId;

        private String _name;

        private String _iconUrl;

        private String _appUrl;

        private Boolean _isOnDock;

        private Boolean _isMax;

        private Boolean _isFull;

        private Boolean _isSetbar;

        private Boolean _isOpenMax;

        private Boolean _isLock;

        private Boolean _isFlash;

        private Boolean _isDraw;

        private Boolean _isResize;

        private MemberType _memberType;

        private Int32 _deskIndex;

        private Int32 _accountId;

        private Boolean _isIconByUpload;

        #endregion

        #region public property

        /// <summary>
        /// 应用Id
        /// </summary>
        public Int32 AppId
        {
            get { return _appId; }
            private set
            {
                if(_appId != value)
                {
                    _appId = value;
                    OnPropertyChanged(nameof(AppId));
                }
            }
        }

        /// <summary>
        /// 成员的宽
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
        /// 成员的高
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
        /// 文件夹Id
        /// </summary>
        public Int32 FolderId
        {
            get { return _folderId; }
            private set
            {
                if(_folderId != value)
                {
                    _folderId = value;
                    OnPropertyChanged(nameof(FolderId));
                }
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        [Required, MaxLength(6)]
        public String Name
        {
            get { return _name; }
            private set
            {
                if(_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        /// <summary>
        /// 图标地址
        /// </summary>
        [Required]
        public String IconUrl
        {
            get { return _iconUrl; }
            private set
            {
                if(_iconUrl != value)
                {
                    _iconUrl = value;
                    OnPropertyChanged(nameof(IconUrl));
                }
            }
        }

        /// <summary>
        /// app地址
        /// </summary>
        public String AppUrl
        {
            get { return _appUrl; }
            private set
            {
                if(_appUrl != value)
                {
                    _appUrl = value;
                    OnPropertyChanged(nameof(AppUrl));
                }
            }
        }

        /// <summary>
        /// 成员是否在应用码头上
        /// </summary>
        public Boolean IsOnDock
        {
            get { return _isOnDock; }
            private set
            {
                if(_isOnDock != value)
                {
                    _isOnDock = value;
                    OnPropertyChanged(nameof(IsOnDock));
                }
            }
        }

        /// <summary>
        /// 是否能最大化
        /// </summary>
        public Boolean IsMax
        {
            get { return _isMax; }
            private set
            {
                if(_isMax != value)
                {
                    _isMax = value;
                    OnPropertyChanged(nameof(IsMax));
                }
            }
        }

        /// <summary>
        /// 是否打开后铺满全屏
        /// </summary>
        public Boolean IsFull
        {
            get { return _isFull; }
            private set
            {
                if(_isFull != value)
                {
                    _isFull = value;
                    OnPropertyChanged(nameof(IsFull));
                }
            }
        }

        /// <summary>
        /// 是否显示app底部的按钮
        /// </summary>
        public Boolean IsSetbar
        {
            get { return _isSetbar; }
            private set
            {
                if(_isSetbar != value)
                {
                    _isSetbar = value;
                    OnPropertyChanged(nameof(IsSetbar));
                }
            }
        }

        /// <summary>
        /// 是否打开最大化
        /// </summary>
        public Boolean IsOpenMax
        {
            get { return _isOpenMax; }
            private set
            {
                if(_isOpenMax != value)
                {
                    _isOpenMax = value;
                    OnPropertyChanged(nameof(IsOpenMax));
                }
            }
        }

        /// <summary>
        /// 是否锁定
        /// </summary>
        public Boolean IsLock
        {
            get { return _isLock; }
            private set
            {
                if(_isLock != value)
                {
                    _isLock = value;
                    OnPropertyChanged(nameof(IsLock));
                }
            }
        }

        /// <summary>
        /// 是否为福莱希
        /// </summary>
        public Boolean IsFlash
        {
            get { return _isFlash; }
            private set
            {
                if(_isFlash != value)
                {
                    _isFlash = value;
                    OnPropertyChanged(nameof(IsFlash));
                }
            }
        }

        /// <summary>
        /// 是否可以拖动
        /// </summary>
        public Boolean IsDraw
        {
            get { return _isDraw; }
            private set
            {
                if(_isDraw != value)
                {
                    _isDraw = value;
                    OnPropertyChanged(nameof(IsDraw));
                }
            }
        }

        /// <summary>
        /// 是否可以拉伸
        /// </summary>
        public Boolean IsResize
        {
            get { return _isResize; }
            private set
            {
                if(_isResize != value)
                {
                    _isResize = value;
                    OnPropertyChanged(nameof(IsResize));
                }
            }
        }

        /// <summary>
        /// 成员类型
        /// </summary>
        public MemberType MemberType
        {
            get { return _memberType; }
            private set
            {
                if(_memberType != value)
                {
                    _memberType = value;
                    OnPropertyChanged(nameof(MemberType));
                }
            }
        }

        /// <summary>
        /// 桌面索引
        /// </summary>
        public Int32 DeskIndex
        {
            get { return _deskIndex; }
            private set
            {
                if(_deskIndex != value)
                {
                    _deskIndex = value;
                    OnPropertyChanged(nameof(DeskIndex));
                }
            }
        }

        /// <summary>
        /// 账户Id
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

        public Boolean IsIconByUpload
        {
            get { return _isIconByUpload; }
            private set
            {
                if(_isIconByUpload != value)
                {
                    _isIconByUpload = value;
                    OnPropertyChanged(nameof(IsIconByUpload));
                }
            }
        }

        #endregion

        #region public ctor

        /// <summary>
        /// 实例化一个成员对象
        /// </summary>
        public Member(
            String name,
            String iconUrl,
            String appUrl,
            Int32 appId,
            Int32 width,
            Int32 height,
            Boolean isLock = default(Boolean),
            Boolean isMax = default(Boolean),
            Boolean isFull = default(Boolean),
            Boolean isSetbar = default(Boolean),
            Boolean isOpenMax = default(Boolean),
            Boolean isFlash = default(Boolean),
            Boolean isDraw = default(Boolean),
            Boolean isResize = default(Boolean))
        {
            AppId = appId;
            Width = width > 800 ? 800 : width;
            Height = height > 600 ? 600 : height;
            IsDraw = isDraw;
            IsOpenMax = isOpenMax;
            IsSetbar = isSetbar;
            IsMax = isMax;
            IsFull = isFull;
            IsFlash = isFlash;
            IsResize = isResize;
            IsLock = isLock;
            Name = name;
            IconUrl = iconUrl;
            AppUrl = appUrl;
            MemberType = appId == 0 ? MemberType.Folder : MemberType.App;
            DeskIndex = 1;
            IsIconByUpload = false;
        }

        /// <summary>
        /// 实例化一个成员对象
        /// </summary>
        public Member(String name, String iconUrl, Int32 appId)
        {
            AppId = appId;
            Width = 800;
            Height = 600;
            IsDraw = false;
            IsOpenMax = false;
            Name = name;
            IconUrl = iconUrl;
            DeskIndex = 1;
            MemberType = appId == 0 ? MemberType.Folder : MemberType.App;
            IsIconByUpload = false;
        }

        public Member()
        {
        }

        #endregion
    }

    /// <summary>
    /// MemberExtension
    /// </summary>
    public partial class Member
    {

    }
}
