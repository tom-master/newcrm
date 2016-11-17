using System;
using System.ComponentModel.Composition;
using System.Data.Entity;
using NewCRM.Domain.Entities.UnitWork;

namespace NewCRM.Repository.UnitOfWorkProvide
{
    /// <summary>
    ///     数据单元操作类
    /// </summary> 
    [Export(typeof(IUnitOfWork))]
    public class EfUnitOfWorkContext : UnitOfWorkContextBase
    {
        /// <summary>
        ///     获取 当前使用的数据访问上下文对象
        /// </summary>
        protected override DbContext Context
        {
            get
            {
                //bool secondCachingEnabled = ConfigurationManager.AppSettings["EntityFrameworkCachingEnabled"].CastTo(false);
                return /*secondCachingEnabled ? EFCachingDbContext.Value :*/ EfDbContext.Value;
            }
        }

        [Import(typeof(DbContext))]
        private Lazy<NewCrmBackSite> EfDbContext { get; set; }

        //[Import("EFCaching", typeof(DbContext))]
        //private Lazy<EFCachingDbContext> EFCachingDbContext { get; set; }
    }
}
