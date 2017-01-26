using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using NewCRM.Application.Interface;
using NewCRM.Domain;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Domain.ValueObject;
using NewCRM.Dto;
using NewCRM.Dto.Dto;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Application.Services
{
    [Export(typeof(IWallpaperApplicationServices))]
    internal class WallpaperApplicationServices :  IWallpaperApplicationServices
    {

        [Import]
        public BaseServiceContext BaseContext { get; set; }

        private readonly IModifyWallpaperServices _modifyWallpaperServices;

        [ImportingConstructor]
        public WallpaperApplicationServices(IModifyWallpaperServices modifyWallpaperServices)
        {
            _modifyWallpaperServices = modifyWallpaperServices;
        }

        public List<WallpaperDto> GetWallpaper()
        {
            return  BaseContext.Query.Find(BaseContext.FilterFactory.Create<Wallpaper>(wallpaper => wallpaper.Source == WallpaperSource.System)).ConvertToDtos<Wallpaper, WallpaperDto>().ToList();
        }

        public Tuple<Int32, String> AddWallpaper(WallpaperDto wallpaperDto)
        {
            BaseContext.ValidateParameter.Validate(wallpaperDto);

            var wallpaper = wallpaperDto.ConvertToModel<WallpaperDto, Wallpaper>();

            var wallPaperCount = BaseContext.Query.Find(BaseContext.FilterFactory.Create<Wallpaper>(w => w.AccountId == wallpaper.AccountId)).Count();

            if (wallPaperCount == 6)
            {
                throw new BusinessException($"最多只能上传6张壁纸");
            }

            BaseContext.Repository.Create<Wallpaper>().Add(wallpaper);

            BaseContext.UnitOfWork.Commit();

            return new Tuple<Int32, String>(wallpaper.Id, wallpaper.ShortUrl);


        }

        public List<WallpaperDto> GetUploadWallpaper(Int32 accountId)
        {
            BaseContext.ValidateParameter.Validate(accountId);

            return BaseContext.Query.Find(BaseContext.FilterFactory.Create<Wallpaper>(wallpaper => wallpaper.AccountId == accountId)).ConvertToDtos<Wallpaper, WallpaperDto>().ToList();

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

                BaseContext.UnitOfWork.Commit();

                return new Tuple<Int32, String>(wallpaperResult.Item1, wallpaperResult.Item2);
            }

        }

        public WallpaperDto GetUploadWallpaper(String md5)
        {
            BaseContext.ValidateParameter.Validate(md5);

            return BaseContext.Query.FindOne(BaseContext.FilterFactory.Create<Wallpaper>(wallpaper => wallpaper.Md5 == md5)).ConvertToDto<Wallpaper, WallpaperDto>();

        }

        public void ModifyWallpaperMode(Int32 accountId, String newMode)
        {
            BaseContext.ValidateParameter.Validate(accountId).Validate(newMode);

            _modifyWallpaperServices.ModifyWallpaperMode(newMode);

            BaseContext.UnitOfWork.Commit();
        }

        public void ModifyWallpaper(Int32 accountId, Int32 newWallpaperId)
        {
            BaseContext.ValidateParameter.Validate(accountId).Validate(newWallpaperId);

            _modifyWallpaperServices.ModifyWallpaper(newWallpaperId);

            BaseContext.UnitOfWork.Commit();
        }

        public void RemoveWallpaper(Int32 accountId, Int32 wallpaperId)
        {
            BaseContext.ValidateParameter.Validate(accountId).Validate(wallpaperId);

            _modifyWallpaperServices.RemoveWallpaper(wallpaperId);

            BaseContext.UnitOfWork.Commit();
        }
    }
}
