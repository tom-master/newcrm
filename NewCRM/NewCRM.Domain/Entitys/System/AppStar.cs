using System;

namespace NewCRM.Domain.Entitys.System
{
    public partial class AppStar : DomainModelBase
    {
        public Int32 AccountId { get; private set; }

        public Int32 AppId { get; private set; }

        public Double StartNum { get; private set; }

        public AppStar(Int32 accountId, Double startNum)
        {
            AccountId = accountId;
            StartNum = startNum;
        }

        public AppStar() { }
    }
}
