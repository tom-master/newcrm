using System.ComponentModel.Composition;
using NewCRM.Domain.Interface.BoundedContext.App;
using NewCRM.Domain.Interface.BoundedContextMember;

namespace NewCRM.Domain.Services.BoundedContext.App
{
    [Export(typeof(IAppContext))]
    public sealed class AppContext : IAppContext
    {
        [Import]
        public IInstallAppServices InstallAppServices { get; set; }

        [Import]
        public IModifyAppInfoServices ModifyAppInfoServices { get; set; }
    }
}
