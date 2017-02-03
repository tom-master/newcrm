using System;

namespace NewCRM.Infrastructure.CommonTools.CustomHelper
{
    /// <summary>
    ///     数据辅助操作类
    /// </summary>
    public static class DataBaseErrorResponse
    {
        public static String ReturnDatabaseErrorMessage(DataBaseErrorType dataBaseErrorType)
        {
            String msg;
            switch (dataBaseErrorType)
            {
                case DataBaseErrorType.ConnectionTimeout:
                    msg = "连接数据库超时，请检查网络连接或者数据库服务器是否正常。";
                    break;
                case DataBaseErrorType.NotExistOrAccessDenied:
                    msg = "数据库服务不存在或拒绝访问。";
                    break;
                case DataBaseErrorType.ServiceStop:
                    msg = "数据库服务已暂停，不能提供数据服务。";
                    break;
                case DataBaseErrorType.TableNotExist:
                    msg = "指定名称的表不存在。";
                    break;
                case DataBaseErrorType.DatabaseInvalid:
                    msg = "所连接的数据库无效。";
                    break;
                case DataBaseErrorType.LoginFail:
                    msg = "使用设定的用户名与密码登录数据库失败。";
                    break;
                case DataBaseErrorType.ForeignKeyConstraintException:
                    msg = "外键约束，无法保存数据的变更。";
                    break;
                case DataBaseErrorType.PrimaryKeyRepeat:
                    msg = "主键重复，无法插入数据。";
                    break;
                case DataBaseErrorType.UnknowError:
                    msg = "未知错误。";
                    break;
                default:
                    throw new BusinessException(nameof(dataBaseErrorType), dataBaseErrorType, null);
            }
            return msg;
        }
    }
}