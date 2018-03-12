using System;
using System.ComponentModel;
using NewCRM.Domain.ValueObject;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys.System
{
	[Description("日志"), Serializable]
	public class Log: DomainModelBase
	{
		#region private field

		private LogLevel _logLevelEnum;

		private String _controller;

		private String _action;

		private String _exceptionMessage;

		private String _track;

		private Int32 _accountId;
		#endregion

		#region public property

		/// <summary>
		/// 日志等级
		/// </summary>
		public LogLevel LogLevelEnum
		{
			get { return _logLevelEnum; }
			private set
			{
				if (_logLevelEnum != value)
				{
					_logLevelEnum = value;
					OnPropertyChanged("LogLevelEnum");
				}
			}
		}

		/// <summary>
		/// 类名
		/// </summary>
		[Required]
		public String Controller
		{
			get { return _controller; }
			private set
			{
				if (_controller != value)
				{
					_controller = value;
					OnPropertyChanged("Controller");
				}
			}
		}

		/// <summary>
		/// 方法名
		/// </summary>
		[Required]
		public String Action
		{
			get { return _action; }
			private set
			{
				if (_action != value)
				{
					_action = value;
					OnPropertyChanged("Action");
				}
			}
		}

		/// <summary>
		/// 异常信息
		/// </summary>
		[Required]
		public String ExceptionMessage
		{
			get { return _exceptionMessage; }
			private set
			{
				if (_exceptionMessage != value)
				{
					_exceptionMessage = value;
					OnPropertyChanged("ExceptionMessage");
				}
			}
		}

		/// <summary>
		/// 异常堆栈
		/// </summary>
		[Required]
		public String Track
		{
			get { return _track; }
			private set
			{
				if (_track != value)
				{
					_track = value;
					OnPropertyChanged("Track");
				}
			}
		}

		/// <summary>
		/// 用户id
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
		#endregion

		#region ctor
		public Log(Int32 accountId, String controller, String action, LogLevel logLevel, String track, String exceptionMessage)
		{
			AccountId = accountId;
			Controller = controller;
			Action = action;
			LogLevelEnum = logLevel;
			Track = track;
			ExceptionMessage = exceptionMessage;
		}

		public Log()
		{

		}
		#endregion

	}
}
