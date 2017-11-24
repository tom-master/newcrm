using System;

namespace NewCRM.Domain.Services.Interface
{
    public interface IRecommendAppServices
    {
        /// <summary>
        /// 获取今日推荐
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        dynamic GetTodayRecommend(Int32 accountId);
    }
}
