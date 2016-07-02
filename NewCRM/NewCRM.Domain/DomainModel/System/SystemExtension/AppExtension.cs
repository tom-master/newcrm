using System;
using NewCRM.Domain.Entities.ValueObject;

namespace NewCRM.Domain.Entities.DomainModel.System
{
    public partial class App
    {
        #region public method

        /// <summary>
        /// 更新用户使用数
        /// </summary>
        /// <param name="userCount"></param>
        public void ModifyUserCount(Int32 userCount)
        {
            if (userCount <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(userCount)}:不能为0");
            }
            UserCount += userCount;

        }

        /// <summary>
        /// 更新评价等级
        /// </summary>
        /// <param name="startCount"></param>
        public void ModifyStartCount(Int32 startCount)
        {
            if (startCount <= 0)
            {
                throw new ArgumentOutOfRangeException($"{nameof(startCount)}:不能为0");
            }
            StartCount += startCount;
        }


        /// <summary>
        /// app审核通过
        /// </summary>
        public void Pass()
        {
            AppAuditState = AppAuditState.Pass;

        }

        /// <summary>
        /// app审核未通过
        /// </summary>
        public void Deny()
        {
            AppAuditState = AppAuditState.Deny;

        }



        #endregion
    }
}
