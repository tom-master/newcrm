using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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

        public T Query<T>(Expression<Func<T, Boolean>> entity) where T : DomainModelBase, IAggregationRoot
        {
            var key = BuilderRedisKey(entity);

            var cacheValue = _cacheQueryProvider.StringGet<T>(key);

            if (cacheValue == null)
            {
                var value = EfContext.Set<T, Int32>().FirstOrDefault(entity);

                if (_cacheQueryProvider.KeyExists(key))
                {
                    _cacheQueryProvider.KeyDelete(key);
                }

                _cacheQueryProvider.StringSet(key, value);

                return value;
            }

            return cacheValue;
        }

        public IEnumerable<T> Querys<T>(Expression<Func<T, Boolean>> entity) where T : DomainModelBase, IAggregationRoot
        {
            var key = BuilderRedisKey(entity);

            var cacheValue = _cacheQueryProvider.ListRange<T>(key);

            if (cacheValue == null || !cacheValue.Any())
            {
                IList<T> value = null;

                value = EfContext.Set<T, Int32>().Where(entity).ToList();

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


        private static String BuilderRedisKey<T>(Expression<Func<T, Boolean>> expression)
        {
            var binaryExpression = (BinaryExpression)expression.Body;

            var leftMember = (MemberExpression)binaryExpression.Left;

            var leftMemberName = leftMember.Member.Name;

            var rightMember = (MemberExpression)binaryExpression.Right;

            Expression rightMemberExpression = rightMember.Expression;

            Object returnValue;

            if (rightMemberExpression is MemberExpression)
            {
                var internalRightMemberExpression = (MemberExpression)rightMemberExpression;

                var field = internalRightMemberExpression.Type.GetProperty(rightMember.Member.Name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

                returnValue = field.GetValue(((ConstantExpression)internalRightMemberExpression.Expression).Value);
            }
            else
            {
                var internalRightMemberExpression = (ConstantExpression)rightMemberExpression;

                var field = internalRightMemberExpression.Type.GetProperty(rightMember.Member.Name,BindingFlags.Instance|BindingFlags.NonPublic);

                returnValue = field.GetValue(internalRightMemberExpression.Value);
            }

            return $"NewCRM:{typeof(T).Name}:{returnValue}";
        }
    }
}
