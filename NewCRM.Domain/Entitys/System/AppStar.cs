using System;

namespace NewCRM.Domain.Entitys.System
{
	public partial class AppStar: DomainModelBase
	{
		private Int32 _accountId;

		public Int32 _appId;

		public Double _startNum;


		/// <summary>
		/// 账号
		/// </summary>
		public Int32 AccountId
		{
			get { return _accountId; }
			private set
			{
				if (_accountId != value)
				{
					_accountId = value;
					OnPropertyChanged(AccountId.GetType());
				}
			}
		}

		/// <summary>
		/// 应用Id
		/// </summary>
		public Int32 AppId
		{
			get { return _appId; }
			private set
			{
				if (_appId != value)
				{
					_appId = value;
					OnPropertyChanged(AppId.GetType());
				}
			}
		}

		/// <summary>
		/// 评分
		/// </summary>
		public Double StartNum
		{
			get { return _startNum; }
			private set
			{
				if (_startNum != value)
				{
					_startNum = value;
					OnPropertyChanged(StartNum.GetType());
				}
			}
		}

		public AppStar(Int32 accountId, Int32 appId, Double startNum)
		{
			AccountId = accountId;
			StartNum = startNum;
		}

		public AppStar()
		{
		}
	}
}
