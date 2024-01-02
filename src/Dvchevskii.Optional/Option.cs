using System;
using Dvchevskii.Optional.Factories;

namespace Dvchevskii.Optional
{
    public abstract class Option : IEquatable<Option>
    {
        protected internal Option() { }

        public static Option<T> None<T>() => Optional.None.None<T>.Create();

        public static Option<T> Some<T>(T value) => Optional.Some.Some<T>.From(value);

        public static Option<T> Create<T>(OptionParsingFactory<T> factory, string input)
        {
            if (!factory(input, out T value))
            {
                return None<T>();
            }

            return Some(value);
        }


        public abstract Type GetUnderlyingType();
        public abstract bool IsSome();
        public abstract bool IsNone();

        public abstract bool Equals(Option other);
    }
}
