using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using NewCRM.Domain.Entities.DomainModel.Account;

namespace NewCRM.Domain.Services
{
    public interface ISecurityContext
    {
        IRoleServices RoleServices { get; set; }

        IPowerServices PowerServices { get; set; }

        /// <summary>
        /// 获取所有的用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<dynamic> GetAllUsers(String userName, String userType, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);

        /// <summary>
        /// 根据userId获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        dynamic GetUser(Int32 userId);

        /// <summary>
        /// 添加新的用户
        /// </summary>
        /// <param name="user"></param>
        void AddNewUser(User user);

        /// <summary>
        /// 验证相同的用户名是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Boolean ValidSameUserNameExist(String userName);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user"></param>
        void ModifyUser(User user);
    }
}
