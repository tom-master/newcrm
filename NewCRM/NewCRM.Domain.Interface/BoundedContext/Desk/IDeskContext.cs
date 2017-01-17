using NewCRM.Domain.Interface.BoundedContextMember;

namespace NewCRM.Domain.Interface.BoundedContext.Desk
{
    public interface IDeskContext
    {
        IModifyDeskMemberPostionServices ModifyDeskMemberPostionServices { get; set; }

        IModifyDeskMemberServices ModifyDeskMemberServices { get; set; }

        IModifyDockPostionServices ModifyDockPostionServices { get; set; }

        ICreateNewFolder CreateNewFolder { get; set; }
    }
}