using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.Entitys.System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.Repository.StorageProvider;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    public class AppTypeServices : IAppTypeServices
    {
        public List<AppType> GetAppTypes()
        {
            using(var dataStore = new DataStore())
            {
                var sql = $@"SELECT a.Id,a.Name FROM dbo.AppTypes AS a WHERE a.IsDeleted=0";
                return dataStore.SqlGetDataTable(sql).AsList<AppType>().ToList();
            }
        }
    }
}
