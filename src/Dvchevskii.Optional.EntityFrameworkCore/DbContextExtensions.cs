using System;
using System.Threading.Tasks;
using Dvchevskii.Optional.Async;
using Dvchevskii.Optional.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Optional.EntityFrameworkCore;

public static class DbContextExtensions
{
    public static Option<T> FindOrNone<T>(this DbContext dbContext, params object[] keyValues)
        where T : class => dbContext.Find<T>(keyValues).AsOption();

    public static Option FindOrNone(
        this DbContext dbContext,
        Type entityType,
        params object[] keyValues
    ) => dbContext.Find(entityType, keyValues).AsOption();

    public static Task<Option<T>> FindOrNoneAsync<T>(
        this DbContext dbContext,
        params object[] keyValues
    )
        where T : class => dbContext.FindAsync<T>(keyValues).AsTask().AsOptionAsync();

    public static Task<Option> FindOrNoneAsync(
        this DbContext dbContext,
        Type entityType,
        params object[] keyValues
    ) =>
        dbContext
            .FindAsync(entityType, keyValues)
            .AsTask()
            .ContinueWith<Option>(task => task.Result.AsOption());
}
