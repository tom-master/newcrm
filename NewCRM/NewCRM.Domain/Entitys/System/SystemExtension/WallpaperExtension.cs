using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewCRM.Domain.Entitys.System
{
    public partial class Wallpaper
    {
        public override String KeyGenerator()
        {
            return $"NewCRM:{nameof(Wallpaper)}:Id:{Id}";
        }
    }
}
