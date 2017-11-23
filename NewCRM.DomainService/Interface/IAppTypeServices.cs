using System.Collections.Generic;
using NewCRM.Domain.Entitys.System;

namespace NewCRM.Domain.Services.Interface
{
    public interface IAppTypeServices
    {
        /// <summary>
        /// 获取所有App类型
        /// </summary>
        /// <returns></returns>
        List<AppType> GetAppTypes();
    }
}
