using System.ComponentModel;

namespace NewCRM.Domain.Entities.ValueObject
{
    /// <summary>
    /// App的审核状态
    /// </summary>
    public enum AppAuditState
    {
        [Description("审核中")]
        Wait = 1,
        [Description("通过")]
        Pass = 2,
        [Description("未通过")]
        Deny = 3
    }
}
