using System;
using System.Collections.Generic;
using NewCRM.Domain.Entities.DomainModel.Security;

namespace NewCRM.Domain.Services
{
    public interface IRoleServices
    {
        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="roleId"></param>
        void RemoveRole(Int32 roleId);

        /// <summary>
        /// 根据角色Id获取角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        dynamic GetRole(Int32 roleId);

        /// <summary>
        /// 添加新的角色
        /// </summary>
        void AddNewRole(Role role);

        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="role"></param>
        void ModifyRole(Role role);

        /// <summary>
        /// 获取全部的角色
        /// </summary>
        /// <returns></returns>
        List<dynamic> GetAllRoles();

        /// <summary>
        /// 获取全部的角色
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<dynamic> GetAllRoles(String roleName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);

        /// <summary>
        /// 添加权限到当前角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="powerIds"></param>
        void AddPowerToCurrentRole(Int32 roleId, IEnumerable<Int32> powerIds);

    }
}
