using System;
using System.Linq;
using NewCRM.Infrastructure.CommonTools.CustomException;

namespace NewCRM.Domain.Entitys.System
{
    public partial class Desk
    {
        #region public method

        /// <summary>
        /// 添加桌面成员
        /// </summary>
        public Desk AddMember(Member member)
        {
            if (member == null)
            {
                throw new BusinessException($"{nameof(member)}不能为空");
            }

            Members.Add(member);
            return this;
        }

        /// <summary>
        /// 移除桌面成员
        /// </summary>
        public void RemoveMember(Int32 memberId)
        {
            if (memberId <= 0)
            {
                throw new BusinessException($"{nameof(memberId)}不能为0");
            }

            Members.Where(member => member.Id == memberId).ToList().ForEach(m => { m.Remove(); });
        }

        public override String KeyGenerator()
        {
            return $"NewCRM:{nameof(Desk)}:AccountId:{AccountId}";
        }
        #endregion
    }
}
