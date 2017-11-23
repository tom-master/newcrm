using System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Repository.StorageProvider;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    public class DeskContext : BaseServiceContext, IDeskContext
    {
        public void ModifyMemberDirectionToX(int accountId)
        {
            ValidateParameter.Validate(accountId);
            using(var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Configs SET AppXy={(Int32)AppAlignMode.X} WHERE AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void ModifyMemberDirectionToY(int accountId)
        {
            ValidateParameter.Validate(accountId);
            using(var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Configs SET AppXy={(Int32)AppAlignMode.Y} WHERE AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void ModifyMemberDisplayIconSize(int accountId, int newSize)
        {
            ValidateParameter.Validate(accountId).Validate(newSize);
            using(var dataStore = new DataStore())
            {
                var sql = $@"
UPDATE dbo.Configs SET AppSize={newSize} WHERE AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void ModifyMemberHorizontalSpacing(int accountId, int newSize)
        {
            ValidateParameter.Validate(accountId).Validate(newSize);
            using(var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Configs SET AppVerticalSpacing={newSize} WHERE AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }

        public void ModifyMemberVerticalSpacing(int accountId, int newSize)
        {
            ValidateParameter.Validate(accountId).Validate(newSize);
            using(var dataStore = new DataStore())
            {
                var sql = $@"UPDATE dbo.Configs SET AppHorizontalSpacing={newSize} WHERE AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql);
            }
        }
    }
}
