using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using NewCRM.Infrastructure.CommonTools.CustomException;
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

            if (vaildateParameter is String)
            {
                return this;
            }

            #region 判断如果需要验证的参数是复杂类型的话 获取应用在类型的属性上的特性，并判断

            //判断如果需要验证的参数是复杂类型的话 获取应用在类型的属性上的特性，并判断
            var instance = vaildateParameter;

            var propertys = instance.GetType().GetProperties();

            foreach (var propertyInfo in propertys)
            {
                var propertyAttributes = propertyInfo.GetCustomAttributes().ToArray();

                if (!propertyAttributes.Any())
                {
                    continue;
                }

                foreach (var attribute in propertyAttributes)
                {
                    var value = propertyInfo.GetValue(instance);

                    if (attribute.GetType() == typeof(RequiredAttribute))
                    {
                        if ((value + "").Length <= 0)
                        {
                            var internalField = propertyInfo.Name;

                            throw new ArgumentException($"字段:{internalField}值不能为空");
                        }
                    }

                    if (attribute.GetType() == typeof(StringLengthAttribute))
                    {
                        var contentLength = ((StringLengthAttribute)attribute).MaximumLength;

                        if (((String)value).Length > contentLength)
                        {
                            var internalField = propertyInfo.Name;

                            throw new ArgumentException($"字段:{internalField}值长度不能超过:{contentLength}");
                        }
                    }
                }
            }

            #endregion

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
        private static ComponentException ThrowComponentException(String msg, Exception e = default(Exception))
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