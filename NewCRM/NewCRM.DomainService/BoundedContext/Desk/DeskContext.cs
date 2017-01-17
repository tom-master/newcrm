using NewCRM.Domain.Interface.BoundedContext.Desk;
using NewCRM.Domain.Interface.BoundedContextMember;

namespace NewCRM.Domain.Services.BoundedContext.Desk
{
    public sealed class DeskContext : IDeskContext
    {
        public IModifyDeskMemberPostionServices ModifyDeskMemberPostionServices { get; set; }

        public IModifyDeskMemberServices ModifyDeskMemberServices { get; set; }

        public IModifyDockPostionServices ModifyDockPostionServices { get; set; }

        public ICreateNewFolder CreateNewFolder { get; set; }
    }
}
