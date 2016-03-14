using System;
using System.Linq;
using NewCRM.Infrastructure.CommonTools.CustemException;
using NewCRM.Infrastructure.CommonTools.CustomExtension;

namespace NewCRM.Infrastructure.CommonTools.CustomHelper
{
    /// <summary>
    ///     公共辅助操作类
    /// </summary>
    public class Parameter
    {
        #region 公共方法

        public static void Vaildate(params dynamic[] vaildateParameters)
        {
            vaildateParameters.ToList().ForEach(parameter =>
            {
                var parameterString = nameof(parameter);
                if (parameter == null)
                {
                    ArgumentNullException e = new ArgumentNullException(parameterString);
                    throw ThrowComponentException(String.Format("参数 {0} 为空引发异常。", parameterString), e);
                }
            });
        }

        public static void Vaildate(ValueType parameter, bool canZero = false)
        {
            var parameterString = nameof(parameter);
            if (parameter == null)
            {
                ArgumentNullException e = new ArgumentNullException(parameterString);
                throw ThrowComponentException(String.Format("参数 {0} 为空引发异常。", parameterString), e);
            }
            Type type = parameter.GetType();
            if (type.IsValueType && type.IsNumeric())
            {
                if (!canZero)
                {
                    throw ThrowComponentException(String.Format("参数 {0} 不在有效范围内引发异常。具体信息请查看系统日志。", parameterString), new ArgumentOutOfRangeException(parameterString));
                }
            }
        }

        /// <summary>
        ///     向调用层抛出组件异常
        /// </summary>
        /// <param name="msg"> 自定义异常消息 </param>
        /// <param name="e"> 实际引发异常的异常实例 </param>
        private static ComponentException ThrowComponentException(String msg, Exception e = null)
        {
            if (String.IsNullOrEmpty(msg) && e != null)
            {
                msg = e.Message;
            }
            else if (String.IsNullOrEmpty(msg))
            {
                msg = "未知组件异常，详情请查看日志信息。";
            }
            return e == null ? new ComponentException(String.Format("组件异常：{0}", msg)) : new ComponentException(String.Format("组件异常：{0}", msg), e);
        }

        #endregion
    }
}