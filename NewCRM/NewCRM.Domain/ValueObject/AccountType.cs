using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Domain.Entities.ValueObject
{
    public enum AccountType
    {
        [Description("普通用户")]
        User = 1,

        [Description("管理员")]
        Admin = 2
    }
}
