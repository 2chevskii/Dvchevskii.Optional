using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Dvchevskii.Optional.Exceptions;

// ReSharper disable InconsistentNaming
namespace Dvchevskii.Optional
{
    public abstract class Option<T> : Option, IEquatable<Option<T>>, IEquatable<T>
    {
        public override Type UnderlyingType => typeof(T);

        private protected Option() { }

        public static bool operator ==(Option<T> lhs, Option<T> rhs) =>
            OptionEqualityComparer<T>.Default.Equals(lhs, rhs);

        public static bool operator !=(Option<T> lhs, Option<T> rhs) =>
            !OptionEqualityComparer<T>.Default.Equals(lhs, rhs);

        public static bool operator ==(Option<T> lhs, T rhs) =>
            OptionEqualityComparer<T>.Default.Equals(lhs, rhs);

        public static bool operator !=(Option<T> lhs, T rhs) =>
            !OptionEqualityComparer<T>.Default.Equals(lhs, rhs);

        public static bool operator ==(T lhs, Option<T> rhs) =>
            OptionEqualityComparer<T>.Default.Equals(rhs, lhs);

        public static bool operator !=(T lhs, Option<T> rhs) =>
            !OptionEqualityComparer<T>.Default.Equals(rhs, lhs);

        /// <summary>
        /// Check if <see cref="Option{T}"/> contains a value and it matches the <paramref name="predicate"/>
        /// </summary>
        /// <param name="predicate">A predicate to match</param>
        /// <returns></returns>
        public abstract bool IsSomeAnd(Predicate<T> predicate);

        /// <summary>
        /// Runs a given function on a value if option is Some
        /// </summary>
        /// <param name="inspector">Function to run on a value</param>
        /// <returns></returns>
        public abstract Option<T> Inspect(Action<T> inspector);

        /// <summary>
        /// Unwraps a value if option is Some, or throws a <see cref="ExpectNoneException"/> with given message
        /// </summary>
        /// <param name="message"></param>
        /// <exception cref="ExpectNoneException">Option has no value</exception>
        /// <returns></returns>
        public abstract T Expect(string message);

        /// <summary>
        /// Unwraps a value out of the <see cref="Option{T}"/>
        /// </summary>
        /// <returns></returns>
        public abstract T Unwrap();

        /// <summary>
        /// Unwraps a value or returns a given default
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public abstract T UnwrapOr(T defaultValue);

        public abstract T UnwrapOrElse(Func<T> defaultValueFactory);

        public abstract T UnwrapOrDefault();
        public abstract Option<U> Map<U>(Func<T, U> mapper);

        public abstract U MapOr<U>(Func<T, U> mapper, U defaultValue);

        public abstract U MapOrElse<U>(Func<T, U> mapper, Func<U> defaultValueFactory);

        public abstract Option<U> And<U>(Option<U> optionB);

        public abstract Option<U> AndThen<U>(Func<T, Option<U>> optionBFactory);

        public abstract Option<T> Filter(Predicate<T> predicate);

        public abstract Option<T> Or(Option<T> optionB);

        public abstract Option<T> OrElse(Func<Option<T>> optionBFactory);

        public abstract Option<T> XOr(Option<T> optionB);

        public abstract Option<(T, U)> Zip<U>(Option<U> other);

        public abstract Option<R> ZipWith<U, R>(Option<U> other, Func<T, U, R> func);

        public virtual bool Equals(Option<T> other) => OptionEqualityComparer<T>.Default.Equals(this, other);

        public virtual bool Equals(T value) => OptionEqualityComparer<T>.Default.Equals(this, value);

        public override bool Equals(object obj) => OptionEqualityComparer<T>.Default.Equals(this, obj);

        public override bool Equals(Option other) => OptionEqualityComparer<T>.Default.Equals(this, other);

        public override int GetHashCode() => OptionEqualityComparer<T>.Default.GetHashCode(this);

        internal override object UnwrapAny() => Unwrap();
    }
}
