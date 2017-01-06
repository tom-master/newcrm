using System;

namespace NewCRM.Domain.Interface.BoundedContextMember
{
    public interface IInstallAppServices
    {
        /// <summary>
        /// 用户安装app
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="deskNum"></param>
        void Install(Int32 appId, Int32 deskNum);
    }
}
