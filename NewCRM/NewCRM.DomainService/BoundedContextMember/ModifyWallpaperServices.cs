using System;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Domain.Interface;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    [Export(typeof(IModifyWallpaperServices))]
    internal class ModifyWallpaperServices : BaseService.BaseService, IModifyWallpaperServices
    {
        public void ModifyWallpaperMode(Int32 accountId, String newMode)
        {
            WallpaperMode wallpaperMode;

            if (Enum.TryParse(newMode, true, out wallpaperMode))
            {
                var accountResult = GetAccountInfoService(accountId);

                accountResult.Config.ModifyDisplayMode(wallpaperMode);

                Repository.Create<Account>().Update(accountResult);
            }
            else
            {
                throw new BusinessException($"无法识别的壁纸显示模式:{newMode}");
            }
        }

        public void ModifyWallpaper(Int32 accountId, Int32 newWallpaperId)
        {
            var accountResult = GetAccountInfoService(accountId);

            var wallpaperResult = QueryFactory.First().Create<Wallpaper>().FindOne(SpecificationFactory.First().Create<Wallpaper>(wallpaper => wallpaper.Id == newWallpaperId));

            accountResult.Config.ModifyWallpaper(wallpaperResult);

            Repository.Create<Account>().Update(accountResult);
        }

        public void RemoveWallpaper(Int32 accountId, Int32 wallpaperId)
        {
            var accountResult = GetAccountInfoService(accountId);

            if (accountResult.Config.Wallpaper.Id == wallpaperId)
            {
                throw new BusinessException($"当前壁纸正在使用或已被删除");
            }

            Repository.Create<Wallpaper>().Remove(wallpaper => wallpaper.Id == wallpaperId);

        }
    }
}
