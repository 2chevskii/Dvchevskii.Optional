using System;
using System.Linq;
using System.Linq.Expressions;
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

        public static AsyncOption<T> FirstOrNoneAsync<T>(this IQueryable<T> queryable)
            where T : class =>
            queryable
                .FirstOrDefaultAsync()
                .ContinueWith(task => task.Result.AsOption())
                .AsAsyncOption();

        public static AsyncOption<T> FirstOrNoneAsync<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate
        )
            where T : class =>
            queryable
                .FirstOrDefaultAsync(predicate)
                .ContinueWith(task => task.Result.AsOption())
                .AsAsyncOption();

        public static AsyncOption<T> SingleOrNoneAsync<T>(this IQueryable<T> queryable)
            where T : class =>
            queryable
                .SingleOrDefaultAsync()
                .ContinueWith(task => task.Result.AsOption())
                .AsAsyncOption();

        public static AsyncOption<T> SingleOrNoneAsync<T>(
            this IQueryable<T> queryable,
            Expression<Func<T, bool>> predicate
        )
            where T : class =>
            queryable
                .SingleOrDefaultAsync(predicate)
                .ContinueWith(task => task.Result.AsOption())
                .AsAsyncOption();
    }
}
