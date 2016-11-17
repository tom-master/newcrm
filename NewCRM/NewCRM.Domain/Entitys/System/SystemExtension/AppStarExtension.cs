namespace NewCRM.Domain.Entitys.System
{
    public partial class AppStar
    {
        public void RemoveStar()
        {
            IsDeleted = true;
        }
    }
}
