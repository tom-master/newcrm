using System;
using NewCRM.Domain.Entities.DomainModel.System;

namespace NewCRM.Domain.Interface
{
    public interface IAppServices
    {
        /// <summary>
        /// 修改开发者（用户）的app信息
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="app"></param>
        void ModifyAccountAppInfo(Int32 accountId, App app);

        /// <summary>
        /// app打分
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="appId"></param>
        /// <param name="starCount"></param>
        void ModifyAppStar(Int32 accountId, Int32 appId, Int32 starCount);

        /// <summary>
        /// 用户安装app
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="appId"></param>
        /// <param name="deskNum"></param>
        void InstallApp(Int32 accountId, Int32 appId, Int32 deskNum);
    }
}
