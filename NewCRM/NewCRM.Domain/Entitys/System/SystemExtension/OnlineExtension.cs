using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Domain.Entitys.System
{
    public partial class Online
    {
        public override String KeyGenerator()
        {
            return $"NewCRM:{GetType().Name}:{Id}";
        }
    }
}
