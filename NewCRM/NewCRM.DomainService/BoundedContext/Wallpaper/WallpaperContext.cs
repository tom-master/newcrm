using System.ComponentModel.Composition;
using NewCRM.Domain.Interface.BoundedContext.Wallpaper;
using NewCRM.Domain.Interface.BoundedContextMember;

namespace NewCRM.Domain.Services.BoundedContext.Wallpaper
{
    [Export(typeof(IWallpaperContext))]
    public sealed class WallpaperContext : IWallpaperContext
    {
        [Import]
        public IModifyWallpaperServices ModifyWallpaperServices { get; set; }
    }
}
