using System;
using System.ComponentModel.Composition;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    internal sealed class ModifyWallpaperServices : BaseServiceContext, IModifyWallpaperServices
    {


        public void ModifyWallpaperMode(String newMode)
        {
            WallpaperMode wallpaperMode;

            if (Enum.TryParse(newMode, true, out wallpaperMode))
            {
                var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == AccountId));

                accountResult.Config.ModifyDisplayMode(wallpaperMode);

                Repository.Create<Account>().Update(accountResult);
            }
            else
            {
                throw new BusinessException($"无法识别的壁纸显示模式:{newMode}");
            }
        }

        public void ModifyWallpaper(Int32 newWallpaperId)
        {
            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == AccountId));

            var wallpaperResult = DatabaseQuery.FindOne(FilterFactory.Create<Wallpaper>(wallpaper => wallpaper.Id == newWallpaperId));

            accountResult.Config.ModifyWallpaper(wallpaperResult);

            Repository.Create<Account>().Update(accountResult);
        }

        public void RemoveWallpaper(Int32 wallpaperId)
        {
            var accountResult = DatabaseQuery.FindOne(FilterFactory.Create((Account account) => account.Id == AccountId));

            if (accountResult.Config.Wallpaper.Id == wallpaperId)
            {
                throw new BusinessException($"当前壁纸正在使用或已被删除");
            }

            Repository.Create<Wallpaper>().Remove(wallpaper => wallpaper.Id == wallpaperId);
        }
    }
}
