using System;
using System.Linq;
using Newtonsoft.Json;

namespace NewCRM.Infrastructure.CommonTools.CustomHelper
{
    /// <summary>
    /// 字符串
    /// </summary>
    public sealed class StringHelper
    {
        private static Parameter _parameter = new Parameter();

        public static Int32[] GetIntArrayFromString(String commaSeparatedString)
        {
            _parameter.Validate(commaSeparatedString);
            return String.IsNullOrEmpty(commaSeparatedString) ? new Int32[0] : commaSeparatedString.Split(',').Select(s => Convert.ToInt32(s)).ToArray();
        }

        /// <summary>
        ///     把对象转换成Json字符串表示形式
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        public static String BuildJsonString(object jsonObject)
        {
            _parameter.Validate(jsonObject);
            return JsonConvert.SerializeObject(jsonObject);
        }

        /// <summary>
        ///     判断指定字符串是否对象（Object）类型的Json字符串格式
        /// </summary>
        /// <param name="input">要判断的Json字符串</param>
        /// <returns></returns>
        public static bool IsJsonObjectString(String input)
        {
            _parameter.Validate(input);
            return input != null && input.StartsWith("{") && input.EndsWith("}");
        }

        /// <summary>
        ///     判断指定字符串是否集合类型的Json字符串格式
        /// </summary>
        /// <param name="input">要判断的Json字符串</param>
        /// <returns></returns>
        public static bool IsJsonArrayString(String input)
        {
            _parameter.Validate(input);
            return input != null && input.StartsWith("[") && input.EndsWith("]");
        }
    }
}
