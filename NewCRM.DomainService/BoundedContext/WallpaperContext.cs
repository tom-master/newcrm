using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
                    var sql = $@"SELECT COUNT(*) FROM dbo.Wallpapers AS a WHERE a.AccountId=@AccountId AND a.IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@AccountId",wallpaper.AccountId)
                    };
                    var result = dataStore.FindSingleValue<Int32>(sql, parameters);
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
                    VALUES  ( @Title , -- Title - nvarchar(max)
                              @Url , -- Url - nvarchar(max)
                              @ShortUrl , -- ShortUrl - nvarchar(max)
                              @Source , -- Source - int
                              @Description , -- Description - nvarchar(50)
                              @Width , -- Width - int
                              @Height , -- Height - int
                              @AccountId , -- AccountId - int
                              @Md5 , -- Md5 - nvarchar(max)
                              0 , -- IsDeleted - bit
                              GETDATE() , -- AddTime - datetime
                              GETDATE()  -- LastModifyTime - datetime
                            ) SELECT CAST(@@IDENTITY AS INT) AS Ide";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@Title",wallpaper.Title),
                        new SqlParameter("@Url",wallpaper.Url),
                        new SqlParameter("@ShortUrl",wallpaper.ShortUrl),
                        new SqlParameter("@Source",(Int32)wallpaper.Source),
                        new SqlParameter("@Description",wallpaper.Description),
                        new SqlParameter("@Width",wallpaper.Width),
                        new SqlParameter("@Height",wallpaper.Height),
                        new SqlParameter("@AccountId",wallpaper.AccountId),
                        new SqlParameter("@Md5",wallpaper.Md5)
                    };
                    newWallpaperId = dataStore.FindSingleValue<Int32>(sql, parameters);
                }
                #endregion

                #region 获取返回值
                {
                    var sql = $@"SELECT a.Id,a.Url FROM dbo.Wallpapers AS a WHERE a.Id=@parameters AND a.IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@Id",newWallpaperId)
                    };
                    var result = dataStore.FindOne<Wallpaper>(sql, parameters);
                    if (result != null)
                    {
                        return new Tuple<int, string>(result.Id, result.Url);
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
                            FROM dbo.Wallpapers AS a WHERE a.Md5=@Md5 AND a.IsDeleted=0";
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Md5",md5)
                };
                return dataStore.FindOne<Wallpaper>(sql, parameters);
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
                            FROM dbo.Wallpapers AS a WHERE a.AccountId=@AccountId AND a.Source=@Source AND a.IsDeleted=0";
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@AccountId",accountId),
                    new SqlParameter("@Source",(Int32)WallpaperSource.Upload)
                };
                return dataStore.Find<Wallpaper>(sql, parameters);
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
                            FROM dbo.Wallpapers AS a WHERE a.Source=@Source AND a.IsDeleted=0";
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@Source",(Int32)WallpaperSource.System)
                };
                return dataStore.Find<Wallpaper>(sql, parameters);
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
                    var sql = $@"UPDATE dbo.Configs SET WallpaperMode=@WallpaperMode WHERE AccountId=@accountId AND IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@WallpaperMode",(Int32)wallpaperMode),
                        new SqlParameter("@AccountId",accountId)
                    };
                    dataStore.SqlExecute(sql, parameters);
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
                var sql = $@"UPDATE dbo.Configs SET IsBing=0,WallpaperId=@WallpaperId WHERE AccountId=@AccountId AND IsDeleted=0";
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@WallpaperId",newWallpaperId),
                    new SqlParameter("@AccountId",accountId)
                };
                dataStore.SqlExecute(sql, parameters);
            }
        }

        public void RemoveWallpaper(Int32 accountId, Int32 wallpaperId)
        {
            ValidateParameter.Validate(accountId).Validate(wallpaperId);
            using (var dataStore = new DataStore())
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@WallpaperId",wallpaperId),
                    new SqlParameter("@AccountId",accountId)
                };
                #region 前置条件验证
                {
                    var sql = $@"SELECT COUNT(*) FROM dbo.Configs AS a WHERE a.AccountId=@AccountId AND a.WallpaperId=@WallpaperId AND a.IsDeleted=0";

                    var result = dataStore.FindSingleValue<Int32>(sql, parameters);
                    if (result > 0)
                    {
                        throw new BusinessException("当前壁纸正在使用中，不能删除");
                    }
                }
                #endregion

                #region 移除壁纸
                {
                    var sql = $@"UPDATE dbo.Wallpapers SET IsDeleted=1 WHERE Id=@WallpaperId AND AccountId=@AccountId AND IsDeleted=0";
                    dataStore.SqlExecute(sql, parameters);
                }
                #endregion
            }
        }

    }
}
