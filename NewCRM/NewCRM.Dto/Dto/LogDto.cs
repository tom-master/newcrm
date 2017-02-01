﻿using System;

namespace NewCRM.Dto.Dto
{
    public sealed class LogDto : BaseDto
    {
        /// <summary>
        /// 日志等级
        /// </summary>
        public Int32 LogLevelEnum { get; set; }

        /// <summary>
        /// 类名
        /// </summary>
        public String Controller { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>
        public String Action { get; set; }

        /// <summary>
        /// 异常信息
        /// </summary>
        public String ExceptionMessage { get; set; }

        /// <summary>
        /// 异常堆栈
        /// </summary>
        public String Track { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public Int32 AccountId { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public String AddTime { get; set; }
    }
}
