namespace Dvchevskii.Optional.Extensions
{
    public static class NullableExtensions
    {
        public static Option<T> AsOption<T>(this T? nullable)
            where T : struct => nullable.HasValue ? Option.Some(nullable.Value) : Option.None<T>();

        public static Option<T> AsOption<T>(this T nullable)
            where T : class =>
            ReferenceEquals(nullable, null) ? Option.None<T>() : Option.Some(nullable);
    }
}
