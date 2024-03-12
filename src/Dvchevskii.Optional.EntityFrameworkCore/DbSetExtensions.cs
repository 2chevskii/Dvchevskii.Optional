using Dvchevskii.Optional.Async;
using Dvchevskii.Optional.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Dvchevskii.Optional.EntityFrameworkCore;

public static class DbSetExtensions
{
    public static Option<T> FindOrNone<T>(this DbSet<T> set, params object[] keyValues)
        where T : class
    {
        var entity = set.Find(keyValues);
        return entity == null ? Option.None<T>() : Option.Some(entity);
    }

    public static AsyncOption<T> FindOrNoneAsync<T>(this DbSet<T> set, params object[] keyValues)
        where T : class
    {
        return set.FindAsync(keyValues)
            .AsTask()
            .ContinueWith(task => task.Result.AsOption())
            .AsAsyncOption();
    }
}
