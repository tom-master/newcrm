using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NewCRM.Domain.Entitys.System
{
    [Description("桌面"), Serializable]
    public partial class Desk : DomainModelBase, IAggregationRoot
    {
        #region public property

        [Required()]
        public Int32 DeskNumber { get; private set; }
         
        [Required()]
        public Int32 AccountId { get; set; }

        public virtual ICollection<Member> Members { get; private set; }
        #endregion 

        #region ctor
        public Desk(Int32 deskNumber, Int32 accountId)
        {
            DeskNumber = deskNumber;
            Members = new List<Member>();

            AccountId = accountId;
        }

        #endregion

    }
}
