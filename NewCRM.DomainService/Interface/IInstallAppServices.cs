using System;

namespace NewCRM.Domain.Services.Interface
{
    public interface IInstallAppServices
    {
	    /// <summary>
	    /// 用户安装app
	    /// </summary> 
	    void Install(Int32 accountId,Int32 appId, Int32 deskNum);

        /// <summary>
        /// 获取当前账户下已开发和未发布的app
        /// </summary>
        Tuple<Int32, Int32> GetAccountDevelopAppCountAndNotReleaseAppCount(Int32 accountId);
    }
}
