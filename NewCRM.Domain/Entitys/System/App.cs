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
                _name = value;
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
                _iconUrl = value;
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
                _appUrl = value;
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
                _remark = value;
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
                _width = value;
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
                _height = value;
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
                _useCount = value;
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
                _isMax = value;
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
                _isFull = value;
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
                _isSetbar = value;
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
                _isOpenMax = value;
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
                _isLock = value;
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
                _isSystem = value;
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
                _isFlash = value;
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
                _isDraw = value;
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
                _isResize = value;
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
                _accountId = value;
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
                _appTypeId = value;
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
                _isRecommand = value;
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
                _appAuditState = value;
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
                _appReleaseState = value;
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
                _appStyle = value;
            }
        }

        public Boolean IsInstall
        {
            get { return _isInstall; }
            private set
            {
                _isInstall = value;
            }
        }

        public Double StarCount
        {
            get { return _starCount; }
            private set
            {
                _starCount = value;
            }
        }

        public String AccountName
        {
            get { return _accountName; }
            private set
            {
                _accountName = value;
            }
        }

        public Boolean IsIconByUpload
        {
            get { return _isIconByUpload; }
            private set
            {
                _isIconByUpload = value;
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

    /// <summary>
    /// AppExtension
    /// </summary>
    public partial class App
    {

        public App ModifyName(String appName)
        {
            Name = appName;
            OnPropertyChanged(nameof(appName));
            return this;
        }

        public App ModifyIconUrl(String iconUrl)
        {
            IconUrl = iconUrl;
            OnPropertyChanged(nameof(iconUrl));
            return this;
        }

        public App ModifyWidth(Int32 width)
        {
            Width = width;
            OnPropertyChanged(nameof(Width));
            return this;
        }

        public App ModifyHeight(Int32 height)
        {
            Height = height;
            OnPropertyChanged(nameof(Height));
            return this;
        }

        public App ModifyUseCount(Int32 count)
        {
            UseCount = count;
            OnPropertyChanged(nameof(UseCount));
            return this;
        }

        public App ModifyIsMax(Boolean isMax)
        {
            IsMax = isMax;
            OnPropertyChanged(nameof(IsMax));
            return this;
        }

        public App ModifyIsFull(Boolean isFull)
        {
            IsFull = isFull;
            OnPropertyChanged(nameof(IsFull));
            return this;
        }

        public App ModifyIsSetbar(Boolean isSetbar)
        {
            IsSetbar = isSetbar;
            OnPropertyChanged(nameof(IsSetbar));
            return this;
        }

        public App ModifyIsOpenMax(Boolean isOpenMax)
        {
            IsOpenMax = isOpenMax;
            OnPropertyChanged(nameof(IsOpenMax));
            return this;
        }

        public App ModifyIsLock(Boolean isLock)
        {
            IsLock = isLock;
            OnPropertyChanged(nameof(IsLock));
            return this;
        }

        public App ModifyIsSystem(Boolean isSystem)
        {
            IsSystem = isSystem;
            OnPropertyChanged(nameof(IsSystem));
            return this;
        }

        public App ModifyIsFlash(Boolean isFlash)
        {
            IsFlash = isFlash;
            OnPropertyChanged(nameof(IsFlash));
            return this;
        }

        public App ModifyIsDraw(Boolean isDraw)
        {
            IsDraw = isDraw;
            OnPropertyChanged(nameof(IsDraw));
            return this;
        }

        public App ModifyIsResize(Boolean isResize)
        {
            IsResize = isResize;
            OnPropertyChanged(nameof(IsResize));
            return this;
        }

        public App ModifyAppTypeId(Int32 appTypeId)
        {
            AppTypeId = appTypeId;
            OnPropertyChanged(nameof(AppTypeId));
            return this;
        }

        public App Recommand()
        {
            IsRecommand = true;
            OnPropertyChanged(nameof(IsRecommand));
            return this;
        }

        public App CancelRecommand()
        {
            IsRecommand = false;
            OnPropertyChanged(nameof(IsRecommand));
            return this;
        }

        public App AppRelease()
        {
            AppReleaseState = NewCRM.Domain.ValueObject.AppReleaseState.Release;
            OnPropertyChanged(nameof(AppReleaseState));
            return this;
        }

        public App AppUnrelease()
        {
            AppReleaseState = NewCRM.Domain.ValueObject.AppReleaseState.UnRelease;
            OnPropertyChanged(nameof(AppReleaseState));
            return this;
        }

        public App ModifyAppAuditState(AppAuditState appAudit)
        {
            AppAuditState = appAudit;
            OnPropertyChanged(nameof(AppAuditState));
            return this;
        }

        public App ModifyAppStyle(AppStyle appStyle)
        {
            AppStyle = appStyle;
            OnPropertyChanged(nameof(AppStyle));
            return this;
        }

        public App IconNotFromUpload()
        {
            IsIconByUpload = false;
            OnPropertyChanged(nameof(IsIconByUpload));
            return this;
        }

        public App IconFromUpload()
        {
            IsIconByUpload = true;
            OnPropertyChanged(nameof(IsIconByUpload));
            return this;
        }
    }
}
