using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.Repository.StorageProvider;

namespace NewCRM.Domain.Services.BoundedContext
{
    public class WallpaperContext : BaseServiceContext, IWallpaperContext
    {
        public Tuple<int, string> AddWallpaper(Wallpaper wallpaper)
        {
            ValidateParameter.Validate(wallpaper);
            using (var dataStore = new DataStore())
            {
                #region 前置条件验证
                {
                    var sql = $@"
SELECT COUNT(*) FROM dbo.Wallpapers AS a WHERE a.AccountId={wallpaper.AccountId} AND a.IsDeleted=0";
                    var result = (Int32)dataStore.SqlScalar(sql);
                    if (result >= 6)
                    {
                        throw new BusinessException("最多只能上传6张图片");
                    }
                }
                #endregion

                var newWallpaperId = 0;
                #region 插入壁纸
                {
                    var sql = $@"INSERT dbo.Wallpapers
                            ( Title ,
                              Url ,
                              ShortUrl ,
                              Source ,
                              Description ,
                              Width ,
                              Height ,
                              AccountId ,
                              Md5 ,
                              IsDeleted ,
                              AddTime ,
                              LastModifyTime
                            )
                    VALUES  ( N'{wallpaper.Title}' , -- Title - nvarchar(max)
                              N'{wallpaper.Url}' , -- Url - nvarchar(max)
                              N'{wallpaper.ShortUrl}' , -- ShortUrl - nvarchar(max)
                              {(Int32)wallpaper.Source} , -- Source - int
                              N'{wallpaper.Description}' , -- Description - nvarchar(50)
                              {wallpaper.Width} , -- Width - int
                              {wallpaper.Height} , -- Height - int
                              {wallpaper.AccountId} , -- AccountId - int
                              N'{wallpaper.Md5}' , -- Md5 - nvarchar(max)
                              0 , -- IsDeleted - bit
                              GETDATE() , -- AddTime - datetime
                              GETDATE()  -- LastModifyTime - datetime
                            ) SELECT CAST(@@IDENTITY AS INT) AS Ide";

                    newWallpaperId = (Int32)dataStore.SqlScalar(sql);
                }
                #endregion

                #region 获取返回值
                {
                    var sql = $@"SELECT a.Id,a.Url FROM dbo.Wallpapers AS a WHERE a.Id={newWallpaperId} AND a.IsDeleted=0";
                    var result = dataStore.SqlGetDataReader(sql);
                    while (result.Read())
                    {
                        return new Tuple<int, string>(Int32.Parse(result["Id"].ToString()), result["Url"].ToString());
                    }
                    return null;
                }
                #endregion
            }
        }

        public Wallpaper GetUploadWallpaper(string md5)
        {
            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT
                            a.AccountId,
                            a.Height,
                            a.Id,
                            a.Md5,
                            a.ShortUrl,
                            a.Source,
                            a.Title,
                            a.Url,
                            a.Width
                            FROM dbo.Wallpapers AS a WHERE a.Md5={md5} AND a.IsDeleted=0";
                return dataStore.SqlGetDataTable(sql).AsSignal<Wallpaper>();
            }
        }

        public List<Wallpaper> GetUploadWallpaper(int accountId)
        {
            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT
                            a.AccountId,
                            a.Height,
                            a.Id,
                            a.Md5,
                            a.ShortUrl,
                            a.Source,
                            a.Title,
                            a.Url,
                            a.Width
                            FROM dbo.Wallpapers AS a WHERE a.AccountId={accountId} AND a.Source={(Int32)WallpaperSource.Upload} AND a.IsDeleted=0";
                return dataStore.SqlGetDataTable(sql).AsList<Wallpaper>().ToList();
            }
        }

        public List<Wallpaper> GetWallpapers()
        {
            using (var dataStore = new DataStore())
            {
                var sql = $@"SELECT
                            a.AccountId,
                            a.Height,
                            a.Id,
                            a.Md5,
                            a.ShortUrl,
                            a.Source,
                            a.Title,
                            a.Url,
                            a.Width
                            FROM dbo.Wallpapers AS a WHERE a.IsDeleted=0";
                return dataStore.SqlGetDataTable(sql).AsList<Wallpaper>().ToList();
            }
        }

        public void ModifyWallpaperMode(Int32 accountId, String newMode)
        {
            ValidateParameter.Validate(accountId).Validate(newMode);

            WallpaperMode wallpaperMode;
            if (Enum.TryParse(newMode, true, out wallpaperMode))
            {
                using (var dataStore = new DataStore())
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
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Configs SET WallpaperId={newWallpaperId} WHERE AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void RemoveWallpaper(Int32 accountId, Int32 wallpaperId)
        {
            ValidateParameter.Validate(accountId).Validate(wallpaperId);
            using (var dataStore = new DataStore())
            {
                #region 前置条件验证
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.Configs AS a WHERE a.AccountId={accountId} AND a.WallpaperId={wallpaperId} AND a.IsDeleted=0";
                    var result = (Int32)dataStore.SqlExecute(sql);
                    if (result > 0)
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
