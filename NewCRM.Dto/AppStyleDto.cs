using System;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Dto
{
    public sealed class AppStyleDto : BaseDto
    {
        [Required]
        public String Name { get; set; }

        [Required]
        public String Type { get; set; }
    }
}
