using System;
using System.ComponentModel;
using NewCRM.Domain.ValueObject;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys.System
{
    [Description("日志"), Serializable]
    public partial class Log : DomainModelBase
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
                _logLevelEnum = value;
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
                _controller = value;
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
                _action = value;
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
                _exceptionMessage = value;
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
                _track = value;
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
                _accountId = value;
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
