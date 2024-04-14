namespace Dvchevskii.Optional.Extensions
{
    public static class NullableExtensions
    {
        public static Option<T> AsOption<T>(this T? nullable)
            where T : struct
        {
            return nullable.HasValue ? Option.Some(nullable.Value) : Option.None<T>();
        }

        public static Option<T> AsOption<T>(this T nullable)
            where T : class
        {
            return nullable == null ? Option.None<T>() : Option.Some(nullable);
        }
    }
}
