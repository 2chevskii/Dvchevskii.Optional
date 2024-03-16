using System.Collections.Generic;
using System.Linq;

namespace Dvchevskii.Optional.Extensions
{
    public static class EnumerableOptionExtensions
    {
        /// <summary>
        /// Unwraps all Options which hold value
        /// </summary>
        /// <param name="enumerable"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> UnwrapAll<T>(this IEnumerable<Option<T>> enumerable) =>
            from option in enumerable
            where option.IsSome
            select option.Unwrap();

        public static IEnumerable<Option<T>> OnlySome<T>(this IEnumerable<Option<T>> enumerable) =>
            enumerable.Where(option => option.IsSome);

#if NULLABLE
        public static IEnumerable<Option<T>> WrapAllNullAsNone<T>(
            this IEnumerable<T?> enumerable
        ) =>
            enumerable.Select(entry => Equals(entry, null) ? Option.None<T>() : Option.Some(entry));

        public static IEnumerable<Option<T?>> WrapAll<T>(this IEnumerable<T?> enumerable) =>
            enumerable.Select(Option.Some);
#else
        public static IEnumerable<Option<T>> WrapAll<T>(this IEnumerable<T> enumerable) =>
            enumerable.Select(Option.Some);

        public static IEnumerable<Option<T>> WrapAllNullAsNone<T>(this IEnumerable<T> enumerable)
            where T : class =>
            enumerable.Select(entry => entry == null ? Option.None<T>() : Option.Some(entry));

        public static IEnumerable<Option<T>> WrapAllNullAsNone<T>(this IEnumerable<T?> enumerable)
            where T : struct =>
            enumerable.Select(
                entry => entry.HasValue ? Option.Some(entry.Value) : Option.None<T>()
            );
#endif
    }
}
