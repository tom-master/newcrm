using System;
using System.Globalization;

namespace NewCRM.Infrastructure.CommonTools
{
    /// <summary>
    /// 常量字段类
    /// </summary>
    public sealed class ConstString
    {
        /// <summary>
        /// 
        /// </summary>
        public sealed class OtherString
        {
            public const String TempDataPagevalue = "TempDataPageValue";
            public const String TempDataUserInfo = "UserInfo";
            /// <summary>
            /// 常量字段 
            /// </summary>
            public const String UserOnlineTime = "userOnlineTime";
            public const String CheckPowerFaliPageMessage = "您没有权限访问此页面";
            public const String CheckPowerFaliActionMessage = "您没有操作此页面的权限";
            public const String UserPowerList = "User_Power_List";
            /// <summary>
            /// 返回一个以字符串表示的日期
            /// </summary>
            public static String LocalTimeToString
            {
                get { return DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture); }
            }
            /// <summary>
            /// 返回一个以日期类型的日期
            /// </summary>
            public static DateTime LocalTimeToDate
            {
                get { return DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)); }
            }
        }
        /// <summary>
        /// 页面访问权限
        /// </summary>
        public sealed class ViewPowerString
        {
            /// <summary>
            /// 部门
            /// </summary>
            public const String CoreDeptView = "CoreDeptView";
            public const String CoreDeptEdit = "CoreDeptEdit";
            public const String CoreDeptNew = "CoreDeptNew";
            public const String CoreDeptUserView = "CoreDeptUserView";
            public const String CoreDeptUserNew = "CoreDeptUserNew";
            /// <summary>
            /// 日志
            /// </summary>
            public const String CoreLogView = "CoreLogView";
            /// <summary>
            /// 配置
            /// </summary>
            public const String CoreConfigView = "CoreConfigView";
            /// <summary>
            /// 菜单
            /// </summary>
            public const String CoreMenuView = "CoreMenuView";
            public const String CoreMenuEdit = "CoreMenuEdit";
            public const String CoreMenuNew = "CoreMenuNew";
            public const String CoreOnlineView = "CoreOnlineView";
            /// <summary>
            /// 权限
            /// </summary>
            public const String CorePowerView = "CorePowerView";
            public const String CorePowerEdit = "CorePowerEdit";
            public const String CorePowerNew = "CorePowerNew";
            /// <summary>
            /// 脚色
            /// </summary>
            public const String CoreRoleView = "CoreRoleView";
            public const String CoreRoleEdit = "CoreRoleEdit";
            public const String CoreRoleNew = "CoreRoleNew";
            public const String CoreRolePowerView = "CoreRolePowerView";
            public const String CoreRoleUserView = "CoreRoleUserView";
            public const String CoreRoleUserNew = "CoreRoleUserNew";
            /// <summary>
            /// 标题
            /// </summary>
            public const String CoreTitleView = "CoreTitleView";
            public const String CoreTitleEdit = "CoreTitleEdit";
            public const String CoreTitleNew = "CoreTitleNew";
            public const String CoreTitleUserView = "CoreTitleUserView";
            public const String CoreTitleUserNew = "CoreTitleUserNew";
            /// <summary>
            /// 用户
            /// </summary>
            public const String CoreUserView = "CoreUserView";
            public const String CoreUserChangePassword = "CoreUserChangePassword";
            public const String CoreUserEdit = "CoreUserEdit";
            public const String CoreUserNew = "CoreUserNew";
        }
        /// <summary>
        /// 控件操作权限
        /// </summary>
        public sealed class CheckPowerWithControl
        {
            /// <summary>
            /// 配置
            /// </summary>
            public const String CoreConfigEdit = "CoreConfigEdit";
            /// <summary>
            /// 部门
            /// </summary>
            public const String CoreDeptNew = "CoreDeptNew";
            public const String CoreDeptEdit = "CoreDeptEdit";
            public const String CoreDeptDelete = "CoreDeptDelete";
            public const String CoreDeptUserNew = "CoreDeptUserNew";
            public const String CoreDeptUserDelete = "CoreDeptUserDelete";
            /// <summary>
            /// 日志
            /// </summary>
            public const String CoreLogDelete = "CoreLogDelete";
            /// <summary>
            /// 菜单
            /// </summary>
            public const String CoreMenuNew = "CoreMenuNew";
            public const String CoreMenuEdit = "CoreMenuEdit";
            public const String CoreMenuDelete = "CoreMenuDelete";
            public const String CorePowerNew = "CorePowerNew";
            /// <summary>
            /// 权限
            /// </summary>
            public const String CorePowerEdit = "CorePowerEdit";
            public const String CoresPowerDelete = "CoresPowerDelete";
            /// <summary>
            /// 脚色
            /// </summary>
            public const String CoreRoleNew = "CoreRoleNew";
            public const String CoreRoleEdit = "CoreRoleEdit";
            public const String CoreRoleDelete = "CoreRoleDelete";
            public const String CoreRolePowerEdit = "CoreRolePowerEdit";
            public const String CoreRoleUserNew = "CoreRoleUserNew";
            public const String CoreRoleUserDelete = "CoreRoleUserDelete";
            /// <summary>
            /// 标题
            /// </summary>
            public const String CoreTitleNew = "CoreTitleNew";
            public const String CoreTitleEdit = "CoreTitleEdit";
            public const String CoreTitleDelete = "CoreTitleDelete";
            public const String CoreTitleUserNew = "CoreTitleUserNew";
            public const String CoreTitleUserDelete = "CoreTitleUserDelete";
            /// <summary>
            /// 用户
            /// </summary>
            public const String CoreUserEdit = "CoreUserEdit";
            public const String CoreUserDelete = "CoreUserDelete";
            public const String CoreUserNew = "CoreUserNew";
            public const String CoreUserChangePassword = "CoreUserChangePassword";
        }

    }
}
