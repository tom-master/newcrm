﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using NewCRM.Domain.DomainSpecification;
using NewCRM.Domain.Entitys;
using NewCRM.Domain.Repositories;
using NewCRM.Domain.UnitWork;
using NewCRM.Infrastructure.CommonTools.CustomException;
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

        public T Query<T>(T entity) where T : DomainModelBase, IAggregationRoot
        {
            var key = entity.KeyGenerator();

            var cacheValue = _cacheQueryProvider.StringGet<T>(key);

            if (cacheValue == null)
            {
                var value = EfContext.Set<T, Int32>().FirstOrDefault(t => t.Id == entity.Id);

                if (_cacheQueryProvider.KeyExists(key))
                {
                    _cacheQueryProvider.KeyDelete(key);
                }

                _cacheQueryProvider.StringSet(key, value);

                return value;
            }

            return cacheValue;
        }

        public IEnumerable<T> Querys<T>(T entity) where T : DomainModelBase, IAggregationRoot
        {
            var key = entity.KeyGenerator();

            var cacheValue = _cacheQueryProvider.ListRange<T>(key);

            if (cacheValue == null|| !cacheValue.Any())
            {
                var value = EfContext.Set<T, Int32>().Where(t => true).ToList();

                if (_cacheQueryProvider.KeyExists(key))
                {
                    _cacheQueryProvider.KeyDelete(key);
                }

                foreach (var v in value)
                {
                    _cacheQueryProvider.ListRightPush(key, v);
                }

                return value;
            }

            return cacheValue;
        }
    }
}
