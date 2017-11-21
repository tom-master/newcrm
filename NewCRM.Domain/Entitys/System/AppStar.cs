using System;

namespace NewCRM.Domain.Entitys.System
{
    public partial class AppStar : DomainModelBase
    {
        /// <summary>
        /// 账号
        /// </summary>
        public Int32 AccountId { get; private set; }

        /// <summary>
        /// 评分
        /// </summary>
        public Double StartNum { get; private set; }

        public AppStar(Int32 accountId, Double startNum)
        {
            AccountId = accountId;
            StartNum = startNum;
        }

        public AppStar()
        {
        }
    }
}
