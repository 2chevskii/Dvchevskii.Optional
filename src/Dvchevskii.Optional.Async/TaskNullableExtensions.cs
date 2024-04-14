using System.Threading.Tasks;
using Dvchevskii.Optional.Extensions;

namespace Dvchevskii.Optional.Async
{
    public static class TaskNullableExtensions
    {
        public static Task<Option<T>> AsOptionAsync<T>(this Task<T?> self)
            where T : struct =>
            self.ContinueWith(
                task =>
                    task.Status == TaskStatus.RanToCompletion
                        ? task.Result.AsOption()
                        : Option.None<T>()
            );

        public static Task<Option<T>> AsOptionAsync<T>(this Task<T> self)
            where T : class =>
            self.ContinueWith(
                task =>
                    task.Status == TaskStatus.RanToCompletion
                        ? task.Result.AsOption()
                        : Option.None<T>()
            );
    }
}
