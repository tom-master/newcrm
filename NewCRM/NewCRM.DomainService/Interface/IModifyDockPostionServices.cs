using System;

namespace NewCRM.Domain.Services.Interface
{
    public interface IModifyDockPostionServices
    {
	    /// <summary>
	    /// 修改应用码头的位置
	    /// </summary>
	    /// <param name="accountId"></param>
	    /// <param name="defaultDeskNumber"></param>
	    /// <param name="newPosition"></param>
	    void ModifyDockPosition(Int32 accountId,Int32 defaultDeskNumber, String newPosition);
    }
}
