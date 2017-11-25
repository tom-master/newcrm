using System;
using NewCRM.Domain.Entitys.System;

namespace NewCRM.Domain.Services.Interface
{
    public interface IModifyAppInfoServices
    {
        /// <summary>
        /// app打分
        /// </summary>
        void ModifyAppStar(Int32 accountId, Int32 appId, Int32 starCount);

        /// <summary>
        /// 修改开发者（用户）的app信息
        /// </summary>
        void ModifyAccountAppInfo(Int32 accountId, App app);

        /// <summary>
        /// 删除App分类
        /// </summary>
        void DeleteAppType(Int32 appTypeId);

        /// <summary>
        /// 创建新的app分类
        /// </summary>
        void CreateNewAppType(AppType appType);

        /// <summary>
        /// 修改app分类
        /// </summary>
        void ModifyAppType(String appTypeName, Int32 appTypeId);
    }
}
