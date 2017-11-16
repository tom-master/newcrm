using System;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Dto.Dto
{
    public sealed class AppStateDto : BaseDto
    {
        [Required]
        public String Name { get; set; }

        [Required]
        public String Type { get; set; }
    }
}
