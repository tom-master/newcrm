using System;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Dto.Dto
{
    public sealed class PowerDto : BaseDto
    {
        [Required, StringLength(20)]
        public String Name { get; set; }

        [Required, StringLength(20)]
        public String PowerIdentity { get; set; }
    }
}
