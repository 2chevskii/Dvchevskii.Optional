namespace Dvchevskii.Optional.Extensions
{
    public static class OptionExtensions
    {
        public static (Option<T>, Option<U>) Unzip<T, U>(this Option<(T, U)> self) =>
            self.IsNone()
                ? (Option.None<T>(), Option.None<U>())
                : (Option.Some(self.Unwrap().Item1), Option.Some(self.Unwrap().Item2));

        public static Option<T> Flatten<T>(this Option<Option<T>> self) =>
            self.MapOrElse(val => val, Option.None<T>);

        public static Option<T> ToOption<T>(this T value) => Option.Some(value);
    }
}
