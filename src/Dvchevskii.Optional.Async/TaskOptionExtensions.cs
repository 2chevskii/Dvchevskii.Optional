using System;
using System.Threading.Tasks;
using Dvchevskii.Optional.Exceptions;

// ReSharper disable InconsistentNaming

namespace Dvchevskii.Optional.Async
{
    /// <summary>
    /// This class contains a bunch of obscure method chains which make Task&lt;Option&lt;T&gt;&gt; work
    /// </summary>
    public static class TaskOptionExtensions
    {
        public static Task<bool> IsSomeAsync(this Task<Option> self) =>
            self.ContinueWith(
                task => task.Status == TaskStatus.RanToCompletion && task.Result.IsSome
            );

        public static Task<bool> IsSomeAsync<T>(this Task<Option<T>> self) =>
            self.MapOrAsync(_ => true, false);

        public static Task<bool> IsNoneAsync(this Task<Option> self) =>
            self.ContinueWith(
                task => task.Status != TaskStatus.RanToCompletion || task.Result.IsNone
            );

        public static Task<bool> IsNoneAsync<T>(this Task<Option<T>> self) =>
            self.MapOrAsync(_ => false, true);

        public static Task<T> UnwrapAsync<T>(this Task<Option<T>> self) =>
            self.ContinueWith(
                task =>
                    task.Status == TaskStatus.RanToCompletion
                        ? task.Result.Unwrap()
                        : Option.None<T>().Unwrap()
            );

        public static Task<T> UnwrapOrAsync<T>(this Task<Option<T>> self, T defaultValue) =>
            self.ContinueWith(
                task =>
                    task.Status == TaskStatus.RanToCompletion
                        ? task.Result.UnwrapOr(defaultValue)
                        : Option.None<T>().UnwrapOr(defaultValue)
            );

#if NULLABLE
        public static Task<T?> UnwrapOrDefaultAsync<T>(this Task<Option<T>> self) =>
#else
        public static Task<T> UnwrapOrDefaultAsync<T>(this Task<Option<T>> self) =>
#endif
            self.ContinueWith(
                task =>
                    task.Status == TaskStatus.RanToCompletion
                        ? task.Result.UnwrapOrDefault()
                        : Option.None<T>().UnwrapOrDefault()
            );

        public static Task<T> UnwrapOrElseAsync<T>(
            this Task<Option<T>> self,
            Func<T> defaultValueFactory
        ) =>
            self.ContinueWith(
                task =>
                    task.Status == TaskStatus.RanToCompletion
                        ? task.Result.UnwrapOrElse(defaultValueFactory)
                        : Option.None<T>().UnwrapOrElse(defaultValueFactory)
            );

        public static Task<T> UnwrapOrElseAsync<T>(
            this Task<Option<T>> self,
            Func<Task<T>> defaultValueFactory
        ) =>
            self.ContinueWith(
                    task =>
                        task.Status == TaskStatus.RanToCompletion
                            ? task.Result.Map(Task.FromResult).UnwrapOrElse(defaultValueFactory)
                            : Option.None<Task<T>>().UnwrapOrElse(defaultValueFactory)
                )
                .Unwrap();

        public static Task<T> ExpectAsync<T>(this Task<Option<T>> self, string message) =>
            self.ContinueWith(
                task =>
                    task.Status != TaskStatus.RanToCompletion
                        ? throw new OptionNoneException(message)
                        : task.Result.Expect(message)
            );

        public static Task<Option<U>> MapAsync<T, U>(
            this Task<Option<T>> self,
            Func<T, U> mapper
        ) =>
            self.ContinueWith(
                task =>
                    task.Status == TaskStatus.RanToCompletion
                        ? task.Result.Map(mapper)
                        : Option.None<T>().Map(mapper)
            );

        public static Task<Option<U>> MapAsync<T, U>(
            this Task<Option<T>> self,
            Func<T, Task<U>> mapper
        ) =>
            self.ContinueWith(async task =>
                {
                    if (task.Status == TaskStatus.RanToCompletion)
                    {
                        var v = task.Result;
                        var mapRes = v.Map(mapper);
                        if (mapRes.IsSome)
                        {
                            return Option.Some(await mapRes.Unwrap());
                        }
                    }

                    return Option.None<U>();
                })
                .Unwrap();

        public static Task<U> MapOrAsync<T, U>(
            this Task<Option<T>> self,
            Func<T, U> mapper,
            U defaultValue
        ) =>
            self.ContinueWith(
                task =>
                    task.Status == TaskStatus.RanToCompletion
                        ? task.Result.MapOr(mapper, defaultValue)
                        : defaultValue
            );

        public static Task<U> MapOrAsync<T, U>(
            this Task<Option<T>> self,
            Func<T, Task<U>> mapper,
            U defaultValue
        ) =>
            self.ContinueWith(
                    task =>
                        task.Status == TaskStatus.RanToCompletion
                            ? task.Result.MapOr(mapper, Task.FromResult(defaultValue))
                            : Task.FromResult(defaultValue)
                )
                .Unwrap();

        public static Task<U> MapOrElseAsync<T, U>(
            this Task<Option<T>> self,
            Func<T, U> mapper,
            Func<U> defaultValueFactory
        ) =>
            self.ContinueWith(
                task =>
                    task.Status == TaskStatus.RanToCompletion
                        ? task.Result.MapOrElse(mapper, defaultValueFactory)
                        : defaultValueFactory()
            );

        public static Task<U> MapOrElseAsync<T, U>(
            this Task<Option<T>> self,
            Func<T, Task<U>> mapper,
            Func<U> defaultValueFactory
        ) =>
            self.ContinueWith(
                    task =>
                        task.Status == TaskStatus.RanToCompletion
                            ? task.Result.MapOrElse(
                                mapper,
                                () => Task.FromResult(defaultValueFactory())
                            )
                            : Task.FromResult(defaultValueFactory())
                )
                .Unwrap();

        public static Task<U> MapOrElseAsync<T, U>(
            this Task<Option<T>> self,
            Func<T, U> mapper,
            Func<Task<U>> defaultValueFactory
        ) =>
            self.ContinueWith(
                    task =>
                        task.Status == TaskStatus.RanToCompletion
                            ? task.Result.MapOrElse(
                                v => Task.FromResult(mapper(v)),
                                defaultValueFactory
                            )
                            : defaultValueFactory()
                )
                .Unwrap();

        public static Task<U> MapOrElseAsync<T, U>(
            this Task<Option<T>> self,
            Func<T, Task<U>> mapper,
            Func<Task<U>> defaultValueFactory
        ) =>
            self.ContinueWith(
                    task =>
                        task.Status == TaskStatus.RanToCompletion
                            ? task.Result.MapOrElse(mapper, defaultValueFactory)
                            : Option.None<T>().MapOrElse(mapper, defaultValueFactory)
                )
                .Unwrap();
    }
}
