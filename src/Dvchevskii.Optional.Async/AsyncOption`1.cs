using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Dvchevskii.Optional.Extensions;

namespace Dvchevskii.Optional.Async
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class AsyncOption<T> : AsyncOption
    {
        private readonly Task<Option<T>> _task;

        public AsyncOption(Task<Option<T>> task)
            : base(task.ContinueWith(x => (Option)x.Result))
        {
            _task = task;
        }

        public new TaskAwaiter<Option<T>> GetAwaiter() => _task.GetAwaiter();

        public AsyncOption<U> MapAsync<U>(Func<T, U> mapper) =>
            _task.ContinueWith(t => t.Result.Map(mapper)).AsAsyncOption();

        public AsyncOption<U> MapAsync<U>(Func<T, Task<U>> mapper) =>
            _task
                .ContinueWith(
                    task1 =>
                        task1.Result.MapOr(
                            v => mapper(v).ContinueWith(task2 => task2.Result.AsSome()),
                            Task.FromResult(Option.None<U>())
                        )
                )
                .Unwrap()
                .AsAsyncOption();

        public Task<U> MapOrAsync<U>(Func<T, U> mapper, U defaultValue) =>
            _task.ContinueWith(task1 => task1.Result.MapOr(mapper, defaultValue));

        public Task<U> MapOrAsync<U>(Func<T, Task<U>> mapper, U defaultValue) =>
            _task
                .ContinueWith(task1 => task1.Result.MapOr(mapper, Task.FromResult(defaultValue)))
                .Unwrap();

        public Task<U> MapOrElseAsync<U>(Func<T, U> mapper, Func<U> defaultValueFactory) =>
            _task.ContinueWith(task1 => task1.Result.MapOrElse(mapper, defaultValueFactory));

        public Task<U> MapOrElseAsync<U>(Func<T, Task<U>> mapper, Func<U> defaultValueFactory) =>
            _task
                .ContinueWith(
                    task1 =>
                        task1.Result.MapOrElse(mapper, () => Task.FromResult(defaultValueFactory()))
                )
                .Unwrap();

        public Task<U> MapOrElseAsync<U>(
            Func<T, Task<U>> mapper,
            Func<Task<U>> defaultValueFactory
        ) =>
            _task
                .ContinueWith(task1 => task1.Result.MapOrElse(mapper, defaultValueFactory))
                .Unwrap();
    }
}
