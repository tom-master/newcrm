using System.ComponentModel.Composition;
using NewCRM.Domain.Services;
using NewCRM.Infrastructure.CommonTools.CustomHelper;

namespace NewCRM.Application.Services
{
    internal class BaseApplicationServices
    {
        [Import]
        protected IAccountServices AccountServices { get; set; }

        [Import]
        protected IAppServices AppServices { get; set; }

        [Import]
        protected IDeskServices DeskServices { get; set; }

        [Import]
        protected ISkinServices SkinServices { get; set; }

        [Import]
        protected IWallpaperServices WallpaperServices;

        protected static Parameter ValidateParameter => new Parameter();

    }
}
