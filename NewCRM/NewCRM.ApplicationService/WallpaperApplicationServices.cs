using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NewCRM.Application.Services.IApplicationService;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Domain.Services;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomHelper;

namespace NewCRM.Application.Services
{
    [Export(typeof(IWallpaperApplicationServices))]
    internal class WallpaperApplicationServices:IWallpaperApplicationServices
    {
        [Import]
        private IWallpaperServices _wallpaperServices;

        private readonly Parameter _validateParameter = new Parameter();
    
        public IList<WallpaperDto> GetWallpaper()
        {
            return _wallpaperServices.GetWallpaper().ConvertToDto<Wallpaper, WallpaperDto>();
        }

        public void ModifyWallpaperMode(Int32 userId, String newMode)
        {
            _validateParameter.Validate(userId).Validate(newMode);

            _wallpaperServices.ModifyWallpaperMode(userId, newMode);
        }

        public void ModifyWallpaper(Int32 userId, Int32 newWallpaperId)
        {
            _validateParameter.Validate(userId).Validate(newWallpaperId);

            _wallpaperServices.ModifyWallpaper(userId, newWallpaperId);
        }

        public Tuple<Int32, String> AddWallpaper(WallpaperDto wallpaperDto)
        {
            _validateParameter.Validate(wallpaperDto);

            var wallpaper = wallpaperDto.ConvertToDomainModel<WallpaperDto, Wallpaper>();

            return _wallpaperServices.AddWallpaper(wallpaper);
        }

        public IList<WallpaperDto> GetUploadWallpaper(Int32 userId)
        {
            _validateParameter.Validate(userId);

            return _wallpaperServices.GetUploadWallpaper(userId).ConvertToDto<Wallpaper, WallpaperDto>();
        }

        public void RemoveWallpaper(Int32 userId, Int32 wallpaperId)
        {
            _validateParameter.Validate(userId).Validate(wallpaperId);
            _wallpaperServices.RemoveWallpaper(userId, wallpaperId);
        }

        public async Task<Tuple<Int32, String>> AddWebWallpaper(Int32 userId, String url)
        {
            var imageTitle = Path.GetFileNameWithoutExtension(url);
            Image image;
            using (var stream = await new HttpClient().GetStreamAsync(new Uri(url)))
            using (image = Image.FromStream(stream))
            {
                var wallpaperMd5 = CalculateFile.Calculate(stream);
                var webWallpaper = GetUploadWallpaper(wallpaperMd5);
                if (webWallpaper != null)
                {
                    return new Tuple<Int32, String>(webWallpaper.Id, webWallpaper.ShortUrl);
                }

                var wallpaperResult = AddWallpaper(new WallpaperDto
                {
                    Width = image.Width,
                    Heigth = image.Height,
                    Source = WallpaperSource.Web,
                    Title = imageTitle,
                    Url = url,
                    UserId = userId,
                    Md5 = wallpaperMd5,
                    ShortUrl = url

                });
                return new Tuple<Int32, String>(wallpaperResult.Item1, wallpaperResult.Item2);
            }
        }

        public WallpaperDto GetUploadWallpaper(String md5)
        {
            _validateParameter.Validate(md5);

            var wallpaperResult = _wallpaperServices.GetUploadWallpaper(md5);

            return wallpaperResult?.ConvertToDto<Wallpaper, WallpaperDto>();
        }

      
    }
}
