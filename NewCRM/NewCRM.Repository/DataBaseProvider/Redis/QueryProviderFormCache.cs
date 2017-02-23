using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Factory.DomainSpecification;
using NewCRM.Domain.Repositories;
using NewCRM.Domain.UnitWork;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.Repository.DataBaseProvider.Redis.InternalHelper;
using NewCRM.Repository.UnitOfWorkProvide;

namespace NewCRM.Repository.DataBaseProvider.Redis
{
    [Export(typeof(IDomainModelQueryProviderFormCache))]
    internal class QueryProviderFormCache : IDomainModelQueryProviderFormCache
    {
        private readonly ICacheQueryProvider _cacheQueryProvider;

        #region 属性

        /// <summary>
        /// 获取 仓储上下文的实例
        /// </summary>
        [Import]
        protected IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        ///获取 EntityFramework的数据仓储上下文
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

        [ImportingConstructor]
        public QueryProviderFormCache(ICacheQueryProvider cacheQueryProvider)
        {
            _cacheQueryProvider = cacheQueryProvider;
        }

        public IEnumerable<T> Query<T>(Specification<T> entity) where T : DomainModelBase, IAggregationRoot
        {
            String internalKey = entity.Expression.GeneratorRedisKey<T>();

            var cacheValue = _cacheQueryProvider.ListRange<T>(internalKey);

            if (cacheValue == null || !cacheValue.Any())
            {
                IList<T> values = EfContext.Set<T, Int32>().Where(entity.Expression).ToList();

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

        public IEnumerable<T> QueryPage<T>(Specification<T> entity, out Int32 totalCount, Int32 pageIndex, Int32 pageSize) where T : DomainModelBase, IAggregationRoot
        {
            throw new NotSupportedException();

            //String internalKey = entity.Expression.GeneratorRedisKey<T>();

            //Int32 internalStart = (pageIndex - 1) * pageSize, internalEnd = (pageSize + internalStart) - 1;

            //var cacheValue = _cacheQueryProvider.ListRange<T>(internalKey, internalStart, internalEnd);

            //totalCount = (Int32)_cacheQueryProvider.ListLength(internalKey);

            //if (cacheValue == null || !cacheValue.Any())
            //{
            //    IList<T> value = EfContext.Set<T, Int32>().Where(entity.Expression).ToList();

            //    if (_cacheQueryProvider.KeyExists(internalKey))
            //    {
            //        _cacheQueryProvider.KeyDelete(internalKey);
            //    }

            //    foreach (var v in value)
            //    {
            //        _cacheQueryProvider.ListRightPush(internalKey, v);
            //    }

            //    return value;
            //}

            //return cacheValue;
        }

    }
}
