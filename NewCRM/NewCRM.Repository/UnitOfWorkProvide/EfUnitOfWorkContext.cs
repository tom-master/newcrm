using System;
using System.ComponentModel.Composition;
using System.Data.Entity;
using NewCRM.Domain.UnitWork;

namespace NewCRM.Repository.UnitOfWorkProvide
{
    /// <summary>
    /// 数据单元操作类
    /// </summary> 
    [Export(typeof(IUnitOfWork))]
    public class EfUnitOfWorkContext : UnitOfWorkContextBase
    {
        /// <summary>
        /// 获取 当前使用的数据访问上下文对象
        /// </summary>
        protected override DbContext Context => EfDbContext.Value;

        [Import(typeof(DbContext))]
        private Lazy<NewCrmBackSite> EfDbContext { get; set; }

    }
}
