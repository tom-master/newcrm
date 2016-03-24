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
        /// <summary>
        /// 验证引用类型是否合法
        /// </summary>
        /// <param name="vaildateParameters"></param>
        public static void Vaildate(params Object[] vaildateParameters)
        {
            vaildateParameters.ToList().ForEach(parameter =>
            {
                var parameterString = parameter.GetType().Name;
                if (parameter == null)
                {
                    ArgumentNullException e = new ArgumentNullException(parameterString);
                    throw ThrowComponentException($"参数 {parameterString} 为空引发异常。", e);
                }
            });
        }
        /// <summary>
        /// 验证值类型是否合法
        /// </summary>
        /// <param name="canZero"></param>
        /// <param name="parameters"></param>
        public static void Vaildate(bool canZero = false, params ValueType[] parameters)
        {
            parameters.ToList().ForEach(parameter =>
            {
                Type type = parameter.GetType();
                if (type.IsValueType && type.IsNumeric())
                {
                    bool flag = !canZero ? parameter.CastTo(0.0) <= 0.0 : parameter.CastTo(0.0) < 0.0;
                    if (flag)
                    {
                        throw ThrowComponentException($"参数 {parameter.GetType().Name} 不在有效范围内引发异常。具体信息请查看系统日志。", new ArgumentOutOfRangeException(parameter.GetType().Name));
                    }
                }
            });
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