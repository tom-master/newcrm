using System;
using System.Collections.Generic;
using NewCRM.Dto.Dto;

namespace NewCRM.Application.Services.IApplicationService
{
    public interface ISecurityApplicationServices
    {
        /// <summary>
        /// 获取全部的角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<RoleDto> GetAllRoles(String roleName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="roleId"></param>
        void RemoveRole(Int32 roleId);

        /// <summary>
        /// 获取只有管理员才能使用的应用
        /// </summary>
        /// <returns></returns>
        List<AppDto> GetSystemRoleApps();

        /// <summary>
        /// 根据角色Id获取角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        RoleDto GetRoleInfo(Int32 roleId);
    }
}
