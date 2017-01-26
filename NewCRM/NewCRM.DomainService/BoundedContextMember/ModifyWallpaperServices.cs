using System;
using System.ComponentModel.Composition;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember; 
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    [Export(typeof(IModifyWallpaperServices))]
    internal sealed class ModifyWallpaperServices :   IModifyWallpaperServices
    {
        [Import]
        public BaseServiceContext BaseContext { get; set; }

        public void ModifyWallpaperMode(String newMode)
        {
            WallpaperMode wallpaperMode;

            if (Enum.TryParse(newMode, true, out wallpaperMode))
            {
                var accountResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create((Account account) => account.Id == BaseContext.GetAccountId()));

                accountResult.Config.ModifyDisplayMode(wallpaperMode);

                BaseContext.Repository.Create<Account>().Update(accountResult);
            }
            else
            {
                throw new BusinessException($"无法识别的壁纸显示模式:{newMode}");
            }
        }

        public void ModifyWallpaper(Int32 newWallpaperId)
        {
            var accountResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create((Account account) => account.Id == BaseContext.GetAccountId()));

            var wallpaperResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create<Wallpaper>(wallpaper => wallpaper.Id == newWallpaperId));

            accountResult.Config.ModifyWallpaper(wallpaperResult);

            BaseContext.Repository.Create<Account>().Update(accountResult);
        }

        public void RemoveWallpaper(Int32 wallpaperId)
        {
            var accountResult = BaseContext.Query.FindOne(BaseContext.FilterFactory.Create((Account account) => account.Id == BaseContext.GetAccountId()));

            if (accountResult.Config.Wallpaper.Id == wallpaperId)
            {
                throw new BusinessException($"当前壁纸正在使用或已被删除");
            }

            BaseContext.Repository.Create<Wallpaper>().Remove(wallpaper => wallpaper.Id == wallpaperId);
        }
    }
}
