using System;

namespace Dvchevskii.Optional
{
    public abstract class Option : IEquatable<Option>
    {
        protected internal Option() { }

        /// <summary>
        /// Create an Option which does not contain a value
        /// </summary>
        /// <typeparam name="T">Value type</typeparam>
        /// <returns></returns>
        public static Option<T> None<T>() => Optional.None.None<T>.Create();

        /// <summary>
        /// Create an Option which holds a provided <paramref name="value"/>
        /// </summary>
        /// <param name="value">Value which will be held in the resulting Option</param>
        /// <typeparam name="T">Value type</typeparam>
        /// <returns></returns>
        public static Option<T> Some<T>(T value) => Optional.Some.Some<T>.From(value);

        public abstract Type GetUnderlyingType();
        public abstract bool IsSome();
        public abstract bool IsNone();

        public abstract bool Equals(Option other);
    }
}
