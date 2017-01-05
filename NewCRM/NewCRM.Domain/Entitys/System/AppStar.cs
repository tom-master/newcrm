using System;

namespace NewCRM.Domain.Entitys.System
{
    public partial class AppStar : DomainModelBase
    {
        public Int32 AccountId { get; private set; }


        public virtual App App { get; set; }

        public Double StartNum { get; private set; }

        public AppStar(Int32 accountId, Double startNum):this()
        {
            AccountId = accountId;
            StartNum = startNum;
        }

        public AppStar() { AddTime = DateTime.Now; }
    }
}
