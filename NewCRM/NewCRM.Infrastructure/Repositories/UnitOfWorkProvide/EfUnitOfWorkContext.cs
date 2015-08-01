using System.Data.Entity;
using NewCRM.Infrastructure.Repositories.RepositoryProvide;
namespace NewCRM.Infrastructure.Repositories.UnitOfWorkProvide
{
    /// <summary>
    ///     数据单元操作类
    /// </summary>
    
    public class EfUnitOfWorkContext : UnitOfWorkContextBase
    {
        /// <summary>
        /// 获取 当前使用的数据访问上下文对象
        /// </summary>
        protected override DbContext Context
        {
            get { return DbContextFactory.GetDbContext; }
        }
    }

    internal static class DbContextFactory
    {
        private static readonly NewCrmBackSite _db = new NewCrmBackSite();

        internal static DbContext GetDbContext
        {
            get { return _db; }
        }
    }
}