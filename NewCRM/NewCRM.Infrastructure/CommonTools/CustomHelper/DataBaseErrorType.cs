using System.ComponentModel;

namespace NewCRM.Infrastructure.CommonTools.CustomHelper
{
    public enum DataBaseErrorType
    {
        [Description("连接超时")]
        ConnectionTimeout = 1,

        [Description("不存在或访问拒绝")]
        NotExistOrAccessDenied = 2,

        [Description("服务停止")]
        ServiceStop = 3,

        [Description("表不存在")]
        TableNotExist = 4,

        [Description("数据库无效")]
        DatabaseInvalid = 5,

        [Description("登陆失败")]
        LoginFail = 6,

        [Description("外键约束异常")]
        ForeignKeyConstraintException = 7,

        [Description("主键重复")]
        PrimaryKeyRepeat = 8,

        [Description("未知错误")]
        UnknowError = 9
    }
}
