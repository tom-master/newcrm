using System;
using System.Collections.Generic;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Services.Impl;

namespace NewCRM.Domain.Services
{
    public interface IAppServices
    {
        /// <summary>
        /// 开发者（用户）创建新的app
        /// </summary>
        /// <param name="app"></param>
        void CreateNewApp(App app);

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
