using System;
using System.ComponentModel;
using NewCRM.Domain.ValueObject;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys.System
{
    [Description("日志"),Serializable]
    public  class Log : DomainModelBase, IAggregationRoot
    {
        #region public property

        /// <summary>
        /// 日志等级
        /// </summary>
        public LogLevel LogLevelEnum { get; private set; }

        /// <summary>
        /// 类名
        /// </summary>
        [Required()]
        public String Controller { get; private set; }

        /// <summary>
        /// 方法名
        /// </summary>
        [Required()]
        public String Action { get; private set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        [Required()]
        public String ExceptionMessage { get; private set; }

        /// <summary>
        /// 异常堆栈
        /// </summary>
        [Required()]
        public String Track { get; private set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public Int32 AccountId { get; private set; }
        #endregion

        #region ctor
        public Log(Int32 accountId, String controller, String action, LogLevel logLevel, String track, String exceptionMessage):this()
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
            AddTime = DateTime.Now;

        }




        #endregion
    }
}
