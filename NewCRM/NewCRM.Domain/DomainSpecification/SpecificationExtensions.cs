using System;
using System.Linq;
using System.Linq.Expressions;
using NewCRM.Domain.Entities.DomainModel;
using NewCRM.Domain.Entities.DomainSpecification.ConcreteSpecification;

namespace NewCRM.Domain.Entities.DomainSpecification
{

    /// <summary>
    /// 规约模式扩展
    /// </summary>
    public static class SpecificationExtensions
    {
        /// <summary>
        /// 逻辑和
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Specification<T> And<T>(this Specification<T> left, Expression<Func<T, Boolean>> right) where T : DomainModelBase, IAggregationRoot
        {

            var internalParameter = Expression.Parameter(typeof(T), "entity");

            var parameterVister = new ParameterVisitor(internalParameter);

            var leftBody = parameterVister.Replace(left.Expression);

            var rightBody = parameterVister.Replace(right.Body);

            var newExpression = Expression.And(leftBody, rightBody);

            return new DefaultSpecification<T>(Expression.Lambda<Func<T, Boolean>>(newExpression, internalParameter));

        }

        /// <summary>
        /// 逻辑或
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Specification<T> Or<T>(this Specification<T> left, Expression<Func<T, Boolean>> right) where T : DomainModelBase, IAggregationRoot
        {
            var internalParameter = Expression.Parameter(typeof(T), "entity");

            var parameterVister = new ParameterVisitor(internalParameter);

            var leftBody = parameterVister.Replace(left.Expression.Body);

            var rightBody = parameterVister.Replace(right.Body);

            var orExpression = Expression.Or(leftBody, rightBody);

            return new DefaultSpecification<T>(Expression.Lambda<Func<T, Boolean>>(orExpression, internalParameter));
        }

        /// <summary>
        /// 逻辑非
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <returns></returns>
        public static Specification<T> Not<T>(this Specification<T> left) where T : DomainModelBase, IAggregationRoot
        {
            var internalParameter = left.Expression.Parameters[0];

            var newExpression = Expression.Not(left.Expression.Body);

            return new DefaultSpecification<T>(Expression.Lambda<Func<T, Boolean>>(newExpression, internalParameter));
        }

        /// <summary>
        /// 逻辑倒序排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static Specification<T> OrderByDescending<T>(this Specification<T> left, Expression<Func<T, dynamic>> right) where T : DomainModelBase, IAggregationRoot
        {
            left.AddOrderByExpression(right);

            return left;
        }
    }
}
