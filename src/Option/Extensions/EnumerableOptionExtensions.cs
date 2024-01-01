using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Option.Extensions
{
    public static class EnumerableOptionExtensions
    {
        public static IEnumerable<T> UnwrapAll<T>(this IEnumerable<Option<T>> enumerable) =>
            enumerable.Where(option => option.IsSome()).Select(option => option.Unwrap());
    }
}
