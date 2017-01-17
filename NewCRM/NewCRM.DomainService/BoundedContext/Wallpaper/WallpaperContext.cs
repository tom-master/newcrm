using NewCRM.Domain.Interface.BoundedContext.Wallpaper;
using NewCRM.Domain.Interface.BoundedContextMember;

namespace NewCRM.Domain.Services.BoundedContext.Wallpaper
{
    public sealed class WallpaperContext : IWallpaperContext
    {
        public IModifyWallpaperServices ModifyWallpaperServices { get; set; }
    }
}
