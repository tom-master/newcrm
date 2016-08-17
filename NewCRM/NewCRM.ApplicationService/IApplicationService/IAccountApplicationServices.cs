using System;
using System.Collections.Generic;
using NewCRM.Dto.Dto;

namespace NewCRM.Application.Services.IApplicationService
{
    public interface IAccountApplicationServices
    {
        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        UserDto Login(String userName, String password);

        /// <summary>
        /// 用户登出
        /// </summary>
        /// <param name="userId"></param>
        void Logout(Int32 userId);

        /// <summary>
        /// 用户启用
        /// </summary>
        /// <param name="userId"></param>
        void Enable(Int32 userId);

        /// <summary>
        /// 用户禁用
        /// </summary>
        /// <param name="userId"></param>
        void Disable(Int32 userId);

        /// <summary>
        /// 获取登陆用户的配置文件
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserDto GetUserConfig(Int32 userId);

        /// <summary>
        /// 获取全部的用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userType"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<UserDto> GetAllUsers(String userName, String userType, Int32 pageIndex, Int32 pageSize, out Int32 totalCount);

        /// <summary>
        /// 根据用户id获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        UserDto GetUser(Int32 userId);

        /// <summary>
        /// 添加新的用户
        /// </summary>
        /// <param name="userDto"></param>
        void AddNewUser(UserDto userDto);

    }
}
