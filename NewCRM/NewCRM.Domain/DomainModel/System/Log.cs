using System;
using System.ComponentModel;
using NewCRM.Domain.Entities.ValueObject;

namespace NewCRM.Domain.Entities.DomainModel.System
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
        public String Controller { get; private set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public String Action { get; private set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public String ExceptionMessage { get; private set; }

        /// <summary>
        /// 异常堆栈
        /// </summary>
        public String Track { get; private set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public Int32 UserId { get; private set; }
        #endregion

        #region ctor
        public Log(Int32 userId, String controller, String action, LogLevel logLevel, String track, String exceptionMessage)
        {
            UserId = userId;
            Controller = controller;
            Action = action;
            LogLevelEnum = logLevel;
            Track = track;
            ExceptionMessage = exceptionMessage;
            AddTime = DateTime.Now;
        }

        public Log()
        {

        }

        #endregion


    }
}
