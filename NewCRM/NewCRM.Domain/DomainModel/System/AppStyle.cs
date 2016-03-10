using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Domain.DomainModel.System
{
    public enum AppStyle
    {
        [Description("窗体")]
        Window=1,
        [Description("挂件")]
        Widget =2
    }
}
