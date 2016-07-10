using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Domain.Services;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools.CustomHelper;

namespace NewCRM.Application.Services
{
    [Export(typeof(IUserSettingApplicationServices))]
    internal class UserSettingApplicationServices : IUserSettingApplicationServices
    {
        [Import]
        private IUserSettingServices _userSettingServices;

        private readonly Parameter _validateParameter = new Parameter();

        public void ModifyDefaultDeskNumber(Int32 userId, Int32 newDefaultDeskNumber)
        {
            _validateParameter.Validate(userId).Validate(newDefaultDeskNumber);
            _userSettingServices.ModifyDefaultShowDesk(userId, newDefaultDeskNumber);
        }

        public void ModifyAppDirection(Int32 userId, String direction)
        {
            _validateParameter.Validate(userId).Validate(direction);
            _userSettingServices.ModifyAppDirection(userId, direction);
        }

        public void ModifyAppIconSize(Int32 userId, Int32 newSize)
        {
            _validateParameter.Validate(userId).Validate(newSize);
            _userSettingServices.ModifyAppIconSize(userId, newSize);
        }

        public void ModifyAppVerticalSpacing(Int32 userId, Int32 newSize)
        {
            _validateParameter.Validate(userId).Validate(newSize);
            _userSettingServices.ModifyAppVerticalSpacing(userId, newSize);
        }

        public void ModifyAppHorizontalSpacing(Int32 userId, Int32 newSize)
        {
            _validateParameter.Validate(userId).Validate(newSize);
            _userSettingServices.ModifyAppHorizontalSpacing(userId, newSize);
        }

        public void ModifyDockPosition(Int32 userId, Int32 defaultDeskNumber, String newPosition)
        {
            _validateParameter.Validate(userId).Validate(defaultDeskNumber).Validate(newPosition);

            _userSettingServices.ModifyDockPosition(userId, defaultDeskNumber, newPosition);
        }

        public IList<WallpaperDto> GetWallpaper()
        {
            return _userSettingServices.GetWallpaper().ConvertToDto<Wallpaper, WallpaperDto>();
        }

        public void ModifyWallpaperMode(Int32 userId, String newMode)
        {
            _validateParameter.Validate(userId).Validate(newMode);

            _userSettingServices.ModifyWallpaperMode(userId, newMode);
        }

        public void ModifyWallpaper(Int32 userId, Int32 newWallpaperId)
        {
            _validateParameter.Validate(userId).Validate(newWallpaperId);

            _userSettingServices.ModifyWallpaper(userId, newWallpaperId);
        }

        public Tuple<Int32, String> AddWallpaper(WallpaperDto wallpaperDto)
        {
            _validateParameter.Validate(wallpaperDto);

            var wallpaper = wallpaperDto.ConvertToDomainModel<WallpaperDto, Wallpaper>();

            return _userSettingServices.AddWallpaper(wallpaper);
        }

        public IList<WallpaperDto> GetUploadWallpaper(Int32 userId)
        {
            _validateParameter.Validate(userId);

            return _userSettingServices.GetUploadWallpaper(userId).ConvertToDto<Wallpaper, WallpaperDto>();
        }

        public void RemoveWallpaper(Int32 userId, Int32 wallpaperId)
        {
            _validateParameter.Validate(userId).Validate(wallpaperId);
            _userSettingServices.RemoveWallpaper(userId, wallpaperId);
        }

        public async Task<Tuple<Int32, String>> AddWebWallpaper(Int32 userId, String url)
        {
            var imageTitle = Path.GetFileNameWithoutExtension(url);
            Image image;
            using (HttpClient httpClient = new HttpClient())
            {
                using (var imageStream = await httpClient.GetStreamAsync(new Uri(url)))
                {
                    using (image = Image.FromStream(imageStream))
                    {
                        var wallpaperResult = AddWallpaper(new WallpaperDto
                        {
                            Width = image.Width,
                            Heigth = image.Height,
                            Source = WallpaperSource.Web,
                            Title = imageTitle,
                            Url = url,
                            UserId = userId
                        });
                        return new Tuple<Int32, String>(wallpaperResult.Item1, wallpaperResult.Item2);
                    }
                }
            }
        }
    }
}
