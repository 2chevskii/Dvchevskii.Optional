namespace Dvchevskii.Optional.Extensions
{
    public static class NullableExtensions
    {
        public static Option<T> NoneIfNull<T>(this T? nullable) where T : struct
        {
            if (nullable.HasValue)
            {
                return Option.Some(nullable.Value);
            }

            return Option.None<T>();
        }
    }
}
