using NewCRM.Domain.Repositories.IRepository.System;
using NewCRM.Domain.Services.Interface;
using System;
using System.Linq;
using NewCRM.Repository.StorageProvider;
using System.Text;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    public sealed class ModifyDeskMemberPostionServices : BaseServiceContext, IModifyDeskMemberPostionServices
    {
        private readonly IDeskRepository _deskRepository;

        public ModifyDeskMemberPostionServices(IDeskRepository deskRepository)
        {
            _deskRepository = deskRepository;
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
                        if ((Int32)dataStore.SqlScalar(sql) > 0)
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
                catch (Exception ex)
                {
                    dataStore.Rollback();
                    throw;
                }
            }
        }

        public void DockToOtherDesk(Int32 accountId, Int32 memberId, Int32 deskId)
        {
            ValidateParameter.Validate(accountId).Validate(memberId).Validate(deskId);

            var desks = GetDesks(accountId);
            var realDeskId = GetRealDeskId(deskId, desks);
            foreach (var desk in desks)
            {
                var member = GetMember(memberId, desk);
                if (member != null)
                {
                    member.OutDock().ToOtherDesk(realDeskId);
                    _deskRepository.Update(desk);

                    break;
                }
            }
        }
    }
}
