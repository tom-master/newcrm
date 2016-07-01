using System.ComponentModel;

namespace NewCRM.Domain.ValueObject
{
    /// <summary>
    /// app标签
    /// </summary>
    public enum AppTag
    {
        [Description("最新")]
        New = 1,
        [Description("最热")]
        Hot = 2,
        [Description("最高评价")]
        TopEvaluate = 3
    }
}
