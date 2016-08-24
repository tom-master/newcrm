using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entities.DomainModel.Account;
using NewCRM.Domain.Entities.ValueObject;
using NewCRM.Infrastructure.CommonTools;
using NewCRM.Infrastructure.CommonTools.CustemException;

namespace NewCRM.Domain.Services.Impl
{
    [Export(typeof(ISecurityContext))]
    internal sealed class SecurityContext : BaseService, ISecurityContext
    {
        [Import]
        public IRoleServices RoleServices { get; set; }

        [Import]
        public IPowerServices PowerServices { get; set; }

        #region User

        public List<dynamic> GetAllUsers(String userName, String userType, Int32 pageIndex, Int32 pageSize, out Int32 totalCount)
        {
            var users = UserRepository.Entities;
            if ((userName + "").Length > 0)
            {
                users = users.Where(user => user.Name.Contains(userName));
            }


            UserType internalUserType;
            if ((userType + "").Length > 0)
            {
                var enumConst = Enum.GetName(typeof(UserType), userType);

                if (Enum.TryParse(enumConst, true, out internalUserType))
                {
                    users = users.Where(user => user.IsAdmin == (internalUserType == UserType.Admin));
                }
                else
                {
                    throw new BusinessException($"用户类型{userType}不是有效的类型");
                }
            }

            totalCount = users.Count();

            return users.OrderByDescending(o => o.AddTime).Skip((pageIndex - 1) * pageSize).Take(pageSize).Select(user => new
            {
                user.Id,
                UserType = user.IsAdmin ? "2"/*管理员*/ : "1"/*用户*/,
                user.Name
            }).ToList<dynamic>();
        }

        public dynamic GetUser(Int32 userId)
        {
            var userResult = UserRepository.Entities.FirstOrDefault(user => user.Id == userId);
            if (userResult == null)
            {
                throw new BusinessException("该用户可能已被禁用或被删除，请联系管理员");
            }

            return new
            {
                userResult.Id,
                userResult.Name,
                Password = userResult.LoginPassword,
                UserType = userResult.IsAdmin ? "2" : "1",
                Roles = userResult.Roles.Select(s => new
                {
                    Id = s.RoleId
                })
            };
        }

        public void AddNewUser(User user)
        {
            UserType userType;

            var enumConst = Enum.GetName(typeof(UserType), user.IsAdmin ? "2"/*管理员*/ : "1"/*用户*/);

            if (!Enum.TryParse(enumConst, true, out userType))
            {
                throw new BusinessException($"类型{enumConst}不是有效的枚举类型");
            }

            var internalNewUser = new User(user.Name, PasswordUtil.CreateDbPassword(user.LoginPassword), userType);

            var userRoles = internalNewUser.Roles.ToList();

            if (userRoles.Any())
            {
                userRoles.ForEach(userRole =>
                {
                    userRole.Remove();
                });
            }

            internalNewUser.AddUserRole(user.Roles.Select(role => role.RoleId).ToArray());

            UserRepository.Add(internalNewUser);
        }

        public void ModifyUser(User user)
        {
            var userResult = UserRepository.Entities.FirstOrDefault(internalUser => internalUser.Id == user.Id);

            if (userResult == null)
            {
                throw new BusinessException($"用户{user.Name}可能已被禁用或删除");
            }

            if ((user.LoginPassword + "").Length > 0)
            {
                var newPassword = PasswordUtil.CreateDbPassword(user.LoginPassword);
                userResult.ModifyPassword(newPassword);
            }

            if (userResult.Roles.Any())
            {
                userResult.Roles.ToList().ForEach(role =>
                {
                    role.Remove();
                });
            }

            userResult.AddUserRole(user.Roles.Select(role => role.RoleId).ToArray());

            UserRepository.Update(userResult);
        }

        public Boolean ValidSameUserNameExist(String userName)
        {
            return UserRepository.Entities.Any(user => user.Name == userName);
        }

        #endregion
    }
}
