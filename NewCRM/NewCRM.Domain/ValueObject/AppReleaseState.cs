using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Domain.Entities.ValueObject
{
    /// <summary>
    /// app发布状态
    /// </summary>
    public enum AppReleaseState
    {
        [Description("已发布")]
        Release = 1,
        [Description("未发布")]
        UnRelease = 0
    }
}
