using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewLib;
using NewLib.Data.SqlMapper.InternalDataStore;

namespace NewCRM.Domain.Services.BoundedContext
{
    public class DeskContext : BaseServiceContext, IDeskContext
    {
        public async Task ModifyDefaultDeskNumberAsync(Int32 accountId, Int32 newDefaultDeskNumber)
        {
            Parameter.Validate(accountId).Validate(newDefaultDeskNumber);
            return await Task.Run(() =>
            {
                using(var dataStore = new DataStore())
                {
                    var sql = $@"UPDATE dbo.Configs SET DefaultDeskNumber=@DefaultDeskNumber WHERE AccountId=@AccountId AND IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@DefaultDeskNumber",newDefaultDeskNumber),
                        new SqlParameter("@AccountId",accountId)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
            });
        }

        public async Task ModifyDockPositionAsync(Int32 accountId, Int32 defaultDeskNumber, String newPosition)
        {
            Parameter.Validate(accountId).Validate(defaultDeskNumber).Validate(newPosition);
            return await Task.Run(() =>
            {
                using(var dataStore = new DataStore())
                {
                    var sql = $@"UPDATE dbo.Configs SET DockPosition=@DockPosition WHERE AccountId=@AccountId AND DefaultDeskNumber=@defaultDeskNumber AND IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@DockPosition",(Int32)EnumExtensions.ToEnum<DockPostion>(newPosition)),
                        new SqlParameter("@AccountId",accountId),
                        new SqlParameter("@DefaultDeskNumber",defaultDeskNumber)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
            });
        }

        public async Task ModifyMemberDirectionToXAsync(int accountId)
        {
            Parameter.Validate(accountId);
            return await Task.Run(() =>
            {
                using(var dataStore = new DataStore())
                {
                    var sql = $@"UPDATE dbo.Configs SET AppXy=@AppXy WHERE AccountId=@AccountId AND IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@AppXy",(Int32)AppAlignMode.X),
                        new SqlParameter("@AccountId",accountId)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
            });
        }

        public async Task ModifyMemberDirectionToYAsync(int accountId)
        {
            Parameter.Validate(accountId);
            return await Task.Run(() =>
            {
                using(var dataStore = new DataStore())
                {
                    var sql = $@"UPDATE dbo.Configs SET AppXy=@AppXy WHERE AccountId=@AccountId AND IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@AppXy",(Int32)AppAlignMode.Y),
                        new SqlParameter("@AccountId",accountId)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
            });
        }

        public async Task ModifyMemberDisplayIconSizeAsync(int accountId, int newSize)
        {
            Parameter.Validate(accountId).Validate(newSize);
            return await Task.Run(() =>
            {
                using(var dataStore = new DataStore())
                {
                    var sql = $@"UPDATE dbo.Configs SET AppSize=@AppSize WHERE AccountId=@AccountId AND IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@AppSize",newSize),
                        new SqlParameter("@AccountId",accountId)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
            });
        }

        public async Task ModifyMemberHorizontalSpacingAsync(int accountId, int newSize)
        {
            Parameter.Validate(accountId).Validate(newSize);
            return await Task.Run(() =>
            {
                using(var dataStore = new DataStore())
                {
                    var sql = $@"UPDATE dbo.Configs SET AppVerticalSpacing=@AppVerticalSpacing WHERE AccountId=@AccountId AND IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@AppVerticalSpacing",newSize),
                        new SqlParameter("@AccountId",accountId)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
            });

        }

        public async Task ModifyMemberVerticalSpacingAsync(int accountId, int newSize)
        {
            Parameter.Validate(accountId).Validate(newSize);
            return await Task.Run(() =>
            {
                using(var dataStore = new DataStore())
                {
                    var sql = $@"UPDATE dbo.Configs SET AppHorizontalSpacing=@AppHorizontalSpacing WHERE AccountId=@AccountId AND IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@AppHorizontalSpacing",newSize),
                        new SqlParameter("@AccountId",accountId)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
            });
        }

        public async Task MemberInDockAsync(Int32 accountId, Int32 memberId)
        {
            Parameter.Validate(accountId).Validate(memberId);
            return await Task.Run(() =>
            {
                using(var dataStore = new DataStore())
                {
                    var sql = $@"UPDATE dbo.Members SET IsOnDock=1 WHERE Id=@Id AND AccountId=@AccountId AND IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@Id",memberId),
                        new SqlParameter("@AccountId",accountId)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
            });
        }

        public async Task MemberOutDockAsync(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            Parameter.Validate(accountId).Validate(memberId).Validate(deskId);
            return await Task.Run(() =>
            {
                using(var dataStore = new DataStore())
                {
                    var sql = $@"UPDATE dbo.Members SET IsOnDock=0 WHERE Id=@Id AND AccountId=@AccountId AND IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@Id",memberId),
                        new SqlParameter("@AccountId",accountId)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
            });
        }

        public async Task DockToFolderAsync(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            Parameter.Validate(accountId).Validate(memberId).Validate(folderId);
            return await Task.Run(() =>
            {
                using(var dataStore = new DataStore())
                {
                    var sql = $@"UPDATE dbo.Members SET IsOnDock=0,FolderId=@FolderId WHERE Id=@Id AND AccountId=@AccountId AND IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@FolderId",folderId),
                        new SqlParameter("@Id",memberId),
                        new SqlParameter("@AccountId",accountId)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
            });
        }

        public async Task FolderToDockAsync(Int32 accountId, Int32 memberId)
        {
            Parameter.Validate(accountId).Validate(memberId);
            return await Task.Run(() =>
            {
                using(var dataStore = new DataStore())
                {
                    var sql = $@"UPDATE dbo.Members SET IsOnDock=1,FolderId=0 WHERE Id=@Id AND AccountId=@AccountId AND IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@Id",memberId),
                        new SqlParameter("@AccountId",accountId)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
            });
        }

        public async Task DeskToFolderAsync(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            Parameter.Validate(accountId).Validate(memberId).Validate(folderId);
            return await Task.Run(() =>
            {
                using(var dataStore = new DataStore())
                {
                    var sql = $@"UPDATE dbo.Members SET FolderId=@FolderId WHERE Id=@Id AND AccountId=@AccountId AND IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@FolderId",folderId),
                        new SqlParameter("@Id",memberId),
                        new SqlParameter("@AccountId",accountId)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
            });
        }

        public async Task FolderToDeskAsync(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            Parameter.Validate(accountId).Validate(memberId).Validate(deskId);
            return await Task.Run(() =>
            {
                using(var dataStore = new DataStore())
                {
                    var sql = $@"UPDATE dbo.Members SET FolderId=0,DeskIndex=@DeskIndex WHERE Id=@Id AND AccountId=@AccountId AND IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@DeskIndex",deskId),
                        new SqlParameter("@Id",memberId),
                        new SqlParameter("@AccountId",accountId)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
            });
        }

        public async Task FolderToOtherFolderAsync(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            Parameter.Validate(accountId).Validate(memberId).Validate(folderId);
            return await Task.Run(() =>
            {
                using(var dataStore = new DataStore())
                {
                    var sql = $@"UPDATE dbo.Members SET FolderId=@FolderId WHERE Id=@Id AND AccountId=@AccountId AND IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@FolderId",folderId),
                        new SqlParameter("@Id",memberId),
                        new SqlParameter("@AccountId",accountId)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
            });
        }

        public async Task DeskToOtherDeskAsync(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            Parameter.Validate(accountId).Validate(memberId).Validate(deskId);
            return await Task.Run(() =>
            {
                using(var dataStore = new DataStore())
                {
                    dataStore.OpenTransaction();
                    try
                    {
                        var set = new StringBuilder();
                        #region 查询成员是否在应用码头中
                        {
                            var sql = $@"SELECT COUNT(*) FROM dbo.Members AS a WHERE a.Id=0 AND a.AccountId=0 AND a.IsDeleted=0 AND IsOnDock=1";
                            if(dataStore.FindSingleValue<Int32>(sql) > 0)
                            {
                                set.Append($@" ,IsOnDock=0");
                            }
                        }
                        #endregion

                        #region 成员移动到其他桌面
                        {
                            var sql = $@"UPDATE dbo.Members SET DeskIndex=@DeskIndex {set} WHERE Id=@Id AND AccountId=@AccountId AND IsDeleted=0";
                            var parameters = new List<SqlParameter>
                        {
                            new SqlParameter("@DeskIndex",deskId),
                            new SqlParameter("@Id",memberId),
                            new SqlParameter("@AccountId",accountId)
                        };
                            dataStore.SqlExecute(sql, parameters);
                        }
                        #endregion

                        dataStore.Commit();
                    }
                    catch(Exception)
                    {
                        dataStore.Rollback();
                        throw;
                    }
                }
            });
        }

        public async Task DockToOtherDeskAsync(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            Parameter.Validate(accountId).Validate(memberId).Validate(deskId);
            return await Task.Run(() =>
            {
                using(var dataStore = new DataStore())
                {
                    var sql = $@"UPDATE dbo.Members SET IsOnDock=0,DeskIndex=@DeskIndex WHERE Id=@Id AND AccountId=@AccountId AND IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@DeskIndex",deskId),
                        new SqlParameter("@Id",memberId),
                        new SqlParameter("@AccountId",accountId)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
            });
        }

        public async Task CreateNewFolderAsync(Int32 deskId, String folderName, String folderImg, Int32 accountId)
        {
            Parameter.Validate(deskId).Validate(folderImg).Validate(folderName);
            return await Task.Run(() =>
            {
                var folder = new Member(folderName, folderImg, 0);
                using(var dataStore = new DataStore())
                {
                    var sql = $@"INSERT dbo.Members
                            (
                                AppId,
                                Width,
                                Height,
                                FolderId,
                                Name,
                                IconUrl,
                                AppUrl,
                                IsOnDock,
                                IsMax,
                                IsFull,
                                IsSetbar,
                                IsOpenMax,
                                IsLock,
                                IsFlash,
                                IsDraw,
                                IsResize,
                                MemberType,
                                DeskIndex,
                                IsDeleted,
                                AddTime,
                                LastModifyTime,
                                AccountId
                            )
                            VALUES
                            (   0,         -- AppId - int
                                @Width,         -- Width - int
                                @Height,         -- Height - int
                                0,         -- FolderId - int
                                @Name,       -- Name - nvarchar(6)
                                @IconUrl,       -- IconUrl - nvarchar(max)
                                N'',       -- AppUrl - nvarchar(max)
                                0,      -- IsOnDock - bit
                                0,      -- IsMax - bit
                                0,      -- IsFull - bit
                                0,      -- IsSetbar - bit
                                0,      -- IsOpenMax - bit
                                0,      -- IsLock - bit
                                0,      -- IsFlash - bit
                                0,      -- IsDraw - bit
                                0,      -- IsResize - bit
                                @MemberType,         -- MemberType - int
                                @deskId,         -- DeskIndex - int
                                0,      -- IsDeleted - bit
                                GETDATE(), -- AddTime - datetime
                                GETDATE(), -- LastModifyTime - datetime
                                @accountId          -- AccountId - int
                            )";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@Width",folder.Width),
                        new SqlParameter("@Height",folder.Height),
                        new SqlParameter("@Name",folder.Name),
                        new SqlParameter("@MemberType",(Int32)folder.MemberType),
                        new SqlParameter("@deskId",deskId),
                        new SqlParameter("@accountId",accountId),
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
            });
        }

        public async Task ModifyWallpaperSourceAsync(String source, Int32 accountId)
        {
            Parameter.Validate(source).Validate(accountId);
            return await Task.Run(() =>
            {
                using(var dataStore = new DataStore())
                {
                    var sql = $@"UPDATE dbo.Configs SET IsBing=@IsBing WHERE AccountId=@AccountId AND IsDeleted=0";
                    var parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@IsBing",(source.ToLower() == WallpaperSource.Bing.ToString().ToLower() ? 1 : 0)),
                        new SqlParameter("@AccountId",accountId)
                    };
                    dataStore.SqlExecute(sql, parameters);
                }
            });
        }
    }
}
