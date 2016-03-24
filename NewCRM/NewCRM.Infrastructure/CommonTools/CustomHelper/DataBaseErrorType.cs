namespace NewCRM.Infrastructure.CommonTools.CustomHelper
{
    public enum DataBaseErrorType
    {
        ConnectionTimeout = 1,
        NotExistOrAccessDenied = 2,
        ServiceStop = 3,
        TableNotExist = 4,
        DatabaseInvalid = 5,
        LoginFail = 6,
        ForeignKeyConstraintException = 7,
        PrimaryKeyRepeat = 8,
        UnknowError = 9
    }
}
