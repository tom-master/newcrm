using System.ComponentModel.Composition;
using NewCRM.Domain.Entities.Repositories.IRepository.Security;
using NewCRM.Domain.Services;
using NewCRM.Infrastructure.CommonTools.CustomHelper;
using NewCRM.QueryServices;

namespace NewCRM.Application.Services
{
    internal class BaseApplicationServices
    {

        [Import]
        protected IAccountContext AccountContext { get; set; }

        [Import]
        protected IAppServices AppServices { get; set; }

        [Import]
        protected IWallpaperServices WallpaperServices { get; set; }

        [Import]
        protected ISecurityContext SecurityContext { get; set; }

        [Import]
        protected IAccountQuery AccountQuery { get; set; }

        [Import]
        protected IAppQuery AppQuery { get; set; }

        [Import]
        protected IAppTypeQuery AppTypeQuery { get; set; }

        [Import]
        protected IRoleQuery RoleQuery { get; set; }


        [Import]
        protected IPowerQuery PowerQuery { get; set; }


        protected static Parameter ValidateParameter => new Parameter();

    }
}
