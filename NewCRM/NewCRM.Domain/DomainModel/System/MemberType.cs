using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Domain.DomainModel.System
{
    public enum MemberType
    {
        [Description("应用")]
        App = 1,
        [Description("文件夹")]
        Folder = 2
    }
}
