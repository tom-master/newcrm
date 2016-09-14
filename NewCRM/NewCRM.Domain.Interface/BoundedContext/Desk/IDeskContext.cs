using System.ComponentModel.Composition;
using NewCRM.Domain.Interface.BoundedContextMember;

namespace NewCRM.Domain.Interface.BoundedContext.Desk
{
    public interface IDeskContext
    {
        [Import]
        IModifyDeskMemberPostionServices ModifyDeskMemberPostionServices { get; set; }

        [Import]
        IModifyDeskMemberServices ModifyDeskMemberServices { get; set; }

        [Import]
        IModifyDockPostionServices ModifyDockPostionServices { get; set; }

    }
}