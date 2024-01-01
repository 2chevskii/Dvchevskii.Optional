using System;

namespace Option
{
    public static class Option
    {
        public static Option<T> None<T>() => new None<T>();

        public static Option<T> Some<T>(T value) => new Some<T>(value);

        public static Option<T> Create<T, R>(Factory<R, T> factory, R arg)
        {
            if (!factory(arg, out T value))
            {
                return None<T>();
            }

            return Some(value);
        }
    }

    public delegate bool Factory<in TInput, TOutput>(TInput argument, out TOutput output);
}
