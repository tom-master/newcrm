using System.ComponentModel.Composition;
using NewCRM.Domain.Interface.BoundedContextMember;

namespace NewCRM.Domain.Interface.BoundedContext.App
{
    public interface IAppContext
    {
        [Import]
        IInstallAppServices InstallAppServices { get; set; }

        [Import]
        IModifyAppInfoServices ModifyAppInfoServices { get; set; }
    }
}
