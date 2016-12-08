using System;
using System.Collections.Generic;
using NewCRM.Dto.Dto;

namespace NewCRM.Application.Interface
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
        /// 获取所有的权限
        /// </summary>
        /// <returns></returns>
        List<PowerDto> GetAllPowers();

        /// <summary>
        /// 根据角色Id获取角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        RoleDto GetRole(Int32 roleId);

        /// <summary>
        /// 创建新权限
        /// </summary>
        /// <param name="power"></param>
        void AddNewPower(PowerDto power);

        /// <summary>
        /// 获取所有的权限
        /// </summary>
        /// <param name="powerName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<PowerDto> GetAllPowers(String powerName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);


        /// <summary>
        /// 根据powerId获取权限信息
        /// </summary>
        /// <param name="powerId"></param>
        /// <returns></returns>
        PowerDto GetPower(Int32 powerId);

        /// <summary>
        /// 修改权限信息
        /// </summary>
        /// <param name="power"></param>
        void ModifyPower(PowerDto power);

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="powerId"></param>
        void RemovePower(Int32 powerId);

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
        /// 获取全部角色
        /// </summary>
        /// <returns></returns>
        List<RoleDto> GetAllRoles();

        /// <summary>
        /// 检查用户权限
        /// </summary>
        /// <param name="powerName"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        Boolean CheckPermissions(String powerName, params Int32[] roleIds);
    }
}
