using System;
using System.Collections.Generic;
using System.Linq;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Factory.DomainSpecification;
using NewCRM.Domain.Repositories;
using NewCRM.Domain.UnitWork;
using NewCRM.Infrastructure.CommonTools.CustomException;
using NewCRM.Infrastructure.CommonTools.CustomExtension;
using NewCRM.Repository.DataBaseProvider.Redis.InternalHelper;
using Unity.Attributes;

namespace NewCRM.Repository.DataBaseProvider.Redis
{
    public class QueryProviderFormCache : IDomainModelQueryProviderFormCache
    {
        [Dependency("ICacheQueryProvider")]
        public ICacheQueryProvider CacheQueryProvider { get; set; }

        #region 属性

        /// <summary>
        /// 获取 仓储上下文的实例
        /// </summary>
        [Dependency]
        protected IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        ///获取 EntityFramework的数据仓储上下文
        /// </summary>
        private UnitOfWorkContextBase EfContext
        {
            get
            {
                if(UnitOfWork is UnitOfWorkContextBase)
                {
                    return UnitOfWork as UnitOfWorkContextBase;
                }

                throw new RepositoryException($"无法获取当前工作单元的实例:{nameof(UnitOfWork)}");
            }
        }

        #endregion

        public IEnumerable<T> Query<T>(Specification<T> entity) where T : DomainModelBase, IAggregationRoot
        {
            String internalKey = entity.Expression.GeneratorRedisKey<T>();

            var cacheValue = CacheQueryProvider.ListRange<T>(internalKey);

            if(cacheValue == null || !cacheValue.Any())
            {
                IList<T> values = EfContext.Set<T, Int32>().Where(entity.Expression).ToList();
                if(CacheQueryProvider.KeyExists(internalKey))
                {
                    CacheQueryProvider.KeyDelete(internalKey);
                }

                foreach(var value in values)
                {
                    CacheQueryProvider.ListRightPush(value.KeyGenerator(), value);
                }

                return values;
            }

            return cacheValue;
        }

        public IEnumerable<T> QueryPage<T>(Specification<T> entity, out Int32 totalCount, Int32 pageIndex, Int32 pageSize) where T : DomainModelBase, IAggregationRoot
        {
            throw new NotSupportedException();
        }

    }
}
