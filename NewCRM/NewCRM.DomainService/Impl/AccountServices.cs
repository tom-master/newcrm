using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.DomainModel.System;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.Impl
{
    [Export(typeof(IAccountServices))]
    internal class AccountServices : BaseService, IAccountServices
    {
        #region public method

        public User Validate(String userName, String password)
        {
            var userResult = UserRepository.Entities.FirstOrDefault(user => user.Name == userName && user.IsDisable == false && user.IsDeleted == false);
            if (userResult == null)
            {
                throw new BusinessException($"该用户不存在或被禁用{userName}");
            }
            if (!PasswordUtil.ComparePasswords(userResult.LoginPassword, password))
            {
                throw new BusinessException("密码错误");
            }
            userResult.Online();

            UserRepository.Update(userResult);

            OnlineRepository.Add(new Online(GetCurrentIpAddress(), userResult.Id));

            return userResult;

        }

        public void Logout(Int32 userId)
        {
            var userResult = GetUserInfoService(userId);

            if (!userResult.IsOnline)
            {
                throw new BusinessException("该用户可能已在其他地方下线");
            }
            userResult.Offline();

            UserRepository.Update(userResult);

            ModifyOnlineState(userId);
        }

        public void Disable(Int32 userId)
        {
            var userResult = GetUserInfoService(userId);

            if (userResult.IsDisable)
            {
                throw new BusinessException("该用户可能已在其他地方被禁用");
            }

            userResult.Disable();
            userResult.Offline();

            UserRepository.Update(userResult);

            ModifyOnlineState(userId);
        }

        public void Enable(Int32 userId)
        {
            var userResult = GetUserInfoService(userId);
            userResult.Enable();

            UserRepository.Update(userResult);
        }

        public User GetUserConfig(Int32 userId)
        {
            var userResult = GetUserInfoService(userId);

            return userResult;
        }

        public List<dynamic> GetAllUsers(String userName, Int32 userType, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            var users = UserRepository.Entities;
            if ((userName + "").Length > 0)
            {
                users = users.Where(user => user.Name.Contains(userName));
            }

            if (userType != 0)
            {
                var enumConst = Enum.GetName(typeof(UserType), userType);

                UserType internalUserType;
                if (Enum.TryParse(enumConst, true, out internalUserType))
                {
                    users = users.Where(user => user.IsAdmin == (internalUserType == UserType.Admin));
                }
                else
                {
                    throw new BusinessException($"类型{enumConst}不是有效的用户类型");
                }
            }

            totalCount = users.Count();

            return users.OrderByDescending(o => o.AddTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(user => new
            {
                user.Id,
                UserType = user.IsAdmin ? "管理员" : "用户",
                user.Name
            }).ToList<dynamic>();
        }

        public User GetUser(Int32 userId)
        {
            var userResult = UserRepository.Entities.FirstOrDefault(user => user.Id == userId);
            if (userResult == null)
            {
                throw new BusinessException("该用户可能已被禁用或被删除，请联系管理员");
            }
            return null;
        }

        #endregion

        #region private method

        /// <summary>
        /// 获取当前登陆的ip
        /// </summary>
        /// <returns></returns>
        private String GetCurrentIpAddress()
        {
            IPHostEntry localhost = Dns.GetHostEntry(Dns.GetHostName());
            return (localhost.AddressList[0]).ToString();
        }

        /// <summary>
        /// 修改在线状态
        /// </summary>
        /// <param name="userId"></param>
        private void ModifyOnlineState(Int32 userId)
        {
            var onlineResult = OnlineRepository.Entities.FirstOrDefault(online => online.UserId == userId);
            OnlineRepository.Remove(onlineResult);
        }

        #endregion
    }
}
