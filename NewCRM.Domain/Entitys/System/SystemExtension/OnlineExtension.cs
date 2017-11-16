using System;

namespace NewCRM.Domain.Entitys.System
{
    public partial class Online
    {
        public override String KeyGenerator()
        {
            return $"NewCRM:{nameof(Online)}:Id:{Id}";
        }
    }
}
