using System.Threading.Tasks;

namespace Dvchevskii.Optional.Extensions
{
    public static class AsyncExtensions
    {
        public static Task<Option<T>> ResolveAsOption<T>(this Task<T> task) =>
            task.ContinueWith(t => t.IsCompleted ? Option.Some(t.Result) : Option.None<T>());
    }
}
