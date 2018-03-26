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
	public partial class Member: DomainModelBase
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
				_appId = value;
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
				_width = value;
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
				_height = value;
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
				_folderId = value;
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
		/// 成员是否在应用码头上
		/// </summary>
		public Boolean IsOnDock
		{
			get { return _isOnDock; }
			private set
			{
				_isOnDock = value;
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
		/// 成员类型
		/// </summary>
		public MemberType MemberType
		{
			get { return _memberType; }
			private set
			{
				_memberType = value;
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
				_deskIndex = value;
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

		public Boolean IsIconByUpload
		{
			get { return _isIconByUpload; }
			private set
			{
				_isIconByUpload = value;
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
			Int32 accountId,
			Int32 deskIndex,
			Boolean isIconByUpload = default(Boolean),
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
			IsIconByUpload = isIconByUpload;
			AccountId = accountId;
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

		public Member ModifyWidth(Int32 width)
		{
			if (width <= 0)
			{
				throw new ArgumentException($@"{nameof(width)} less than or equal to zero");
			}

			Width = width;
			OnPropertyChanged(nameof(Width));
			return this;
		}

		public Member ModifyHeight(Int32 height)
		{
			if (height <= 0)
			{
				throw new ArgumentException($@"{nameof(height)} less than or equal to zero");
			}

			Height = height;
			OnPropertyChanged(nameof(Height));
			return this;
		}

		public Member ModifyFolderId(Int32 folderId)
		{
			if (folderId <= 0)
			{
				throw new ArgumentException($@"{nameof(folderId)} less than or equal to zero");
			}

			FolderId = folderId;
			OnPropertyChanged(nameof(FolderId));
			return this;
		}

		public Member ModifyName(String name)
		{
			if (String.IsNullOrEmpty(name))
			{
				throw new ArgumentException($@"{nameof(name)} is null");
			}

			Name = name;
			OnPropertyChanged(nameof(Name));
			return this;
		}

		public Member ModifyIconUrl(String iconUrl)
		{
			if (String.IsNullOrEmpty(iconUrl))
			{
				throw new ArgumentException($@"{nameof(iconUrl)} is null");
			}

			IconUrl = iconUrl;
			OnPropertyChanged(nameof(IconUrl));
			return this;
		}

		public Member ModifyAppUrl(String appUrl)
		{
			if (String.IsNullOrEmpty(appUrl))
			{
				throw new ArgumentException($@"{nameof(appUrl)} is null");
			}

			AppUrl = appUrl;
			OnPropertyChanged(nameof(AppUrl));
			return this;
		}

		public Member OnDock()
		{
			IsOnDock = true;
			OnPropertyChanged(nameof(IsOnDock));
			return this;
		}

		public Member OutDock()
		{
			IsOnDock = false;
			OnPropertyChanged(nameof(IsOnDock));
			return this;
		}

		public Member ModifyIsMax(Boolean isMax)
		{
			IsMax = isMax;
			OnPropertyChanged(nameof(IsMax));
			return this;
		}

		public Member ModifyIsFull(Boolean isFull)
		{
			IsFull = isFull;
			OnPropertyChanged(nameof(IsFull));
			return this;
		}

		public Member ModifyIsSetbar(Boolean isSetbar)
		{
			IsSetbar = isSetbar;
			OnPropertyChanged(nameof(IsSetbar));
			return this;
		}

		public Member ModifyIsOpenMax(Boolean isOpenMax)
		{
			IsOpenMax = isOpenMax;
			OnPropertyChanged(nameof(IsOpenMax));
			return this;
		}

		public Member ModifyIsLock(Boolean isLock)
		{
			IsLock = isLock;
			OnPropertyChanged(nameof(IsLock));
			return this;
		}

		public Member ModifyIsFlash(Boolean isFlash)
		{
			IsFlash = isFlash;
			OnPropertyChanged(nameof(IsFlash));
			return this;
		}

		public Member ModifyIsDraw(Boolean isDraw)
		{
			IsDraw = isDraw;
			OnPropertyChanged(nameof(IsDraw));
			return this;
		}

		public Member ModifyIsResize(Boolean isResize)
		{
			IsResize = isResize;
			OnPropertyChanged(nameof(IsResize));
			return this;
		}

		public Member ModifyDeskIndex(Int32 deskIndex)
		{
			if (deskIndex <= 0)
			{
				throw new ArgumentException($@"{nameof(deskIndex)} less than or equal to zero");
			}

			DeskIndex = deskIndex;
			OnPropertyChanged(nameof(DeskIndex));
			return this;
		}

		public Member IconNotFromUpload()
		{
			IsIconByUpload = false;
			OnPropertyChanged(nameof(IsIconByUpload));
			return this;
		}

		public Member IconFromUpload()
		{
			IsIconByUpload = true;
			OnPropertyChanged(nameof(IsIconByUpload));
			return this;
		}
	}
}
