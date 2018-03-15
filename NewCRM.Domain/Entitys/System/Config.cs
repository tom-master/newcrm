using System;
using System.ComponentModel;
using NewCRM.Domain.ValueObject;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys.System
{
    [Description("用户配置"), Serializable]
    public partial class Config : DomainModelBase
    {
        #region private field

        private String _skin;

        private Int32 _appSize;

        private String _accountFace;

        private Int32 _appVerticalSpacing;

        private Int32 _appHorizontalSpacing;

        private Int32 _defaultDeskNumber;

        private Int32 _defaultDeskCount;

        private WallpaperMode _wallpaperMode;

        private Boolean _isBing;

        private AppAlignMode _appXy;

        private DockPostion _dockPosition;

        private Int32 _accountId;

        private Int32 _wallpaperId;

        #endregion

        #region public property

        /// <summary>
        /// 皮肤
        /// </summary>
        [Required]
        public String Skin
        {
            get { return _skin; }
            private set
            {
                _skin = value;
            }
        }

        /// <summary>
        /// 用户头像
        /// </summary>
        [Required]
        public String AccountFace
        {
            get { return _accountFace; }
            private set
            {
                _accountFace = value;
            }
        }

        /// <summary>
        /// app尺寸
        /// </summary>
        public Int32 AppSize
        {
            get { return _appSize; }
            private set
            {
                _appSize = value;
            }
        }

        /// <summary>
        /// app垂直间距
        /// </summary>
        public Int32 AppVerticalSpacing
        {
            get { return _appVerticalSpacing; }
            private set
            {
                _appVerticalSpacing = value;
            }
        }

        /// <summary>
        /// app水平间距
        /// </summary>
        public Int32 AppHorizontalSpacing
        {
            get { return _appHorizontalSpacing; }
            private set
            {
                _appHorizontalSpacing = value;
            }
        }

        /// <summary>
        /// 默认桌面编号
        /// </summary>
        public Int32 DefaultDeskNumber
        {
            get { return _defaultDeskNumber; }
            private set
            {
                _defaultDeskNumber = value;
            }
        }

        /// <summary>
        /// 默认桌面数量
        /// </summary>
        public Int32 DefaultDeskCount
        {
            get { return _defaultDeskCount; }
            private set
            {
                _defaultDeskCount = value;
            }
        }

        /// <summary>
        /// 壁纸的展示模式
        /// </summary>
        public WallpaperMode WallpaperMode
        {
            get { return _wallpaperMode; }
            private set
            {
                _wallpaperMode = value;
            }
        }

        /// <summary>
        /// 壁纸来源
        /// </summary>
        public Boolean IsBing
        {
            get { return _isBing; }
            private set
            {
                _isBing = value;
            }
        }

        /// <summary>
        /// app排列方向
        /// </summary>
        public AppAlignMode AppXy
        {
            get { return _appXy; }
            private set
            {
                _appXy = value;
            }
        }

        /// <summary>
        /// 码头位置
        /// </summary>
        public DockPostion DockPosition
        {
            get { return _dockPosition; }
            private set
            {
                _dockPosition = value;
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
                _accountId = value;
            }
        }

        /// <summary>
        /// 壁纸Id
        /// </summary>
        public Int32 WallpaperId
        {
            get { return _wallpaperId; }
            private set
            {
                _wallpaperId = value;
            }
        }

        #endregion

        #region ctor

        public Config()
        {
            AppXy = AppAlignMode.X;
            DockPosition = DockPostion.Top;
            AccountFace = @"\Scripts\HoorayUI\img\ui\avatar_48.jpg";
            Skin = "default";
            WallpaperMode = WallpaperMode.Fill;
            AppSize = 48;
            AppVerticalSpacing = 50;
            AppHorizontalSpacing = 50;
            DefaultDeskNumber = 1;
            DefaultDeskCount = 5;
        }

        #endregion
    }

    /// <summary>
    /// ConfigExtension
    /// </summary>
    public partial class Config
    {

        public Config ModifySkin(String skin)
        {
            if(String.IsNullOrEmpty(skin))
            {
                throw new ArgumentException($@"{nameof(skin)} is null");
            }

            Skin = skin;
            OnPropertyChanged(nameof(Skin));
            return this;
        }

        public Config ModifyAccountFace(String accountFace)
        {
            if(String.IsNullOrEmpty(accountFace))
            {
                throw new ArgumentException($@"{nameof(accountFace)} is null");
            }

            AccountFace = accountFace;
            OnPropertyChanged(nameof(AccountFace));
            return this;
        }

        public Config ModifyAppSize(Int32 appSize)
        {
            if(appSize <= 0)
            {
                throw new ArgumentException($@"{nameof(appSize)} less than or equal to zero");
            }

            AppSize = appSize;
            OnPropertyChanged(nameof(AppSize));
            return this;
        }

        public Config ModifyAppVerticalSpacing(Int32 appVerticalSpacing)
        {
            if(appVerticalSpacing <= 0)
            {
                throw new ArgumentException($@"{nameof(appVerticalSpacing)} less than or equal to zero");
            }

            AppVerticalSpacing = appVerticalSpacing;
            OnPropertyChanged(nameof(AppVerticalSpacing));
            return this;
        }

        public Config ModifyAppHorizontalSpacing(Int32 appHorizontalSpacing)
        {
            if(appHorizontalSpacing <= 0)
            {
                throw new ArgumentException($@"{nameof(appHorizontalSpacing)} less than or equal to zero");
            }

            AppHorizontalSpacing = appHorizontalSpacing;
            OnPropertyChanged(nameof(AppHorizontalSpacing));
            return this;
        }

        public Config ModifyDefaultDeskNumber(Int32 deskNumber)
        {
            if(deskNumber <= 0)
            {
                throw new ArgumentException($@"{nameof(deskNumber)} less than or equal to zero");
            }

            DefaultDeskNumber = deskNumber;
            OnPropertyChanged(nameof(DefaultDeskNumber));
            return this;
        }

        public Config ModifyDefaultDeskCount(Int32 deskCount)
        {
            if(deskCount <= 0)
            {
                throw new ArgumentException($@"{nameof(deskCount)} less than or equal to zero");
            }

            DefaultDeskCount = deskCount;
            OnPropertyChanged(nameof(DefaultDeskCount));
            return this;
        }

        public Config ModifyWallpaperMode(WallpaperMode wallpaperMode)
        {
            WallpaperMode = wallpaperMode;
            OnPropertyChanged(nameof(WallpaperMode));
            return this;
        }

        public Config FromBing()
        {
            IsBing = true;
            OnPropertyChanged(nameof(IsBing));
            return this;
        }

        public Config NotFromBing()
        {
            IsBing = false;
            OnPropertyChanged(nameof(IsBing));
            return this;
        }

        public Config ModifyAppX()
        {
            AppXy = AppAlignMode.X;
            OnPropertyChanged(nameof(AppXy));
            return this;
        }

        public Config ModifyAppY()
        {
            AppXy = AppAlignMode.Y;
            OnPropertyChanged(nameof(AppXy));
            return this;
        }

        public Config ModifyDockPosition(DockPostion dockPostion)
        {
            DockPosition = dockPostion;
            OnPropertyChanged(nameof(DockPosition));
            return this;
        }

        public Config ModifyWallpaperId(Int32 wallpaperId)
        {
            WallpaperId = wallpaperId;
            OnPropertyChanged(nameof(WallpaperId));
            return this;
        }
    }
}
