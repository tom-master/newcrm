using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.Entities.DomainModel;
using NewCRM.Domain.Entities.DomainSpecification;
using NewCRM.Domain.Entities.Repositories;
using NewCRM.Domain.Entities.UnitWork;
using NewCRM.Infrastructure.CommonTools.CustemException;
using NewCRM.Repository.UnitOfWorkProvide;

namespace NewCRM.Repository
{
    [Export(typeof(IDomainModelQueryProvider))]
    public class QueryProvider : IDomainModelQueryProvider
    {
        #region 仓储上下文的实例

        /// <summary>
        ///     获取 仓储上下文的实例
        /// </summary>

        [Import]
        private IUnitOfWork UnitOfWork { get; set; }

        #endregion

        #region 属性

        /// <summary>
        ///     获取 EntityFramework的数据仓储上下文
        /// </summary>
        private UnitOfWorkContextBase EfContext
        {
            get
            {
                if (UnitOfWork is UnitOfWorkContextBase)
                {
                    return UnitOfWork as UnitOfWorkContextBase;
                }
                throw new RepositoryException($"无法获取当前工作单元的实例:{UnitOfWork}");
            }
        }


        public IQueryable<T> Query<T>(ISpecification<T> selector) where T : DomainModelBase, IAggregationRoot
        {
            return EfContext.Set<T, Int32>().Where(model => selector.IsSatisfiedBy(model));
        }

        #endregion

    }
}
