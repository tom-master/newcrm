using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewCRM.Domain.Entitys.Security;

namespace NewCRM.Domain.Services.Interface
{
    public interface ISecurityContext
    {
        /// <summary>
        /// 获取角色
        /// </summary>
        Role GetRole(Int32 roleId);

        /// <summary>
        /// 获取权限列表
        /// </summary>
        /// <returns></returns>
        IList<RolePower> GetPowers();

        /// <summary>
        /// 获取角色列表
        /// </summary>
        List<Role> GetRoles(String roleName, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);

        /// <summary>
        /// 移除角色
        /// </summary>
        void RemoveRole(Int32 roleId);

        /// <summary>
        /// 添加新角色
        /// </summary>
        void AddNewRole(Role role);

        /// <summary>
        /// 修改角色 
        /// </summary>
        void ModifyRole(Role role);

        /// <summary>
        /// 添加权限到角色
        /// </summary>
        void AddPowerToCurrentRole(Int32 roleId, IEnumerable<Int32> powerIds);

        /// <summary>
        /// 检查授权
        /// </summary>
        bool CheckPermissions(int accessAppId, params int[] roleIds);
    }
}
