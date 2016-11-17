using System;
using NewCRM.Domain.Entitys.System;

namespace NewCRM.Domain.Interface.BoundedContextMember
{
    public interface IModifyAppInfoServices
    {
        /// <summary>
        /// app打分
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="appId"></param>
        /// <param name="starCount"></param>
        void ModifyAppStar(Int32 accountId, Int32 appId, Int32 starCount);


        /// <summary>
        /// 修改开发者（用户）的app信息
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="app"></param>
        void ModifyAccountAppInfo(Int32 accountId, App app);

    }
}
