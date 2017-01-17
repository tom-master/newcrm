using NewCRM.Domain.Interface.BoundedContext.App;
using NewCRM.Domain.Interface.BoundedContextMember;

namespace NewCRM.Domain.Services.BoundedContext.App
{
    public sealed class AppContext : IAppContext
    {
        public IInstallAppServices InstallAppServices { get; set; }

        public IModifyAppInfoServices ModifyAppInfoServices { get; set; }

        public IModifyAppTypeServices ModifyAppTypeServices { get; set; }
    }
}
