using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Interface.BoundedContextMember;

namespace NewCRM.Domain.Interface.BoundedContext.Wallpaper
{
    public interface IWallpaperContext
    {
        [Import]
        IModifyWallpaperServices ModifyWallpaperServices { get; set; }

    }
}
