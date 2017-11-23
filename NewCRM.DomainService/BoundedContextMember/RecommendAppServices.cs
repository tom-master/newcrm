using System;
using NewCRM.Domain.Services.Interface;
using NewCRM.Domain.ValueObject;
using NewCRM.Repository.StorageProvider;

namespace NewCRM.Domain.Services.BoundedContextMember
{
    public class RecommendAppServices : BaseServiceContext, IRecommendAppServices
    {
        /// <summary>
        /// 获取今日推荐App
        /// </summary>
        /// <returns></returns>
        public dynamic GetTodayRecommend(int accountId)
        {
            ValidateParameter.Validate(accountId);

            using(var dataStore = new DataStore())
            {
                var sql = $@"SELECT 
                            a.UseCount,
                            a.Id,
                            a.Name,
                            a.IconUrl,
                            a.Remark,
                            a.AppStyle,
                            (
	                            SELECT COUNT(*) FROM dbo.AppStars AS a1
	                            WHERE a1.AccountId={accountId} AND a1.AppId=a.Id AND a1.IsDeleted=0
                            ) AS  AppStars,
                            (
	                            SELECT COUNT(*) FROM dbo.Members AS a1 WHERE a1.AccountId={accountId} AND a1.IsDeleted=0
                            ) AS IsInstall
                            FROM dbo.Apps AS a 
                            WHERE a.AppAuditState={(Int32)AppAuditState.Pass} AND a.AppReleaseState={(Int32)AppReleaseState.Release} AND a.IsRecommand=1";
                var dataReader = dataStore.SqlGetDataReader(sql);
                if(!dataReader.HasRows)
                {
                    return null;
                }
                while(dataReader.Read())
                {
                    return new
                    {
                        UseCount = Int32.Parse(dataReader["UseCount"].ToString()),
                        Id = Int32.Parse(dataReader["Id"].ToString()),
                        Name = dataReader["Name"].ToString(),
                        IconUrl = dataReader["IconUrl"].ToString(),
                        Remark = dataReader["Remark"].ToString(),
                        AppStyle = Int32.Parse(dataReader["AppStyle"].ToString()),
                        AppStars = Int32.Parse(dataReader["AppStars"].ToString()),
                        IsInstall = Int32.Parse(dataReader["IsInstall"].ToString()) > 0 ? true : false
                    };
                }
            }
        }
    }
}
