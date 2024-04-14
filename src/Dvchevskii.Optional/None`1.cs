using System;
using Dvchevskii.Optional.Exceptions;

// ReSharper disable InconsistentNaming

namespace Dvchevskii.Optional
{
    /*
     * NOTE: All Nones are equal, no matter the <T>'s type
     * This is done so because we don't have smart enums in c# =(
     */

    /// <summary>
    /// Represents an <see cref="Option"/> which does not hold a value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class None<T> : Option<T>
    {
        private static readonly None<T> Instance = new None<T>();

        public override bool IsSome => false;
        public override bool IsNone => true;

        private None() { }

        public static None<T> Create() => Instance;

        public override bool IsSomeAnd(Predicate<T> predicate) => false;

        public override Option<T> Inspect(Action<T> inspector) => this;

        public override T Expect(string message) => throw new ExpectNoneException(message);

        public override T Unwrap() => Expect("Option is none");

        public override T UnwrapOr(T defaultValue) => defaultValue;

        public override T UnwrapOrElse(Func<T> defaultValueFactory) => defaultValueFactory();

        public override T UnwrapOrDefault() => default;

        public override Option<U> Map<U>(Func<T, U> mapper) => None<U>();

        public override U MapOr<U>(Func<T, U> mapper, U defaultValue) => defaultValue;

        public override U MapOrElse<U>(Func<T, U> mapper, Func<U> defaultValueFactory) =>
            defaultValueFactory();

        public override Option<U> And<U>(Option<U> optionB) => None<U>();

        public override Option<U> AndThen<U>(Func<T, Option<U>> optionBFactory) => None<U>();

        public override Option<T> Filter(Predicate<T> predicate) => None<T>();

        public override Option<T> Or(Option<T> optionB) => optionB;

        public override Option<T> OrElse(Func<Option<T>> optionBFactory) => optionBFactory();

        public override Option<T> XOr(Option<T> optionB) => optionB;

        public override Option<(T, U)> Zip<U>(Option<U> other) => None<(T, U)>();

        public override Option<R> ZipWith<U, R>(Option<U> other, Func<T, U, R> func) => None<R>();

        public None<R> ToType<R>() => Optional.None<R>.Instance;

        public override string ToString()
        {
            return $"None<{UnderlyingType.Name}>()";
        }
    }
}
