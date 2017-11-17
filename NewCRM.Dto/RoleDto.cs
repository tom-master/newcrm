using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Dto
{
    public sealed class RoleDto : BaseDto
    {
        [Required, StringLength(20)]
        public String Name { get; set; }

        [Required, StringLength(20)]
        public String RoleIdentity { get; set; }

        [StringLength(20)]
        public String Remark { get; set; }

        public List<PowerDto> Powers { get; set; }
    }
}
