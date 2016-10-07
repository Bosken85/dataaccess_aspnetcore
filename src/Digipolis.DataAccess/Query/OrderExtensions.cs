using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Digipolis.DataAccess.Query
{
    public static class OrderExtensions
    {
        public static IOrderedQueryable<T> OrderByQuery<T>(this IQueryable<T> source, params string[] orderFields) where T : class
        {
            if (orderFields == null || !orderFields.Any()) return source.OrderBy<T, object>(x=> null);

            IOrderedQueryable<T> result = null;
            for (int i = 0; i < orderFields.Length; i++)
            {
                var descending = orderFields[i].StartsWith("-");
                var field = descending ? orderFields[i].Remove(0, 1) : orderFields[i];
                if (i == 0) result = descending ? source.OrderByDescending(field) : source.OrderBy(field);
                else result = descending ? result.ThenByDescending(field) : result.ThenBy(field);
            }
            MethodCallExpression expression = (result != null ? result.Expression : source.Expression) as MethodCallExpression;
            return (IOrderedQueryable<T>)source.Provider.CreateQuery<T>(expression);
        }

        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> query, string propertyname)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var prop = Expression.PropertyOrField(param, propertyname);
            var sortLambda = Expression.Lambda(prop, param);

            Expression<Func<IOrderedQueryable<T>>> sortMethod = () => query.OrderBy<T, object>(k => null);

            var methodCallExpression = (sortMethod.Body as MethodCallExpression);
            var method = methodCallExpression.Method.GetGenericMethodDefinition();
            var genericSortMethod = method.MakeGenericMethod(typeof(T), prop.Type);
            var orderedQuery = (IOrderedQueryable<T>)genericSortMethod.Invoke(query, new object[] { query, sortLambda });

            return orderedQuery;
        }

        public static IOrderedQueryable<T> OrderByDescending<T>(this IQueryable<T> query, string propertyname)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var prop = Expression.PropertyOrField(param, propertyname);
            var sortLambda = Expression.Lambda(prop, param);

            Expression<Func<IOrderedQueryable<T>>> sortMethod = () => query.OrderByDescending<T, object>(k => null);

            var methodCallExpression = (sortMethod.Body as MethodCallExpression);
            var method = methodCallExpression.Method.GetGenericMethodDefinition();
            var genericSortMethod = method.MakeGenericMethod(typeof(T), prop.Type);
            var orderedQuery = (IOrderedQueryable<T>)genericSortMethod.Invoke(query, new object[] { query, sortLambda });

            return orderedQuery;
        }

        public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> query, string propertyname)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var prop = Expression.PropertyOrField(param, propertyname);
            var sortLambda = Expression.Lambda(prop, param);

            Expression<Func<IOrderedQueryable<T>>> sortMethod = () => query.ThenBy<T, object>(k => null);

            var methodCallExpression = (sortMethod.Body as MethodCallExpression);
            var method = methodCallExpression?.Method.GetGenericMethodDefinition();
            var genericSortMethod = method?.MakeGenericMethod(typeof(T), prop.Type);
            var orderedQuery = (IOrderedQueryable<T>)genericSortMethod?.Invoke(query, new object[] { query, sortLambda });

            return orderedQuery;
        }

        public static IOrderedQueryable<T> ThenByDescending<T>(this IOrderedQueryable<T> query, string propertyname)
        {
            var param = Expression.Parameter(typeof(T), "x");
            var prop = Expression.PropertyOrField(param, propertyname);
            var sortLambda = Expression.Lambda(prop, param);

            Expression<Func<IOrderedQueryable<T>>> sortMethod = () => query.ThenByDescending<T, object>(k => null);

            var methodCallExpression = (sortMethod.Body as MethodCallExpression);
            var method = methodCallExpression.Method.GetGenericMethodDefinition();
            var genericSortMethod = method.MakeGenericMethod(typeof(T), prop.Type);
            var orderedQuery = (IOrderedQueryable<T>)genericSortMethod.Invoke(query, new object[] { query, sortLambda });

            return orderedQuery;
        }
    }
}
