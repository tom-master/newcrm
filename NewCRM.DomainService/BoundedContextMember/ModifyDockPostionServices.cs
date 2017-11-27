using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Repository.StorageProvider;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    public sealed class ModifyDockPostionServices : BaseServiceContext, IModifyDockPostionServices
    {
        public void ModifyDockPosition(Int32 accountId, Int32 defaultDeskNumber, String newPosition)
        {
            ValidateParameter.Validate(accountId).Validate(defaultDeskNumber).Validate(newPosition);

            DockPostion dockPostion;
            if(Enum.TryParse(newPosition, true, out dockPostion))
            {
                using(var dataStore = new DataStore())
                {
                    if(dockPostion == DockPostion.None)
                    {
                        IList<Int32> onDockMemberIds = new List<Int32>();
                        #region 查看是否有成员在应用码头中
                        {
                            var sql = $@"SELECT a.Id FROM dbo.Members AS a WHERE a.IsOnDock=1 AND a.IsDeleted=0";
                            var dataReader = dataStore.SqlGetDataReader(sql);
                            while(dataReader.Read())
                            {
                                onDockMemberIds.Add(Int32.Parse(dataReader["Id"].ToString()));
                            }
                        }
                        #endregion

                        if(onDockMemberIds.Any())
                        {
                            #region 成员移出码头
                            {
                                var sql = $@"UPDATE dbo.Members SET IsOnDock=0 WHERE Id IN ({String.Join(",", onDockMemberIds)}) AND IsDeleted=0";
                                dataStore.SqlExecute(sql);
                            }
                            #endregion
                        }
                    }
                    #region 修改码头位置
                    {
                        var sql = $@"UPDATE dbo.Configs SET DockPosition={(Int32)dockPostion} WHERE AccountId={accountId} AND IsDeleted=0";
                        dataStore.SqlExecute(sql);
                    }
                    #endregion
                }
            }
            else
            {
                throw new BusinessException($"未识别出的码头位置:{newPosition}");
            }
        }
    }
}
