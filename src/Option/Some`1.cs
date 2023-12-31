using System;

namespace Option
{
    public sealed class Some<T> : Option<T>
    {
        private readonly T _value;

        public Some(T value)
        {
            _value = value;
        }

        public override bool IsNone() => false;

        public override bool IsSome() => true;

        public override bool IsSomeAnd(Predicate<T> predicate) => predicate(_value);

        public override Option<T> Inspect(Action<T> inspector)
        {
            inspector(_value);
            return this;
        }

        public override T Expect(string message) => _value;

        public override T Unwrap() => _value;

        public override T UnwrapOr(T defaultValue) => _value;

        public override T UnwrapOrElse(Func<T> defaultValueFactory) => _value;

        public override T UnwrapOrDefault() => _value;

        public override Option<U> Map<U>(Func<T, U> mapper) => new Some<U>(mapper(_value));

        public override U MapOr<U>(U defaultValue, Func<T, U> mapper) => mapper(_value);

        public override U MapOrElse<U>(Func<U> defaultValueFactory, Func<T, U> mapper) =>
            mapper(_value);

        public override Option<U> And<U>(Option<U> optionB) => optionB;

        public override Option<U> AndThen<U>(Func<T, Option<U>> optionBFactory) =>
            optionBFactory(_value);

        public override Option<T> Filter(Predicate<T> predicate) =>
            predicate(_value) ? Option.Some(_value) : Option.None<T>();

        public override Option<T> Or(Option<T> optionB) => this;

        public override Option<T> OrElse(Func<Option<T>> optionBFactory) => this;

        public override Option<T> XOr(Option<T> optionB) =>
            optionB.IsSome() ? Option.None<T>() : this;

        public override Option<(T, U)> Zip<U>(Option<U> other) =>
            other.IsNone() ? Option.None<(T, U)>() : Option.Some((Unwrap(), other.Unwrap()));

        public override Option<R> ZipWith<U, R>(Option<U> other, Func<T, U, R> func) =>
            other.IsNone() ? Option.None<R>() : Option.Some(func(Unwrap(), other.Unwrap()));
    }
}
