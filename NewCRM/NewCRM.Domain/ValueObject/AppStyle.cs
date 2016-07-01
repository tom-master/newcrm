using System.ComponentModel;

namespace NewCRM.Domain.ValueObject
{
    /// <summary>
    /// app的样式
    /// </summary>
    public enum AppStyle
    {
        [Description("窗体")]
        Window=1,
        [Description("挂件")]
        Widget =2
    }
}
