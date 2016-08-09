using System;
using System.Linq;
using System.Text;

namespace NewCRM.Infrastructure.CommonTools.CustomExtension
{
    /// <summary>
    ///     枚举扩展方法类
    /// </summary>
    public static class EnumExtensions
    {
        public static String GetDescription(this Enum enumeration)
        {
            var returnValue = new StringBuilder();
            var type = enumeration.GetType();
            var enumArray = enumeration.ToString().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var members in enumArray.Select(enumItem => type.GetMember(enumItem.Trim())).Where(members => members.Length > 0))
            {
                returnValue.Append(members[0].ToDescription() + " 或 ");
            }
            Char[] replaceChar = { ' ', '或', ' ' };
            return returnValue.ToString().TrimEnd(replaceChar);
        }
    }
}