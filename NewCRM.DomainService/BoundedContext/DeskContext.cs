using System;
using System.Collections.Generic;
using System.Text;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.Repository.StorageProvider;
using NewLib;

namespace NewCRM.Domain.Services.BoundedContext
{
    public class DeskContext : BaseServiceContext, IDeskContext
    {
        public void ModifyDefaultDeskNumber(Int32 accountId, Int32 newDefaultDeskNumber)
        {
            ValidateParameter.Validate(accountId).Validate(newDefaultDeskNumber);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Configs SET DefaultDeskNumber={newDefaultDeskNumber} WHERE AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void ModifyDockPosition(Int32 accountId, Int32 defaultDeskNumber, String newPosition)
        {
            ValidateParameter.Validate(accountId).Validate(defaultDeskNumber).Validate(newPosition);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Configs SET DockPosition={(Int32)EnumExtensions.ToEnum<DockPostion>(newPosition)} WHERE AccountId={accountId} AND DefaultDeskNumber={defaultDeskNumber} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void ModifyMemberDirectionToX(int accountId)
        {
            ValidateParameter.Validate(accountId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Configs SET AppXy={(Int32)AppAlignMode.X} WHERE AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void ModifyMemberDirectionToY(int accountId)
        {
            ValidateParameter.Validate(accountId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Configs SET AppXy={(Int32)AppAlignMode.Y} WHERE AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void ModifyMemberDisplayIconSize(int accountId, int newSize)
        {
            ValidateParameter.Validate(accountId).Validate(newSize);
            using (var dataStore = new DataStore())
            {
                var sql = $@"
UPDATE dbo.Configs SET AppSize={newSize} WHERE AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void ModifyMemberHorizontalSpacing(int accountId, int newSize)
        {
            ValidateParameter.Validate(accountId).Validate(newSize);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Configs SET AppVerticalSpacing={newSize} WHERE AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void ModifyMemberVerticalSpacing(int accountId, int newSize)
        {
            ValidateParameter.Validate(accountId).Validate(newSize);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Configs SET AppHorizontalSpacing={newSize} WHERE AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void MemberInDock(Int32 accountId, Int32 memberId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Members SET IsOnDock=1 WHERE Id={memberId} AND AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void MemberOutDock(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(deskId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Members SET IsOnDock=0 WHERE Id={memberId} AND AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void DockToFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(folderId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Members SET IsOnDock=0,FolderId={folderId} WHERE Id={memberId} AND AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void FolderToDock(Int32 accountId, Int32 memberId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Members SET IsOnDock=1,FolderId=0 WHERE Id={memberId} AND AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void DeskToFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(folderId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Members SET FolderId={folderId} WHERE Id={memberId} AND AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void FolderToDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(deskId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Members SET FolderId=0,DeskIndex={deskId} WHERE Id={memberId} AND AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void FolderToOtherFolder(Int32 accountId, Int32 memberId, Int32 folderId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(folderId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Members SET FolderId={folderId} WHERE Id={memberId} AND AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void DeskToOtherDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(deskId);
            using (var dataStore = new DataStore())
            {
                dataStore.OpenTransaction();
                try
                {
                    var set = new StringBuilder();
                    #region 查询成员是否在应用码头中
                    {
                        var sql = $@"SELECT COUNT(*) FROM dbo.Members AS a WHERE a.Id=0 AND a.AccountId=0 AND a.IsDeleted=0 AND IsOnDock=1";
                        if (dataStore.FindSingleValue<Int32>(sql) > 0)
                        {
                            set.Append($@" ,IsOnDock=0");
                        }
                    }
                    #endregion

                    #region 成员移动到其他桌面
                    {
                        var sql = $@"UPDATE dbo.Members SET DeskIndex={deskId} WHERE Id={memberId} AND AccountId={accountId} AND IsDeleted=0";
                        dataStore.SqlExecute(sql);
                    }
                    #endregion

                    dataStore.Commit();
                }
                catch (Exception)
                {
                    dataStore.Rollback();
                    throw;
                }
            }
        }

        public void DockToOtherDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(deskId);
            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Members SET IsOnDock=0,DeskIndex={deskId} WHERE Id={memberId} AND AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void CreateNewFolder(Int32 deskId, String folderName, String folderImg, Int32 accountId)
        {
            ValidateParameter.Validate(deskId).Validate(folderImg).Validate(folderName);

            var folder = new Member(folderName, folderImg, 0);
            using (var dataStore = new DataStore())
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
                                {folder.Width},         -- Width - int
                                {folder.Height},         -- Height - int
                                0,         -- FolderId - int
                                N'{folder.Name}',       -- Name - nvarchar(6)
                                N'{folder.IconUrl}',       -- IconUrl - nvarchar(max)
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
                                {(Int32)folder.MemberType},         -- MemberType - int
                                {deskId},         -- DeskIndex - int
                                0,      -- IsDeleted - bit
                                GETDATE(), -- AddTime - datetime
                                GETDATE(), -- LastModifyTime - datetime
                                {accountId}          -- AccountId - int
                            )";
                dataStore.SqlExecute(sql);
            }
        }

        public void ModifyWallpaperSource(String source, Int32 accountId)
        {
            ValidateParameter.Validate(source).Validate(accountId);

            using (var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Configs SET IsBing={(source.ToLower() == WallpaperSource.Bing.ToString().ToLower() ? 1 : 0)} WHERE AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }
    }
}
