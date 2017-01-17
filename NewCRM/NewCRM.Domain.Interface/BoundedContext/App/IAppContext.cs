using NewCRM.Domain.Interface.BoundedContextMember;

namespace NewCRM.Domain.Interface.BoundedContext.App
{
    public interface IAppContext
    {
        IInstallAppServices InstallAppServices { get; set; }

        IModifyAppInfoServices ModifyAppInfoServices { get; set; }

        IModifyAppTypeServices ModifyAppTypeServices { get; set; }
    }
}
