using System;
using System.Runtime.Serialization;

namespace NewCRM.Infrastructure.CommonTools.CustemException
{
    /// <summary>
    ///     仓储实现异常类
    /// </summary>
    [Serializable]
    public class RepositoryException : Exception
    {
        /// <summary>
        ///     实体化一个类的新实例
        /// </summary>
        public RepositoryException() { }

        /// <summary>
        ///     使用异常消息实例化一个类的新实例
        /// </summary>
        /// <param name="message">异常消息</param>
        public RepositoryException(string message)
            : base(message)
        {
            new ExceptionMessage(this, message, true);
        }

        /// <summary>
        ///     使用异常消息与一个内部异常实例化一个类的新实例
        /// </summary>
        /// <param name="message">异常消息</param>
        /// <param name="inner">用于封装在DalException内部的异常实例</param>
        public RepositoryException(string message, Exception inner)
            : base(message, inner)
        {
            new ExceptionMessage(inner, message, true);
        }

        /// <summary>
        ///     使用可序列化数据实例化一个类的新实例
        /// </summary>
        /// <param name="info">保存序列化对象数据的对象。</param>
        /// <param name="context">有关源或目标的上下文信息。</param>
        protected RepositoryException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}