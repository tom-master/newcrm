using System;

namespace NewCRM.Domain.Entities.DomainModel.System
{
    public partial class AppStar : DomainModelBase
    {
        public Int32 UserId { get; private set; }

        public Int32 AppId { get; private set; }

        public Double StartNum { get; private set; }

        public AppStar(Int32 userId, Double startNum)
        {
            UserId = userId;
            StartNum = startNum;
        }

        public AppStar() { }
    }
}
