using System;
using Option.Exceptions;

namespace Option
{
    public sealed class None<T> : Option<T>
    {
        public override bool IsNone() => true;

        public override bool IsSome() => false;

        public override bool IsSomeAnd(Predicate<T> predicate) => false;

        public override Option<T> Inspect(Action<T> inspector) => this;

        public override T Expect(string message) => throw new ExpectNoneException(message);

        public override T Unwrap() => Expect("Option is none");

        public override T UnwrapOr(T defaultValue) => defaultValue;

        public override T UnwrapOrElse(Func<T> defaultValueFactory) => defaultValueFactory();

        public override T UnwrapOrDefault() => default;

        public override Option<U> Map<U>(Func<T, U> mapper) => Option.None<U>();

        public override U MapOr<U>(U defaultValue, Func<T, U> mapper) => defaultValue;

        public override U MapOrElse<U>(Func<U> defaultValueFactory, Func<T, U> mapper) =>
            defaultValueFactory();

        public override Option<U> And<U>(Option<U> optionB) => Option.None<U>();

        public override Option<U> AndThen<U>(Func<T, Option<U>> optionBFactory) => Option.None<U>();

        public override Option<T> Filter(Predicate<T> predicate) => Option.None<T>();

        public override Option<T> Or(Option<T> optionB) => optionB;

        public override Option<T> OrElse(Func<Option<T>> optionBFactory) => optionBFactory();

        public override Option<T> XOr(Option<T> optionB) => optionB;

        public override Option<(T, U)> Zip<U>(Option<U> other)
        {
            return Option.None<(T, U)>();
        }

        public override Option<R> ZipWith<U, R>(Option<U> other, Func<T, U, R> func)
        {
            return Option.None<R>();
        }
    }
}
