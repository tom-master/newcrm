using System;
using NewCRM.Domain.Entitys.Security;

namespace NewCRM.Domain.Entitys.System
{
    public class AppRole : DomainModelBase
    {
        public Int32 AppId { get; private set; }

        public Int32 RoleId { get; private set; }

        public virtual Role Role { get; private set; }

        public AppRole(Int32 appId, Int32 roleId)
        {
            AppId = appId;

            RoleId = roleId;
        }
    }
}
