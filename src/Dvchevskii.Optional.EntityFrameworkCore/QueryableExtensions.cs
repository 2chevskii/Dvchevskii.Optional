using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dvchevskii.Optional.Async;
using Dvchevskii.Optional.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Optional.EntityFrameworkCore
{
    public static class QueryableExtensions
    {
        public static Option<T> FirstOrNone<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate
        )
            where T : class => queryable.FirstOrDefault(predicate).AsOption();

        public static Option<T> FirstOrNone<T>(this IQueryable<T> queryable)
            where T : class => queryable.FirstOrDefault().AsOption();

        public static Option<T> SingleOrNone<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate
        )
            where T : class => queryable.SingleOrDefault(predicate).AsOption();

        public static Option<T> SingleOrNone<T>(this IQueryable<T> queryable)
            where T : class => queryable.SingleOrDefault().AsOption();

        public static Task<Option<T>> FirstOrNoneAsync<T>(this IQueryable<T> queryable)
            where T : class => queryable.FirstOrDefaultAsync().AsOptionAsync();

        public static Task<Option<T>> FirstOrNoneAsync<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate
        )
            where T : class => queryable.FirstOrDefaultAsync(predicate).AsOptionAsync();

        public static Task<Option<T>> SingleOrNoneAsync<T>(this IQueryable<T> queryable)
            where T : class => queryable.SingleOrDefaultAsync().AsOptionAsync();

        public static Task<Option<T>> SingleOrNoneAsync<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate
        )
            where T : class => queryable.SingleOrDefaultAsync(predicate).AsOptionAsync();
    }
}
