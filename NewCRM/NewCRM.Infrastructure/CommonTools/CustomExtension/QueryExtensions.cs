using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NewCRM.Infrastructure.CommonTools.CustomExtension
{
    public static class QueryExtensions
    {

        public static IQueryable<T> PageBy<T>(this IQueryable<T> source, Int32 pageIndex, Int32 pageSize, Expression<Func<PropertySortCondition>> sort = default(Expression<Func<PropertySortCondition>>))
        {
            if (source == null)
            {
                throw new ArgumentNullException($"{nameof(source)}不能为空");
            }

            return sort == default(Expression<Func<PropertySortCondition>>) ? source.OrderBy("Id", false).Skip((pageIndex - 1) * pageSize).Take(pageSize) : source.OrderBy(sort.Compile()().PropertyName, true).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, String propertyName)
        {
            return QueryableHelper<T>.OrderBy(queryable, propertyName, false);
        }
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> queryable, String propertyName, Boolean desc)
        {
            return QueryableHelper<T>.OrderBy(queryable, propertyName, desc);
        }


        static class QueryableHelper<T>
        {
            private static Dictionary<String, LambdaExpression> cache = new Dictionary<String, LambdaExpression>();

            public static IQueryable<T> OrderBy(IQueryable<T> queryable, String propertyName, Boolean desc)
            {
                dynamic keySelector = GetLambdaExpression(propertyName);

                return desc ? Queryable.OrderByDescending(queryable, keySelector) : Queryable.OrderBy(queryable, keySelector);
            }

            private static LambdaExpression GetLambdaExpression(String propertyName)
            {
                if (cache.ContainsKey(propertyName))
                {
                    return cache[propertyName];
                }

                var param = Expression.Parameter(typeof(T));

                var body = Expression.Property(param, propertyName);

                var keySelector = Expression.Lambda(body, param);

                cache[propertyName] = keySelector;

                return keySelector;
            }
        }
    }
}
