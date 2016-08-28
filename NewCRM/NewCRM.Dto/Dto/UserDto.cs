using System;
using System.Collections.Generic;

namespace NewCRM.Dto.Dto
{
    public sealed class UserDto : BaseDto
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 是否在线
        /// </summary>
        public Boolean IsOnline { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public String Password { get; set; }

        /// <summary>
        /// 用户类型
        /// </summary>
        public String UserType { get; set; }

        /// <summary>
        /// 桌面
        /// </summary>
        public List<DeskDto> Desks { get; set; }

        /// <summary>
        /// 用户角色Id
        /// </summary>
        public List<RoleDto> Roles { get; set; }
    }
}
