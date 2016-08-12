using System;
using System.Collections.Generic;
using NewCRM.Domain.Entities.DomainModel.Security;

namespace NewCRM.Domain.Services
{
    public interface ISecurityServices
    {
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
        /// 移除角色
        /// </summary>
        /// <param name="roleId"></param>
        void RemoveRole(Int32 roleId);

        /// <summary>
        /// 获取只有管理员才能使用的app
        /// </summary>
        /// <returns></returns>
        List<dynamic> GetSystemRoleApps();


        /// <summary>
        /// 根据角色Id获取角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        dynamic GetRoleInfo(Int32 roleId);

        /// <summary>
        /// 创建新权限
        /// </summary>
        /// <param name="power"></param>
        void AddNewPower(Power power);


        /// <summary>
        /// 获取所有的权限
        /// </summary>
        /// <param name="powerName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<Power> GetAllPowers(String powerName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);

        /// <summary>
        /// 根据powerId获取权限信息
        /// </summary>
        /// <param name="powerId"></param>
        /// <returns></returns>
        Power GetPower(Int32 powerId);

        /// <summary>
        /// 修改权限信息
        /// </summary>
        /// <param name="power"></param>
        void ModifyPower(Power power);

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="powerId"></param>
        void RemovePower(Int32 powerId);
    }
}
