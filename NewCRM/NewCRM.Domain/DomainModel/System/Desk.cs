using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NewCRM.Domain.DomainModel.System
{
    [Description("桌面"), Serializable]
    public partial class Desk : DomainModelBase, IAggregationRoot
    {
        #region public property

        public Int32 DeskNumber { get; private set; }

        public virtual ICollection<Member> Members { get; private set; }
        #endregion


        #region ctor
        public Desk(Int32 deskNumber)
        {
            DeskNumber = deskNumber;
            Members = new List<Member>();
        }


        public Desk() { }
        #endregion



    }
}
