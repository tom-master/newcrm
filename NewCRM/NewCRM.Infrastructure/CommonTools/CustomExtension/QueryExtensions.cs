using System;
using System.Linq;
using System.Linq.Expressions;

namespace NewCRM.Infrastructure.CommonTools.CustomExtension
{
    public static class QueryExtensions
    {
        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, string sortExpression)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            String sortDirection;
            String sropertyName;

            sortExpression = sortExpression.Trim();
            int spaceIndex = sortExpression.Trim().IndexOf(" ", StringComparison.Ordinal);
            if (spaceIndex < 0)
            {
                sropertyName = sortExpression;
                sortDirection = "ASC";
            }
            else
            {
                sropertyName = sortExpression.Substring(0, spaceIndex);
                sortDirection = sortExpression.Substring(spaceIndex + 1).Trim();
            }

            if (String.IsNullOrEmpty(sropertyName))
            {
                return source;
            }

            ParameterExpression parameter = Expression.Parameter(source.ElementType, String.Empty);
            MemberExpression property = Expression.Property(parameter, sropertyName);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            string methodName = (sortDirection == "ASC") ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                                new[] { source.ElementType, property.Type },
                                                source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }
    }
}
