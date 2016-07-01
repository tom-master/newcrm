using System.ComponentModel;

namespace NewCRM.Domain.ValueObject
{
    /// <summary>
    /// 码头位置
    /// </summary>
    public enum DockPostion
    {
        [Description("上")]
        Top = 1,
        [Description("左")]
        Left = 2,
        [Description("右")]
        Right = 3,
        [Description("下")]
        Buttom = 4
    }
}
