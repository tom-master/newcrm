using System.ComponentModel.Composition;
using NewCRM.Domain.Entities.Repositories.IRepository.Account;
using NewCRM.Domain.Entities.Repositories.IRepository.System;
using NewCRM.Domain.Services;
using NewCRM.Infrastructure.CommonTools.CustomHelper;

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
        protected IAccountRepository AccountRepository { get; set; }

        protected static Parameter ValidateParameter => new Parameter();

    }
}
