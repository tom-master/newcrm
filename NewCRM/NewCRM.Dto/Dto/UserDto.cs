using System;
using System.Collections.Generic;
using NewCRM.Domain.DomainModel.Account;
using NewCRM.Domain.DomainModel.Security;
using NewCRM.Domain.DomainModel.System;

namespace NewCRM.Dto.Dto
{
    public sealed class UserDto
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Password { get; set; }

        public Boolean IsDisable { get; set; }
        public DateTime LastLoginTime { get; set; }

        public Department Department { get; set; }
        public UserConfigure UserConfigure { get; set; }

        public ICollection<App> Apps { get; set; }
        public ICollection<Role> Roles { get; set; }
        public ICollection<Log> Logs { get; set; }
        public ICollection<Title> Titles { get; set; }
    }
}
