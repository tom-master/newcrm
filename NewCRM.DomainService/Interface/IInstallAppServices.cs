using System;

namespace NewCRM.Domain.Services.Interface
{
    public interface IInstallAppServices
    {
	    /// <summary>
	    /// 用户安装app
	    /// </summary>
	    /// <param name="accountId"></param>
	    /// <param name="appId"></param>
	    /// <param name="deskNum"></param>
	    void Install(Int32 accountId,Int32 appId, Int32 deskNum);
    }
}
