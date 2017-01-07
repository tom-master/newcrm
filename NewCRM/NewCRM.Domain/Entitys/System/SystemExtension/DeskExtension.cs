using System;
using System.Linq;

namespace NewCRM.Domain.Entitys.System
{
    public partial class Desk
    {
        #region public method

        public Desk AddMember(Member member)
        {
            if (member == null)
            {
                throw new ArgumentNullException($"{nameof(member)}不能为空");
            }

            Members.Add(member);

            return this;
        }

        public void RemoveMember(Int32 memberId)
        {
            if (memberId <= 0)
            {
                throw new ArgumentNullException($"{nameof(memberId)}不能为0");
            }

            Members.Where(member => member.Id == memberId).ToList().ForEach(m => { m.Remove(); });
        }


        public override String KeyGenerator()
        {
            return $"NewCRM:{nameof(Desk)}:AccountId:{AccountId}:Id:{Id}";
        }


        #endregion
    }
}
