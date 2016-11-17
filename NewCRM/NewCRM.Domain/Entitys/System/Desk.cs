using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NewCRM.Domain.Entitys.System
{
    [Description("桌面"), Serializable]
    public partial class Desk : DomainModelBase, IAggregationRoot
    {
        #region public property

        public Int32 DeskNumber { get; private set; }

        public virtual ICollection<Member> Members { get; private set; }

        public  Int32 ConfigId { get; set; }
        #endregion


        #region ctor
        public Desk(Int32 deskNumber)
        {
            DeskNumber = deskNumber;
            Members = new List<Member>();
            AddTime = DateTime.Now;
        }


        public Desk() { }
        #endregion



    }
}
