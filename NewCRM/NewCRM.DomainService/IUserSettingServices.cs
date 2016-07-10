using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entities.DomainModel.System;

namespace NewCRM.Domain.Services
{
    public interface IUserSettingServices
    {
        void ModifyDefaultShowDesk(Int32 userId, Int32 newDefaultDeskNumber);

        void ModifyAppDirection(Int32 userId, String direction);

        void ModifyAppIconSize(Int32 userId, Int32 newSize);

        void ModifyAppVerticalSpacing(Int32 userId, Int32 newSize);

        void ModifyAppHorizontalSpacing(Int32 userId, Int32 newSize);

        void ModifyDockPosition(Int32 userId, Int32 defaultDeskNumber, String newPosition);

        IList<Wallpaper> GetWallpaper();

        void ModifyWallpaperMode(Int32 userId, String newMode);

        void ModifyWallpaper(Int32 userId, Int32 newWallpaperId);

        Tuple<Int32, String> AddWallpaper(Wallpaper wallpaper);

        IList<Wallpaper> GetUploadWallpaper(Int32 userId);

        void RemoveWallpaper(Int32 userId, Int32 wallpaperId);

    }
}
