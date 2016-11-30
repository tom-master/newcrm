using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace NewCRM.Infrastructure.CommonTools.CustomExtension
{
    public static class QueryExtensions
    {

        public static IQueryable<T> PageBy<T>(this IQueryable<T> source, Int32 pageIndex, Int32 pageSize, Expression<Func<T, Object>> sort = default(Expression<Func<T, Object>>))
        {
            if (source == null)
            {
                throw new ArgumentNullException($"{nameof(source)}不能为空");
            }

            return sort == default(Expression<Func<T, Object>>) ? source.Skip((pageIndex - 1) * pageSize).Take(pageSize) : source.OrderByDesc(sort).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public static IOrderedQueryable<TSource> OrderByDesc<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, Object>> keySelector)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }

            Expression body = keySelector.Body;

            if (body.NodeType == ExpressionType.Convert)
            {
                body = ((UnaryExpression)body).Operand;
            }

            LambdaExpression keySelector2 = Expression.Lambda(body, keySelector.Parameters);
            Type tkey = keySelector2.ReturnType;

            MethodInfo orderbyMethod = (from x in typeof(Queryable).GetMethods()
                                        where x.Name == "OrderByDescending"
                                        let parameters = x.GetParameters()
                                        where parameters.Length == 2
                                        let generics = x.GetGenericArguments()
                                        where generics.Length == 2
                                        where parameters[0].ParameterType == typeof(IQueryable<>).MakeGenericType(generics[0]) &&
                                            parameters[1].ParameterType == typeof(Expression<>).MakeGenericType(typeof(Func<,>).MakeGenericType(generics[0], generics[1]))
                                        select x).Single();

            return (IOrderedQueryable<TSource>)source.Provider.CreateQuery<TSource>(Expression.Call(null, orderbyMethod.MakeGenericMethod(typeof(TSource), tkey), new[]
            {
            source.Expression,
            Expression.Quote(keySelector2)
            }));
        }
    }
}
