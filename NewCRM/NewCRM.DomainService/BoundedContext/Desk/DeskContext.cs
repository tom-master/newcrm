using System.Collections.Generic;
using System.ComponentModel.Composition;
using NewCRM.Domain.Interface.BoundedContext.Desk;
using NewCRM.Domain.Interface.BoundedContextMember;

namespace NewCRM.Domain.Services.BoundedContext.Desk
{
    [Export(typeof(IDeskContext))]
    public sealed class DeskContext : IDeskContext
    {
        [Import]
        public IModifyDeskMemberPostionServices ModifyDeskMemberPostionServices { get; set; }

        [Import]
        public IModifyDeskMemberServices ModifyDeskMemberServices { get; set; }

        [Import]
        public IModifyDockPostionServices ModifyDockPostionServices { get; set; }

        [Import]
        public ICreateNewFolder CreateNewFolder { get; set; }

      
    }
}
