using System;

// ReSharper disable InconsistentNaming

namespace Option
{
    public abstract class Option<T> : IOption, IEquatable<Option<T>>
    {
        protected internal Option() { }

        public static explicit operator T(Option<T> option) => option.Unwrap();
        public static implicit operator Option<T>(T value) => Option.Some(value);

        public virtual Type GetUnderlyingType() => typeof(T);

        public abstract bool IsNone();
        public abstract bool IsSome();
        public abstract bool IsSomeAnd(Predicate<T> predicate);
        public abstract Option<T> Inspect(Action<T> inspector);
        public abstract T Expect(string message);

        public abstract T Unwrap();

        public abstract T UnwrapOr(T defaultValue);

        public abstract T UnwrapOrElse(Func<T> defaultValueFactory);

        public abstract T UnwrapOrDefault();

        public abstract Option<U> Map<U>(Func<T, U> mapper);

        public abstract U MapOr<U>(U defaultValue, Func<T, U> mapper);

        public abstract U MapOrElse<U>(Func<U> defaultValueFactory, Func<T, U> mapper);

        /*result callbacks*/

        public abstract Option<U> And<U>(Option<U> optionB);

        public abstract Option<U> AndThen<U>(Func<T, Option<U>> optionBFactory);

        public abstract Option<T> Filter(Predicate<T> predicate);

        public abstract Option<T> Or(Option<T> optionB);

        public abstract Option<T> OrElse(Func<Option<T>> optionBFactory);

        public abstract Option<T> XOr(Option<T> optionB);

        public virtual bool Equals(Option<T> other)
        {
            if (IsSome())
            {
                if (other.IsNone())
                {
                    return false;
                }

                return Unwrap().Equals(other.Unwrap());
            }

            if (other.IsSome())
            {
                return false;
            }

            // both is None
            return true;
        }

        public virtual bool Equals(IOption other)
        {
            if (IsNone() && other.IsNone())
            {
                return true;
            }

            if (other is Option<T> option)
            {
                return Equals(option);
            }

            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is IOption option)
            {
                return Equals(option);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (IsSome())
            {
                return Unwrap().GetHashCode();
            }

            Type type = GetUnderlyingType();
            int typeHc = type.GetHashCode();
            return unchecked(((typeHc * 17) >> 1) * 31);
        }

        public abstract Option<(T,U)> Zip<U>(Option<U> other);

        public abstract Option<R> ZipWith<U, R>(Option<U> other, Func<T, U, R> func);
    }
}
