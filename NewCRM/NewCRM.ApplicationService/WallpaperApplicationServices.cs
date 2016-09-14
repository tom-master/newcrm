using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NewCRM.Application.Interface;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Application.Services
{
    [Export(typeof(IWallpaperApplicationServices))]
    internal class WallpaperApplicationServices : BaseServices.BaseServices, IWallpaperApplicationServices
    {
        public List<WallpaperDto> GetWallpaper()
        {
            return QueryFactory.Create<Wallpaper>()
                .Find(SpecificationFactory.Create<Wallpaper>(wallpaper => wallpaper.Source == WallpaperSource.System))
                .ConvertToDtos<Wallpaper, WallpaperDto>().ToList();
        }

        public void ModifyWallpaperMode(Int32 accountId, String newMode)
        {
            ValidateParameter.Validate(accountId).Validate(newMode);

            WallpaperContext.ModifyWallpaperServices.ModifyWallpaperMode(accountId, newMode);
        }

        public void ModifyWallpaper(Int32 accountId, Int32 newWallpaperId)
        {
            ValidateParameter.Validate(accountId).Validate(newWallpaperId);

            WallpaperContext.ModifyWallpaperServices.ModifyWallpaper(accountId, newWallpaperId);
        }

        public Tuple<Int32, String> AddWallpaper(WallpaperDto wallpaperDto)
        {
            ValidateParameter.Validate(wallpaperDto);

            var wallpaper = wallpaperDto.ConvertToModel<WallpaperDto, Wallpaper>();

            var wallPaperCount = QueryFactory.Create<Wallpaper>().Find(SpecificationFactory.Create<Wallpaper>(w => w.AccountId == wallpaper.AccountId)).Count();

            if (wallPaperCount == 6)
            {
                throw new BusinessException($"最多只能上传6张壁纸");
            }

            Repository.Create<Wallpaper>().Add(wallpaper);

            UnitOfWork.Commit();

            return new Tuple<Int32, String>(wallpaper.Id, wallpaper.ShortUrl);

        }

        public List<WallpaperDto> GetUploadWallpaper(Int32 accountId)
        {
            ValidateParameter.Validate(accountId);

            return QueryFactory.Create<Wallpaper>().Find(SpecificationFactory.Create<Wallpaper>(wallpaper => wallpaper.AccountId == accountId)).ConvertToDtos<Wallpaper, WallpaperDto>().ToList();
        }

        public void RemoveWallpaper(Int32 accountId, Int32 wallpaperId)
        {
            ValidateParameter.Validate(accountId).Validate(wallpaperId);

            WallpaperContext.ModifyWallpaperServices.RemoveWallpaper(accountId, wallpaperId);
        }

        public async Task<Tuple<Int32, String>> AddWebWallpaper(Int32 accountId, String url)
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
                    Height = image.Height,
                    Source = WallpaperSource.Web.ToString(),
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

            return QueryFactory.Create<Wallpaper>().FindOne(SpecificationFactory.Create<Wallpaper>(wallpaper => wallpaper.Md5 == md5)).ConvertToDto<Wallpaper, WallpaperDto>();
        }
    }
}
