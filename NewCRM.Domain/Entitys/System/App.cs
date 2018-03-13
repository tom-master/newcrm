using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NewCRM.Domain.ValueObject;

namespace NewCRM.Domain.Entitys.System
{

    [Serializable, Description("应用")]
    public partial class App : DomainModelBase
    {
        #region private field

        public String _name;

        public String _iconUrl;

        public String _appUrl;

        public String _remark;

        public Int32 _width;

        public Int32 _height;

        public Int32 _useCount;

        public Boolean _isMax;

        public Boolean _isFull;

        public Boolean _isSetbar;

        public Boolean _isOpenMax;

        public Boolean _isLock;

        public Boolean _isSystem;

        public Boolean _isFlash;

        public Boolean _isDraw;

        public Boolean _isResize;

        public Int32 _accountId;

        public Int32 _appTypeId;

        public Boolean _isRecommand;

        public AppAuditState _appAuditState;

        public AppReleaseState _appReleaseState;

        public AppStyle _appStyle;

        public Boolean _isInstall;

        public Double _starCount;

        public String _accountName;

        public Boolean _isIconByUpload;
        #endregion

        #region public property

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
        /// 备注
        /// </summary>
        [MaxLength(50)]
        public String Remark
        {
            get { return _remark; }
            private set
            {
                if(_remark != value)
                {
                    _remark = value;
                    OnPropertyChanged(nameof(Remark));
                }
            }
        }

        /// <summary>
        /// 宽度
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
        /// 高度
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
        /// 使用数
        /// </summary>
        public Int32 UseCount
        {
            get { return _useCount; }
            private set
            {
                if(_useCount != value)
                {
                    _useCount = value;
                    OnPropertyChanged(nameof(UseCount));
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
        /// 是否为系统应用
        /// </summary>
        public Boolean IsSystem
        {
            get { return _isSystem; }
            private set
            {
                if(_isSystem != value)
                {
                    _isSystem = value;
                    OnPropertyChanged(nameof(IsSystem));
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
        /// 开发者(用户)Id
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
        /// App类型Id
        /// </summary>
        public Int32 AppTypeId
        {
            get { return _appTypeId; }
            private set
            {
                if(_appTypeId != value)
                {
                    _appTypeId = value;
                    OnPropertyChanged(nameof(AppTypeId));
                }
            }
        }

        /// <summary>
        /// 是否推荐
        /// </summary>
        public Boolean IsRecommand
        {
            get { return _isRecommand; }
            private set
            {
                if(_isRecommand != value)
                {
                    _isRecommand = value;
                    OnPropertyChanged(nameof(IsRecommand));
                }
            }
        }

        /// <summary>
        /// 审核状态
        /// </summary>
        public AppAuditState AppAuditState
        {
            get { return _appAuditState; }
            private set
            {
                if(_appAuditState != value)
                {
                    _appAuditState = value;
                    OnPropertyChanged(nameof(AppAuditState));
                }
            }
        }

        /// <summary>
        /// 发布状态
        /// </summary>
        public AppReleaseState AppReleaseState
        {
            get { return _appReleaseState; }
            private set
            {
                if(_appReleaseState != value)
                {
                    _appReleaseState = value;
                    OnPropertyChanged(nameof(AppReleaseState));
                }
            }
        }

        /// <summary>
        /// app样式
        /// </summary>
        public AppStyle AppStyle
        {
            get { return _appStyle; }
            private set
            {
                if(_appStyle != value)
                {
                    _appStyle = value;
                    OnPropertyChanged(nameof(AppStyle));
                }
            }
        }

        public Boolean IsInstall
        {
            get { return _isInstall; }
            private set
            {
                if(_isInstall != value)
                {
                    _isInstall = value;
                    OnPropertyChanged(nameof(IsInstall));
                }
            }
        }

        public Double StarCount
        {
            get { return _starCount; }
            private set
            {
                if(_starCount != value)
                {
                    _starCount = value;
                    OnPropertyChanged(nameof(StarCount));
                }
            }
        }

        public String AccountName
        {
            get { return _accountName; }
            private set
            {
                if(_accountName != value)
                {
                    _accountName = value;
                    OnPropertyChanged(nameof(AccountName));
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

        #region ctor

        /// <summary>
        /// 实例化一个app对象
        /// </summary>
        public App(String name,
            String iconUrl,
            String appUrl,
            Int32 width,
            Int32 height,
            Int32 appTypeId,
            AppAuditState appAuditState,
            AppReleaseState appReleaseState,
            AppStyle appStyle = AppStyle.App,
            Int32 accountId = default(Int32),
            String remark = default(String),
            Boolean isMax = default(Boolean),
            Boolean isFull = default(Boolean),
            Boolean isSetbar = default(Boolean),
            Boolean isOpenMax = default(Boolean),
            Boolean isFlash = default(Boolean),
            Boolean isDraw = default(Boolean),
            Boolean isResize = default(Boolean),
            Boolean isIconByUpload = default(Boolean))
        {
            Name = name;
            IconUrl = iconUrl;
            AppUrl = appUrl;
            Width = width > 800 ? 800 : width;
            Height = height > 600 ? 600 : height;
            IsMax = isMax;
            IsFull = isFull;
            IsSetbar = isSetbar;
            IsOpenMax = isOpenMax;
            IsFlash = isFlash;
            IsDraw = isDraw;
            IsResize = isResize;
            AppTypeId = appTypeId;
            AppStyle = appStyle;
            if(accountId == 0)
            {
                IsSystem = true;
            }
            else
            {
                IsSystem = false;
                AccountId = accountId;
            }

            IsLock = false;
            Remark = remark;
            AppAuditState = appAuditState;
            AppReleaseState = appReleaseState;
            UseCount = 0;
            IsRecommand = false;
            IsIconByUpload = isIconByUpload;
        }

        public App() { }
        #endregion
    }
}
