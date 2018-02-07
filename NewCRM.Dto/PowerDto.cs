using System;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Dto
{
    public sealed class PowerDto : BaseDto
    {
        public String Name { get; set; }

        public String PowerIdentity { get; set; }
    }
}
