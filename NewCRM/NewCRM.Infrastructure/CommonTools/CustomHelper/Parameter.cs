using System;
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
        /// <summary>
        /// 验证引用类型是否合法
        /// </summary>
        /// <param name="vaildateParameter"></param>
        public Parameter Validate(Object vaildateParameter)
        {
            if (vaildateParameter == null)
            {
                throw ThrowComponentException($"参数 {nameof(vaildateParameter)} 为空引发异常。");
            }
            return this;
        }

        /// <summary>
        /// 验证值类型是否合法
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="canZero"></param>
        public Parameter Validate(ValueType parameter, Boolean canZero = false)
        {
            Type type = parameter.GetType();
            if (type.IsValueType && type.IsNumeric())
            {
                Boolean flag = !canZero ? parameter.CastTo(0.0) <= 0.0 : parameter.CastTo(0.0) < 0.0;
                if (flag)
                {
                    throw ThrowComponentException($"参数 {parameter.GetType().Name} 不在有效范围内引发异常。具体信息请查看系统日志。", new ArgumentOutOfRangeException(parameter.GetType().Name));
                }
            }
            return this;
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
            return e == null ? new ComponentException($"组件异常：{msg}") : new ComponentException($"组件异常：{msg}", e);
        }

        #endregion
    }
}