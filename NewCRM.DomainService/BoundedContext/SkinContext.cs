using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using NewCRM.Domain.Services.Interface;
using NewLib.Data.SqlMapper.InternalDataStore;

namespace NewCRM.Domain.Services.BoundedContext
{
    public class SkinContext : BaseServiceContext, ISkinContext
    {
        /// <summary>
        /// 修改皮肤
        /// </summary>
        public async Task ModifySkinAsync(System.Int32 accountId, System.String newSkin)
        {
            Parameter.Validate(accountId).Validate(newSkin);
            await Task.Run(() =>
            {
                using (var dataStore = new DataStore())
                {
                    var sql = $@"UPDATE dbo.Configs SET Skin=@skin WHERE AccountId=@AccountId AND IsDeleted=0";
                    dataStore.SqlExecute(sql, new List<SqlParameter> { new SqlParameter("@skin", newSkin), new SqlParameter("@AccountId", accountId) });
                }
            });
        }
    }
}
