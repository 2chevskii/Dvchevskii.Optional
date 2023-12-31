namespace Option.Extensions
{
    public static class OptionExtensions
    {
        public static (Option<T>, Option<U>) Unzip<T, U>(this Option<(T, U)> self) =>
            self.IsNone()
                ? (Option.None<T>(), Option.None<U>())
                : (Option.Some(self.Unwrap().Item1), Option.Some(self.Unwrap().Item2));

        public static Option<T> Flatten<T>(this Option<Option<T>> self) =>
            self.MapOrElse<Option<T>>(Option.None<T>, val => val);
    }
}
