using System;
using Dvchevskii.Optional.Exceptions;

namespace Dvchevskii.Optional.None
{
    /*
     * NOTE: All Nones are equal, no matter the <T>'s type
     * This is done so because we don't have smart enums in c# =(
     */
    internal sealed class None<T> : Option<T>, INone
    {
        private static readonly None<T> Instance = new None<T>();

        private None() { }

        public static None<T> Create() => Instance;

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

        public override U MapOr<U>(Func<T, U> mapper, U defaultValue) => defaultValue;

        public override U MapOrElse<U>(Func<T, U> mapper, Func<U> defaultValueFactory) =>
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

        public None<R> ToType<R>() => global::Dvchevskii.Optional.None.None<R>.Instance;

        public override int GetHashCode() => unchecked(typeof(None<>).GetHashCode() * 31);

        public override bool Equals(Option other)
        {
            if (other is null)
            {
                return false;
            }

            if (other.IsNone())
            {
                return true;
            }

            return false;
        }
    }
}
