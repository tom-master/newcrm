using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NewCRM.Domain;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomException;
using IWallpaperApplicationServices = NewCRM.Application.Services.Interface.IWallpaperApplicationServices;

namespace NewCRM.Application.Services
{
    internal class WallpaperApplicationServices : BaseServiceContext, IWallpaperApplicationServices
    {

        private readonly IModifyWallpaperServices _modifyWallpaperServices;

        
        public WallpaperApplicationServices(IModifyWallpaperServices modifyWallpaperServices)
        {
            _modifyWallpaperServices = modifyWallpaperServices;
        }

        public List<WallpaperDto> GetWallpaper()
        {
            return DatabaseQuery.Find(FilterFactory.Create<Wallpaper>(wallpaper => wallpaper.Source == WallpaperSource.System)).ConvertToDtos<Wallpaper, WallpaperDto>().ToList();
        }

        public Tuple<Int32, String> AddWallpaper(WallpaperDto wallpaperDto)
        {
            ValidateParameter.Validate(wallpaperDto);

            var wallpaper = wallpaperDto.ConvertToModel<WallpaperDto, Wallpaper>();

            var wallPaperCount = DatabaseQuery.Find(FilterFactory.Create<Wallpaper>(w => w.AccountId == wallpaper.AccountId)).Count();

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

            return DatabaseQuery.Find(FilterFactory.Create<Wallpaper>(wallpaper => wallpaper.AccountId == accountId)).ConvertToDtos<Wallpaper, WallpaperDto>().ToList();

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

                UnitOfWork.Commit();

                return new Tuple<Int32, String>(wallpaperResult.Item1, wallpaperResult.Item2);
            }

        }

        public WallpaperDto GetUploadWallpaper(String md5)
        {
            ValidateParameter.Validate(md5);

            return DatabaseQuery.FindOne(FilterFactory.Create<Wallpaper>(wallpaper => wallpaper.Md5 == md5)).ConvertToDto<Wallpaper, WallpaperDto>();

        }

        public void ModifyWallpaperMode(Int32 accountId, String newMode)
        {
            ValidateParameter.Validate(accountId).Validate(newMode);

            _modifyWallpaperServices.ModifyWallpaperMode(accountId,newMode);

            UnitOfWork.Commit();
        }

        public void ModifyWallpaper(Int32 accountId, Int32 newWallpaperId)
        {
            ValidateParameter.Validate(accountId).Validate(newWallpaperId);

            _modifyWallpaperServices.ModifyWallpaper(accountId, newWallpaperId);

            UnitOfWork.Commit();
        }

        public void RemoveWallpaper(Int32 accountId, Int32 wallpaperId)
        {
            ValidateParameter.Validate(accountId).Validate(wallpaperId);

            _modifyWallpaperServices.RemoveWallpaper(accountId, wallpaperId);

            UnitOfWork.Commit();
        }
    }
}
