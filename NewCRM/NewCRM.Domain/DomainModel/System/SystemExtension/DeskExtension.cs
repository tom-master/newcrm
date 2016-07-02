using System;
using System.Linq;

namespace NewCRM.Domain.Entities.DomainModel.System
{
    public partial class Desk
    {
        #region public method

        public void AddDeskMember(Member member)
        {
            if (member == null)
            {
                throw new ArgumentNullException($"{nameof(member)}不能为空");
            }
            Members.Add(member);
        }

        public void RemoveDeskMember(Int32 memberId)
        {
            if (memberId <= 0)
            {
                throw new ArgumentNullException($"{nameof(memberId)}不能为0");
            }
            Enumerable.Where<Member>(Members, member => member.Id == memberId).ToList().ForEach(m => { m.RemoveMember(); });
        }


        #endregion
    }
}
