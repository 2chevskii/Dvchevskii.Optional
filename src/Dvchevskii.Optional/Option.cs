using System;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Dvchevskii.Optional.Async", AllInternalsVisible = true)]

namespace Dvchevskii.Optional
{
    public abstract partial class Option : IEquatable<Option>
    {
        public abstract bool IsSome { get; }
        public abstract bool IsNone { get; }
        public abstract Type UnderlyingType { get; }

        private protected Option() { }

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

        public abstract bool Equals(Option other);

        public abstract object UnwrapAny();
    }
}
