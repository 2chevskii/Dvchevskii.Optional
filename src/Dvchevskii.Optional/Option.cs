using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Dvchevskii.Optional.Async", AllInternalsVisible = true)]

namespace Dvchevskii.Optional
{
    public abstract class Option : IEquatable<Option>
    {
        public abstract bool IsSome { get; }
        public abstract bool IsNone { get; }
        public abstract Type UnderlyingType { get; }

        private protected Option() { }

        public static bool operator ==(Option lhs, Option rhs) =>
            OptionEqualityComparer.Default.Equals(lhs, rhs);

        public static bool operator !=(Option lhs, Option rhs) =>
            !OptionEqualityComparer.Default.Equals(lhs, rhs);

        /// <summary>
        /// Create an Option which does not contain a value
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <returns></returns>
        public static Option<T> None<T>() => Optional.None<T>.Create();

        /// <summary>
        /// Create an Option which holds a provided <paramref name="value"/>
        /// </summary>
        /// <param name="value">Value which will be held in the resulting Option</param>
        /// <typeparam name="T">Value type</typeparam>
        /// <returns></returns>
        public static Option<T> Some<T>(T value) => Optional.Some<T>.From(value);

        internal abstract object UnwrapAny();

        public virtual bool Equals(Option other) =>
            OptionEqualityComparer.Default.Equals(this, other);

        public override bool Equals(object obj) => OptionEqualityComparer.Default.Equals(this, obj);

        public override int GetHashCode() => OptionEqualityComparer.Default.GetHashCode(this);
    }
}
