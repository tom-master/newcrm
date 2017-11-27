using System;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Repository.StorageProvider;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    public class CreateNewFolderServices : BaseServiceContext, ICreateNewFolderServices
    {
        public void NewFolder(Int32 deskId, String folderName, String folderImg)
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
                                {folder.MemberType},         -- MemberType - int
                                1,         -- DeskIndex - int
                                0,      -- IsDeleted - bit
                                GETDATE(), -- AddTime - datetime
                                GETDATE(), -- LastModifyTime - datetime
                                0          -- AccountId - int
                            )";
                dataStore.SqlExecute(sql);
            }
        }
    }
}
