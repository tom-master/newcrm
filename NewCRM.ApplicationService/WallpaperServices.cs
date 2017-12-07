using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NewCRM.Application.Services.Interface;
using NewCRM.Domain;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewLib;

namespace NewCRM.Application.Services
{
    public class WallpaperServices : BaseServiceContext, IWallpaperServices
    {
        private readonly IWallpaperContext _wallpaperContext;
        public WallpaperServices(IWallpaperContext wallpaperContext)
        {
            _wallpaperContext = wallpaperContext;
        }

        public List<WallpaperDto> GetWallpapers()
        {
            var result = _wallpaperContext.GetWallpapers();
            return result.Select(s => new WallpaperDto
            {
                AccountId = s.AccountId,
                Height = s.Height,
                Id = s.Id,
                Md5 = s.Md5,
                ShortUrl = s.ShortUrl,
                Source = (Int32)s.Source,
                Title = s.Title,
                Url = s.Url,
                Width = s.Width
            }).ToList();
        }

        public Tuple<Int32, String> AddWallpaper(WallpaperDto wallpaperDto)
        {
            ValidateParameter.Validate(wallpaperDto);
            return _wallpaperContext.AddWallpaper(wallpaperDto.ConvertToModel<WallpaperDto, Wallpaper>());
        }

        public List<WallpaperDto> GetUploadWallpaper(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);
            return _wallpaperContext.GetUploadWallpaper(accountId).Select(s => new WallpaperDto
            {
                AccountId = s.AccountId,
                Height = s.Height,
                Id = s.Id,
                Md5 = s.Md5,
                ShortUrl = s.ShortUrl,
                Source = (Int32)s.Source,
                Title = s.Title,
                Url = s.Url,
                Width = s.Width
            }).ToList();
        }

        public async Task<Tuple<Int32, String>> AddWebWallpaper(Int32 accountId, String url)
        {
            ValidateParameter.Validate(accountId).Validate(url);

            var imageTitle = Path.GetFileNameWithoutExtension(url);
            Image image;

            using (var stream = await new HttpClient().GetStreamAsync(new Uri(url)))
            using (image = Image.FromStream(stream))
            {
                var wallpaperMd5 = FileHelper.GetMD5(stream);
                var webWallpaper = GetUploadWallpaper(wallpaperMd5);
                if (webWallpaper != null)
                {
                    return new Tuple<Int32, String>(webWallpaper.Id, webWallpaper.ShortUrl);
                }

                var wallpaperResult = AddWallpaper(new WallpaperDto
                {
                    Width = image.Width,
                    Height = image.Height,
                    Source = (Int32)WallpaperSource.Web,
                    Title = imageTitle,
                    Url = url,
                    AccountId = accountId,
                    Md5 = wallpaperMd5,
                    ShortUrl = url
                });
                return new Tuple<Int32, String>(wallpaperResult.Item1, wallpaperResult.Item2);
            }
        }

        public WallpaperDto GetUploadWallpaper(String md5)
        {
            ValidateParameter.Validate(md5);
            var result = _wallpaperContext.GetUploadWallpaper(md5);
            return new WallpaperDto
            {
                AccountId = result.AccountId,
                Height = result.Height,
                Width = result.Width,
                Id = result.Id,
                Md5 = result.Md5,
                ShortUrl = result.ShortUrl,
                Source = (Int32)result.Source,
                Title = result.Title,
                Url = result.Url
            };
        }

        public void ModifyWallpaperMode(Int32 accountId, String newMode)
        {
            ValidateParameter.Validate(accountId).Validate(newMode);
            _wallpaperContext.ModifyWallpaperMode(accountId, newMode);
        }

        public void ModifyWallpaper(Int32 accountId, Int32 newWallpaperId)
        {
            ValidateParameter.Validate(accountId).Validate(newWallpaperId);
            _wallpaperContext.ModifyWallpaper(accountId, newWallpaperId);
        }

        public void RemoveWallpaper(Int32 accountId, Int32 wallpaperId)
        {
            ValidateParameter.Validate(accountId).Validate(wallpaperId);
            _wallpaperContext.RemoveWallpaper(accountId, wallpaperId);
        }
    }
}
