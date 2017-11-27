using System.Collections.Generic;
using System.Data.SqlClient;
using NewCRM.Domain.Services.Interface;
using NewCRM.Repository.StorageProvider;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    public class SkinContext : BaseServiceContext, ISkinContext
    {
        /// <summary>
        /// 修改皮肤
        /// </summary>
        public void ModifySkin(int accountId, string newSkin)
        {
            ValidateParameter.Validate(accountId).Validate(newSkin);
            using(var dataStore = new DataStore())
            {
                var sql = $@"
UPDATE dbo.Configs SET Skin=@skin WHERE AccountId={accountId} AND IsDeleted=0";
                dataStore.SqlExecute(sql, new List<SqlParameter> { new SqlParameter("@skin", newSkin) });
            }
        }
    }
}
