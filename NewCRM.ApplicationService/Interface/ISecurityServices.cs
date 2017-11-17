using System;
using System.Collections.Generic;
using NewCRM.Dto;

namespace NewCRM.Application.Services.Interface
{
    public interface ISecurityServices
    {
        #region have return value

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
        /// 根据角色Id获取角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        RoleDto GetRole(Int32 roleId);

        /// <summary>
        /// 获取全部角色
        /// </summary>
        /// <returns></returns>
        List<RoleDto> GetAllRoles();

        /// <summary>
        /// 检查用户权限
        /// </summary>
        /// <param name="accessAppId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        Boolean CheckPermissions(Int32 accessAppId, params Int32[] roleIds);

        #endregion

        #region not have return value

        /// <summary>
        /// 新建角色
        /// </summary>
        /// <param name="role"></param>
        void AddNewRole(RoleDto role);

        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="role"></param>
        void ModifyRole(RoleDto role);

        /// <summary>
        /// 添加权限到当前的角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="powerIds"></param>
        void AddPowerToCurrentRole(Int32 roleId, IEnumerable<Int32> powerIds);

        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="roleId"></param>
        void RemoveRole(Int32 roleId);

        #endregion
    }
}
