using System.Runtime.CompilerServices;
using System.Threading.Tasks;

// ReSharper disable InconsistentNaming

namespace Dvchevskii.Optional.Async
{
    public abstract class AsyncOption
    {
        private readonly Task<Option> _task;

        public AsyncOption(Task<Option> task)
        {
            _task = task;
        }

        public TaskAwaiter<Option> GetAwaiter()
        {
            return _task.GetAwaiter();
        }
    }
}
