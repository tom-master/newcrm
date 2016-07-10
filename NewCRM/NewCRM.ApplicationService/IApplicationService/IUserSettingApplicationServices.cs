using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Dto.Dto;

namespace NewCRM.Application.Services.IApplicationService
{
    public interface IUserSettingApplicationServices
    {
        void ModifyDefaultDeskNumber(Int32 userId, Int32 newDefaultDeskNumber);

        void ModifyAppDirection(Int32 userId, String direction);

        void ModifyAppIconSize(Int32 userId, Int32 newSize);

        void ModifyAppVerticalSpacing(Int32 userId, Int32 newSize);

        void ModifyAppHorizontalSpacing(Int32 userId, Int32 newSize);

        void ModifyDockPosition(Int32 userId, Int32 defaultDeskNumber, String newPosition);

        IList<WallpaperDto> GetWallpaper();

        void ModifyWallpaperMode(Int32 userId, String newMode);

        void ModifyWallpaper(Int32 userId, Int32 newWallpaperId);

        Tuple<Int32, String> AddWallpaper(WallpaperDto wallpaperDto);

        IList<WallpaperDto> GetUploadWallpaper(Int32 userId);

        void RemoveWallpaper(Int32 userId, Int32 wallpaperId);

        Task<Tuple<Int32, String>> AddWebWallpaper(Int32 userId, String url);
    }
}
