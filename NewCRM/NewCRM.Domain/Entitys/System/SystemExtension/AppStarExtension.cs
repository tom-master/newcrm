namespace NewCRM.Domain.Entitys.System
{
    public partial class AppStar
    {
        /// <summary>
        /// 移除app的评价分数
        /// </summary>
        public void RemoveStar()
        {
            IsDeleted = true;
        }
    }
}
