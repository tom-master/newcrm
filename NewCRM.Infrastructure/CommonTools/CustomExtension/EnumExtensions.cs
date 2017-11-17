using System;
using System.Linq;
using System.Text;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Infrastructure.CommonTools.CustomExtension
{
    /// <summary>
    /// 枚举扩展方法类
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举描述
        /// </summary>
        /// <param name="enumeration"></param>
        /// <returns></returns>
        public static String GetDescription(this Enum enumeration)
        {
            var returnValue = new StringBuilder();
            var type = enumeration.GetType();
            var enumArray = enumeration.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach(var members in enumArray.Select(enumItem => type.GetMember(enumItem.Trim())).Where(members => members.Length > 0))
            {
                returnValue.Append(members[0].ToDescription() + " 或 ");
            }

            Char[] replaceChar = { ' ', '或', ' ' };

            return returnValue.ToString().TrimEnd(replaceChar);
        }

        /// <summary>
        /// 参数转换为枚举类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ParseToEnum<T>(Object value) where T : struct
        {
            var enumConst = Enum.GetName(typeof(T), value);
            T t;
            if(Enum.TryParse(enumConst, true, out t))
            {
                return t;
            }

            throw new BusinessException($"{value}不是有效的类型");
        }
    }
}