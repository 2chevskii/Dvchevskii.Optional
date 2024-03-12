using System.Threading.Tasks;

namespace Dvchevskii.Optional.Async
{
    public static class TaskExtensions
    {
        public static AsyncOption<T> AsAsyncOption<T>(this Task<Option<T>> task)
        {
            return new AsyncOption<T>(task);
        }
    }
}
