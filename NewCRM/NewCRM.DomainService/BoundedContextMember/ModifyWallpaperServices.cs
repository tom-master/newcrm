using System;
using System.ComponentModel.Composition;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Interface.BoundedContextMember;
using NewCRM.Domain.Services.Service;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    [Export(typeof(IModifyWallpaperServices))]
    internal class ModifyWallpaperServices : BaseService, IModifyWallpaperServices
    {
        [Import(typeof(Func<Int32, Account>))]
        public Func<Int32, Account> Func { get; set; }


        public void ModifyWallpaperMode(Int32 accountId, String newMode)
        {
            WallpaperMode wallpaperMode;

            if (Enum.TryParse(newMode, true, out wallpaperMode))
            {
                var accountResult = Func(accountId);

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
            var accountResult = Func(accountId);

            var wallpaperResult = Query.FindOne(FilterFactory.Create<Wallpaper>(wallpaper => wallpaper.Id == newWallpaperId));

            accountResult.Config.ModifyWallpaper(wallpaperResult);

            Repository.Create<Account>().Update(accountResult);
        }

        public void RemoveWallpaper(Int32 accountId, Int32 wallpaperId)
        {
            var accountResult = Func(accountId);

            if (accountResult.Config.Wallpaper.Id == wallpaperId)
            {
                throw new BusinessException($"当前壁纸正在使用或已被删除");
            }

            Repository.Create<Wallpaper>().Remove(wallpaper => wallpaper.Id == wallpaperId);

        }
    }
}
