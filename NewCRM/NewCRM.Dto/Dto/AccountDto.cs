using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Dto.Dto
{
    public sealed class AccountDto : BaseDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required(AllowEmptyStrings =true)]
        public String Name { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public Boolean IsOnline { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required(AllowEmptyStrings = true)]
        public String Password { get; set; }

        [Required(AllowEmptyStrings =true)]
        public String LockScreenPassword { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        [Required(AllowEmptyStrings =true), StringLength(3)]
        public String AccountType { get; set; }

        public String AccountFace { get; set; }


        /// <summary>
        /// 用户角色Id
        /// </summary>
        public List<RoleDto> Roles { get; set; }

        public String AddTime { get; set; }

        public String LastLoginTime { get; set; }
    }
}
