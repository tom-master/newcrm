using System;
using System.Linq;
using System.Linq.Expressions;

namespace NewCRM.Infrastructure.CommonTools.CustomExtension
{
    public static class QueryExtensions
    {
        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, String sortExpression)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            String sortDirection;
            String sropertyName;

            sortExpression = sortExpression.Trim();
            var spaceIndex = sortExpression.Trim().IndexOf(" ", StringComparison.Ordinal);
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

            var parameter = Expression.Parameter(source.ElementType, String.Empty);
            var property = Expression.Property(parameter, sropertyName);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = (sortDirection == "ASC") ? "OrderBy" : "OrderByDescending";

            Expression methodCallExpression = Expression.Call(typeof(Queryable), methodName,
                                                new[] { source.ElementType, property.Type },
                                                source.Expression, Expression.Quote(lambda));

            return source.Provider.CreateQuery<T>(methodCallExpression);
        }
    }
}
