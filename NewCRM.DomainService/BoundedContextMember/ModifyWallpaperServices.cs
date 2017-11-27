using System;
using NewCRM.Domain.Entitys.Agent;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Repository.StorageProvider;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    public sealed class ModifyWallpaperServices : BaseServiceContext, IModifyWallpaperServices
    {
        public ModifyWallpaperServices()
        {
        }

        public void ModifyWallpaperMode(Int32 accountId, String newMode)
        {
            ValidateParameter.Validate(accountId).Validate(newMode);

            WallpaperMode wallpaperMode;
            if(Enum.TryParse(newMode, true, out wallpaperMode))
            {
                using(var dataStore = new DataStore())
                {
                    var sql = $@"UPDATE dbo.Configs SET WallpaperMode={(Int32)wallpaperMode} WHERE AccountId={accountId} AND IsDeleted=0";
                    dataStore.SqlExecute(sql);
                }
            }
            else
            {
                throw new BusinessException($"无法识别的壁纸显示模式:{newMode}");
            }
        }

        public void ModifyWallpaper(Int32 accountId, Int32 newWallpaperId)
        {
            ValidateParameter.Validate(accountId).Validate(newWallpaperId);
            using(var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Configs SET WallpaperId={newWallpaperId} WHERE AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void RemoveWallpaper(Int32 accountId, Int32 wallpaperId)
        {
            ValidateParameter.Validate(accountId).Validate(wallpaperId);
            using(var dataStore = new DataStore())
            {
                #region 前置条件验证
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.Configs AS a WHERE a.AccountId={accountId} AND a.WallpaperId={wallpaperId} AND a.IsDeleted=0";
                    var result = (Int32)dataStore.SqlExecute(sql);
                    if(result > 0)
                    {
                        throw new BusinessException("当前壁纸正在使用中，不能删除");
                    }
                }
                #endregion

                #region 移除壁纸
                {
                    var sql = $@"UPDATE dbo.Wallpapers SET IsDeleted=1 WHERE Id={wallpaperId} AccountId={accountId} AND IsDeleted=0";
                    dataStore.SqlExecute(sql);
                }
                #endregion
            }
        }
    }
}
