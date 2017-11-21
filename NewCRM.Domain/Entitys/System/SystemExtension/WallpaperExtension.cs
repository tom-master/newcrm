using System;
using System.Collections.Generic;

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
