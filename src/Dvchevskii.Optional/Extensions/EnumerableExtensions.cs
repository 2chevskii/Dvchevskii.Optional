using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Dvchevskii.Optional.Extensions
{
    public static class EnumerableExtensions
    {
#if NULLABLE
        public static Option<T> FirstOrNone<T>(this T[] array, Predicate<T>? predicate = null)
        {
            if (predicate == null)
            {
                return array.Length == 0 ? Option.None<T>() : Option.Some(array[0]);
            }
            var index = Array.FindIndex(array, predicate);
            return index == -1 ? Option.None<T>() : Option.Some(array[index]);
        }
#else
        public static Option<T> FirstOrNone<T>(this T[] array, Predicate<T> predicate = null)
        {
            if (predicate == null)
            {
                return array.Length == 0 ? Option.None<T>() : Option.Some(array[0]);
            }
            var index = Array.FindIndex(array, predicate);
            return index == -1 ? Option.None<T>() : Option.Some(array[index]);
        }
#endif

#if NULLABLE
        public static Option<T> FirstOrNone<T>(this List<T> list, Predicate<T>? predicate = null)
        {
            if (predicate == null)
            {
                return list.Count == 0 ? Option.None<T>() : Option.Some(list[0]);
            }
            var index = list.FindIndex(predicate);
            return index == -1 ? Option.None<T>() : Option.Some(list[index]);
        }
#else
        public static Option<T> FirstOrNone<T>(this List<T> list, Predicate<T> predicate = null)
        {
            if (predicate == null)
            {
                return list.Count == 0 ? Option.None<T>() : Option.Some(list[0]);
            }
            var index = list.FindIndex(predicate);
            return index == -1 ? Option.None<T>() : Option.Some(list[index]);
        }
#endif

#if NULLABLE
        public static Option<T> FirstOrNone<T>(
            this IReadOnlyList<T> list,
            Predicate<T>? predicate = null
        )
        {
            if (predicate == null)
            {
                return list.Count == 0 ? Option.None<T>() : Option.Some(list[0]);
            }

            for (var i = 0; i < list.Count; i++)
            {
                var element = list[i];
                if (predicate(element))
                {
                    return Option.Some(element);
                }
            }

            return Option.None<T>();
        }
#else
        public static Option<T> FirstOrNone<T>(
            this IReadOnlyList<T> list,
            Predicate<T> predicate = null
        )
        {
            if (predicate == null)
            {
                return list.Count == 0 ? Option.None<T>() : Option.Some(list[0]);
            }

            for (var i = 0; i < list.Count; i++)
            {
                var element = list[i];
                if (predicate(element))
                {
                    return Option.Some(element);
                }
            }

            return Option.None<T>();
        }
#endif

#if NULLABLE
        public static Option<T> FirstOrNone<T>(this IList<T> list, Predicate<T>? predicate = null)
        {
            if (predicate == null)
            {
                return list.Count == 0 ? Option.None<T>() : Option.Some(list[0]);
            }

            for (var i = 0; i < list.Count; i++)
            {
                var element = list[i];
                if (predicate(element))
                {
                    return Option.Some(element);
                }
            }

            return Option.None<T>();
        }
#else
        public static Option<T> FirstOrNone<T>(this IList<T> list, Predicate<T> predicate = null)
        {
            if (predicate == null)
            {
                return list.Count == 0 ? Option.None<T>() : Option.Some(list[0]);
            }

            for (var i = 0; i < list.Count; i++)
            {
                var element = list[i];
                if (predicate(element))
                {
                    return Option.Some(element);
                }
            }

            return Option.None<T>();
        }
#endif

#if NULLABLE
        public static Option<T> FirstOrNone<T>(
            this ICollection<T> collection,
            Predicate<T>? predicate = null
        )
        {
            if (predicate == null)
            {
                return collection.Count == 0
                    ? Option.None<T>()
                    : Option.Some(collection.ElementAt(0));
            }

            for (var i = 0; i < collection.Count; i++)
            {
                var element = collection.ElementAt(i);
                if (predicate(element))
                {
                    return Option.Some(element);
                }
            }

            return Option.None<T>();
        }
#else
        public static Option<T> FirstOrNone<T>(
            this ICollection<T> collection,
            Predicate<T> predicate = null
        )
        {
            if (predicate == null)
            {
                return collection.Count == 0
                    ? Option.None<T>()
                    : Option.Some(collection.ElementAt(0));
            }

            for (var i = 0; i < collection.Count; i++)
            {
                var element = collection.ElementAt(i);
                if (predicate(element))
                {
                    return Option.Some(element);
                }
            }

            return Option.None<T>();
        }
#endif
#if NULLABLE
        public static Option<T> FirstOrNone<T>(
            this IReadOnlyCollection<T> collection,
            Predicate<T>? predicate = null
        )
        {
            if (predicate == null)
            {
                return collection.Count == 0
                    ? Option.None<T>()
                    : Option.Some(collection.ElementAt(0));
            }

            for (var i = 0; i < collection.Count; i++)
            {
                var element = collection.ElementAt(i);
                if (predicate(element))
                {
                    return Option.Some(element);
                }
            }

            return Option.None<T>();
        }
#else
        public static Option<T> FirstOrNone<T>(
            this IReadOnlyCollection<T> collection,
            Predicate<T> predicate = null
        )
        {
            if (predicate == null)
            {
                return collection.Count == 0
                    ? Option.None<T>()
                    : Option.Some(collection.ElementAt(0));
            }

            for (var i = 0; i < collection.Count; i++)
            {
                var element = collection.ElementAt(i);
                if (predicate(element))
                {
                    return Option.Some(element);
                }
            }

            return Option.None<T>();
        }
#endif

#if NULLABLE
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static Option<T> FirstOrNone<T>(
            this IEnumerable<T> enumerable,
            Predicate<T>? predicate = null
        )
        {
            if (enumerable is T[] array)
            {
                return array.FirstOrNone(predicate);
            }

            if (enumerable is List<T> list)
            {
                return list.FirstOrNone(predicate);
            }

            if (enumerable is IReadOnlyList<T> readonlyList)
            {
                return readonlyList.FirstOrNone(predicate);
            }

            if (enumerable is IReadOnlyCollection<T> readOnlyCollection)
            {
                return readOnlyCollection.FirstOrNone(predicate);
            }

            if (enumerable is IList<T> list2)
            {
                return list2.FirstOrNone(predicate);
            }

            if (enumerable is ICollection<T> collection)
            {
                return collection.FirstOrNone(predicate);
            }

            return FirstOrNoneEnumerableOnly(enumerable, predicate);
        }
#else
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static Option<T> FirstOrNone<T>(
            this IEnumerable<T> enumerable,
            Predicate<T> predicate = null
        )
        {
            if (enumerable is T[] array)
            {
                return array.FirstOrNone(predicate);
            }

            if (enumerable is List<T> list)
            {
                return list.FirstOrNone(predicate);
            }

            if (enumerable is IReadOnlyList<T> readonlyList)
            {
                return readonlyList.FirstOrNone(predicate);
            }

            if (enumerable is IReadOnlyCollection<T> readOnlyCollection)
            {
                return readOnlyCollection.FirstOrNone(predicate);
            }

            if (enumerable is IList<T> list2)
            {
                return list2.FirstOrNone(predicate);
            }

            if (enumerable is ICollection<T> collection)
            {
                return collection.FirstOrNone(predicate);
            }

            return FirstOrNoneEnumerableOnly(enumerable, predicate);
        }
#endif

#if NULLABLE
        private static Option<T> FirstOrNoneEnumerableOnly<T>(
            IEnumerable<T> enumerable,
            Predicate<T>? predicate
        )
        {
            if (predicate == null)
            {
                return enumerable.Any() ? Option.Some(enumerable.First()) : Option.None<T>();
            }

            foreach (var element in enumerable)
            {
                if (predicate(element))
                {
                    return Option.Some(element);
                }
            }

            return Option.None<T>();
        }
#else
        private static Option<T> FirstOrNoneEnumerableOnly<T>(
            IEnumerable<T> enumerable,
            Predicate<T> predicate
        )
        {
            if (predicate == null)
            {
                return enumerable.Any() ? Option.Some(enumerable.First()) : Option.None<T>();
            }

            foreach (var element in enumerable)
            {
                if (predicate(element))
                {
                    return Option.Some(element);
                }
            }

            return Option.None<T>();
        }
#endif
    }
}
