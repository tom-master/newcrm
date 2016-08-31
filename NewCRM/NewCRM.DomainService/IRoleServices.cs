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
        /// 添加新的角色
        /// </summary>
        void AddNewRole(Role role);

        /// <summary>
        /// 修改角色信息
        /// </summary>
        /// <param name="role"></param>
        void ModifyRole(Role role);

        /// <summary>
        /// 添加权限到当前角色
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="powerIds"></param>
        void AddPowerToCurrentRole(Int32 roleId, IEnumerable<Int32> powerIds);

    }
}
