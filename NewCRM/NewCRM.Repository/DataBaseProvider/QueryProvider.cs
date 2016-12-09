using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.DomainSpecification;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Repositories;
using NewCRM.Domain.UnitWork;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Repository.UnitOfWorkProvide;

namespace NewCRM.Repository.DataBaseProvider
{
    /// <summary>
    /// 提供查询
    /// </summary>
    [Export(typeof(IDomainModelQueryProvider))]
    internal class QueryProvider : InternalImportUnitOfWork, IDomainModelQueryProvider
    {
        #region 仓储上下文的实例


        #endregion

        #region 属性

        /// <summary>
        ///     获取 EntityFramework的数据仓储上下文
        /// </summary>
        private UnitOfWorkContextBase EfContext
        {
            get
            {
                var unitofwork = UnitOfWork as UnitOfWorkContextBase;

                if (unitofwork != null)
                {
                    return unitofwork;
                }

                throw new RepositoryException($"无法获取当前工作单元的实例:{nameof(UnitOfWork)}");
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="specification"></param>
        /// <returns></returns>
        public IQueryable<T> Query<T>(Specification<T> specification) where T : DomainModelBase, IAggregationRoot
        {
            return EfContext.Set<T, Int32>().Where(specification.Expression);
        }

        public IQueryable<dynamic> Query<T>(Specification<T> selector, Expression<Func<T, dynamic>> selectorField) where T : DomainModelBase, IAggregationRoot
        {
            return EfContext.Set<T, Int32>().Where(selector.Expression).Select(selectorField);
        }

        #endregion

    }
}
