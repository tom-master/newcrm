using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using NewCRM.Domain.DomainSpecification;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Repositories;
using NewCRM.Domain.UnitWork;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.Repository.UnitOfWorkProvide;
using NewCRM.Repository.DataBaseProvider.Redis;

namespace NewCRM.Repository.DataBaseProvider.EF
{
    /// <summary>
    /// 提供查询
    /// </summary>
    [Export(typeof(IDomainModelQueryProvider))]
    internal class QueryProvider : IDomainModelQueryProvider
    {
        [Import]
        private ICacheQueryProvider _cacheQueryProvider;

        #region 仓储上下文的实例


        #endregion

        #region 属性

        /// <summary>
        /// 获取 仓储上下文的实例
        /// </summary>
        [Import]
        protected IUnitOfWork UnitOfWork { get; set; }

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

        #endregion

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

        public T Query<T>(Expression<Func<T, Boolean>> entity) where T : DomainModelBase, IAggregationRoot
        {

            String internalKey = entity.GeneratorRedisKey<T>();

            var cacheValue = _cacheQueryProvider.StringGet<T>(internalKey);

            if (cacheValue == null)
            {
                var value = EfContext.Set<T, Int32>().FirstOrDefault(entity);

                if (_cacheQueryProvider.KeyExists(internalKey))
                {
                    _cacheQueryProvider.KeyDelete(internalKey);
                }

                _cacheQueryProvider.StringSet(internalKey, value);

                return value;
            }

            return cacheValue;
        }

        public IEnumerable<T> Querys<T>(Expression<Func<T, Boolean>> entity) where T : DomainModelBase, IAggregationRoot
        {
            String internalKey = entity.GeneratorRedisKey<T>();

            var cacheValue = _cacheQueryProvider.ListRange<T>(internalKey);

            if (cacheValue == null || !cacheValue.Any())
            {
                IList<T> values = EfContext.Set<T, Int32>().Where(entity).ToList();

                if (_cacheQueryProvider.KeyExists(internalKey))
                {
                    _cacheQueryProvider.KeyDelete(internalKey);
                }

                foreach (var value in values)
                {
                    _cacheQueryProvider.ListRightPush(value.KeyGenerator(), value);
                }

                return values;
            }

            return cacheValue;
        }

        public IEnumerable<T> QueryPages<T>(Expression<Func<T, Boolean>> entity, out Int32 totalCount, Int32 pageIndex, Int32 pageSize) where T : DomainModelBase, IAggregationRoot
        {
            String internalKey = entity.GeneratorRedisKey<T>();

            Int32 internalStart = (pageIndex - 1) * pageSize, internalEnd = (pageSize + internalStart) - 1;

            var cacheValue = _cacheQueryProvider.ListRange<T>(internalKey, internalStart, internalEnd);

            totalCount = (Int32)_cacheQueryProvider.ListLength(internalKey);

            if (cacheValue == null || !cacheValue.Any())
            {
                IList<T> value = EfContext.Set<T, Int32>().Where(entity).ToList();

                if (_cacheQueryProvider.KeyExists(internalKey))
                {
                    _cacheQueryProvider.KeyDelete(internalKey);
                }

                foreach (var v in value)
                {
                    _cacheQueryProvider.ListRightPush(internalKey, v);
                }

                return value;
            }

            return cacheValue;
        }
    }
}
