using System;
using System.ComponentModel;
using NewCRM.Domain.ValueObject;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys.System
{
	[Description("用户配置"), Serializable]
	public partial class Config: DomainModelBase
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
				if (_skin != value)
				{
					_skin = value;
					OnPropertyChanged("Skin");
				}
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
				if (_accountFace != value)
				{
					_accountFace = value;
					OnPropertyChanged("AccountFace");
				}
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
				if (_appSize != value)
				{
					_appSize = value;
					OnPropertyChanged("AppSize");
				}
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
				if (_appVerticalSpacing != value)
				{
					_appVerticalSpacing = value;
					OnPropertyChanged("AppVerticalSpacing");
				}
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
				if (_appHorizontalSpacing != value)
				{
					_appHorizontalSpacing = value;
					OnPropertyChanged("AppHorizontalSpacing");
				}
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
				if (_defaultDeskNumber != value)
				{
					_defaultDeskNumber = value;
					OnPropertyChanged("DefaultDeskNumber");
				}
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
				if (_defaultDeskCount != value)
				{
					_defaultDeskCount = value;
					OnPropertyChanged("DefaultDeskCount");
				}
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
				if (_wallpaperMode != value)
				{
					_wallpaperMode = value;
					OnPropertyChanged("WallpaperMode");
				}
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
				if (_isBing != value)
				{
					_isBing = value;
					OnPropertyChanged("IsBing");
				}
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
				if (_appXy != value)
				{
					_appXy = value;
					OnPropertyChanged("AppXy");
				}
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
				if (_dockPosition != value)
				{
					_dockPosition = value;
					OnPropertyChanged("DockPosition");
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
				if (_accountId != value)
				{
					_accountId = value;
					OnPropertyChanged("AccountId");
				}
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
				if (_wallpaperId != value)
				{
					_wallpaperId = value;
					OnPropertyChanged("WallpaperId");
				}
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
}
