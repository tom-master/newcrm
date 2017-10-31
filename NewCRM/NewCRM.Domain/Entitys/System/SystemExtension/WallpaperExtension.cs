using System;
using System.Collections.Generic;

namespace NewCRM.Domain.Entitys.System
{
    public partial class Wallpaper
    {
        public Wallpaper AddConfig(params Config[] configs)
        {
            if (Configs==null)
            {
                Configs = new List<Config>();
            }
            foreach (var config in configs)
            {
                Configs.Add(config);
            }

            return this;
        }

        public override String KeyGenerator()
        {
            return $"NewCRM:{nameof(Wallpaper)}:Id:{Id}";
        }
    }
}
