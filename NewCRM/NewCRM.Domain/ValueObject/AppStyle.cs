using System.ComponentModel;

namespace NewCRM.Domain.Entities.ValueObject
{
    /// <summary>
    /// app的样式
    /// </summary>
    public enum AppStyle
    {
        [Description("窗体")]
        App = 1,
        [Description("挂件")]
        Widget =2
    }
}
