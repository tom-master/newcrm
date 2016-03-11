using System;
using System.Reflection;

namespace NewCRM.Infrastructure.CommonTools.CustomExtension
{
    /// <summary>
    ///     枚举扩展方法类
    /// </summary>
    public static class EnumExtensions
    {
        public static String GetDescription(this Enum enumeration)
        {
            var type = enumeration.GetType();
            var members = type.GetMember(enumeration.CastTo<String>());
            return members.Length > 0 ? members[0].ToDescription() : enumeration.CastTo<String>();
        }
    }
}