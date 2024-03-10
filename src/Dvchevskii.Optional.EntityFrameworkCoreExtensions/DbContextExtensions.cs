using System;
using Dvchevskii.Optional.Async;
using Dvchevskii.Optional.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Optional.EntityFrameworkCoreExtensions;

public static class DbContextExtensions
{
    public static Option<T> FindOrNone<T>(this DbContext dbContext, params object[] keyValues)
        where T : class
    {
        var entity = dbContext.Find<T>(keyValues);
        return entity == null ? Option.None<T>() : Option.Some(entity);
    }

    public static Option FindOrNone(
        this DbContext dbContext,
        Type entityType,
        params object[] keyValues
    )
    {
        var entity = dbContext.Find(entityType, keyValues);
        return entity == null ? Option.None<object>() : Option.Some(entity);
    }

    public static AsyncOption<T> FindOrNoneAsync<T>(
        this DbContext dbContext,
        params object[] keyValues
    )
        where T : class =>
        dbContext
            .FindAsync<T>(keyValues)
            .AsTask()
            .ContinueWith(task => task.Result.AsOption())
            .AsAsyncOption();

    public static AsyncOption FindOrNoneAsync(
        this DbContext dbContext,
        Type entityType,
        params object[] keyValues
    ) =>
        dbContext
            .FindAsync(entityType, keyValues)
            .AsTask()
            .ContinueWith(task => task.Result.AsOption())
            .AsAsyncOption();
}
