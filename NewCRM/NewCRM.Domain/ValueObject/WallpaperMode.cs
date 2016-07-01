using System.ComponentModel;

namespace NewCRM.Domain.ValueObject
{
    /// <summary>
    /// 显示方式
    /// </summary>
    public enum WallpaperMode
    {
        [Description("填充")]
        Fill = 1,
        [Description("适应")]
        Adapted = 2,
        [Description("平铺")]
        Tile = 3,
        [Description("拉伸")]
        Draw = 4,
        [Description("居中")]
        Center = 5
    }
}
