using System;
using System.Linq;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Domain.Entitys.Security
{
    public partial class Role
    {
        #region public method

        /// <summary>
        /// 修改角色名称
        /// </summary>
        public Role ModifyRoleName(String newRoleName)
        {
            Name = newRoleName;
            return this;
        }

        /// <summary>
        /// 修改角色标识
        /// </summary>
        /// <returns></returns>
        public Role ModifyRoleIdentity(String newRoleIdentity)
        {
            RoleIdentity = newRoleIdentity;
            return this;
        }



        /// <summary>
        /// 为角色添加权限
        /// </summary>
        public Role AddPower(params Int32[] powerIds)
        {
            if (powerIds == null)
            {
                throw new BusinessException($"{nameof(powerIds)}不能为空");
            }
            
            foreach (var powerid in powerIds)
            {
                Powers.Add(new RolePower(Id, powerid));
            }

            return this;
        }

        /// <summary>
        /// 移除角色权限
        /// </summary>
        public Role RemovePower(params Int32[] powerIds)
        {
            if (powerIds == null)
            {
                throw new BusinessException($"{nameof(powerIds)}不能为空");
            }

            if (!powerIds.Any())
            {
                throw new BusinessException($"{nameof(powerIds)}不能为0");
            }

            foreach (var powerId in powerIds)
            {
                Powers.FirstOrDefault(p => p.AppId == powerId)?.Remove();
            }
            return this;
        }

        /// <summary>
        /// 移除角色
        /// </summary>
        public void Remove()
        {
            if (Powers.Any())
            {
                Powers.ToList().ForEach(p =>
                {
                    p.Remove();
                });
            }

            IsDeleted = true;
        }


        /// <summary>
        /// 检查权限是否存在
        /// </summary>
        public Boolean CheckPower(params Int32[] appIds)
        {
            if (!appIds.Any())
            {
                throw new BusinessException($"对不起，您没有访问的权限！");
            }

            return appIds.Any(appId =>
            {
                var internalPower = Powers.FirstOrDefault(power => power.AppId == appId);
                return internalPower?.AppId == appId;
            });
        }

        public override String KeyGenerator()
        {
            return $"NewCRM:{nameof(Role)}:Id:{Id}";
        }

        #endregion
    }
}
