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
            where option.IsSome()
            select option.Unwrap();

        /// <summary>
        /// Wraps all entries in collection to Options
        /// </summary>
        /// <param name="enumerable">The collection</param>
        /// <param name="defaultAsNone">
        /// If true - entries which are equal to their type's default value (i.e. null for reference types) will be wrapped as None.
        /// If false - all output Options will be Some
        /// </param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<Option<T>> WrapAll<T>(
            this IEnumerable<T> enumerable,
            bool defaultAsNone = false
        )
        {
            foreach (T item in enumerable)
            {
                if (defaultAsNone)
                {
                    bool isDefault = item.Equals(default(T));
                    if (isDefault)
                    {
                        yield return Option.None<T>();
                    }
                }

                yield return Option.Some(item);
            }
        }
    }
}
