using System;
using System.Collections.Generic;
using System.Linq;

namespace Disqord.Collections
{
    internal static class ReadOnlyCollectionExtensions
    {
        public static IReadOnlyDictionary<TKey, TValue> ReadOnly<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
            => new ReadOnlyDictionary<TKey, TValue>(dictionary);

        public static IReadOnlyList<T> ReadOnly<T>(this IList<T> list)
            => new ReadOnlyList<T>(list);

        public static IReadOnlyCollection<T> ReadOnly<T>(this ICollection<T> collection)
            => new ReadOnlyCollection<T>(collection);

        public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TSource, TKey, TValue>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TValue> valueSelector)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            foreach (var item in source)
            {
                var key = keySelector(item);
                var value = valueSelector(item);
                dictionary[key] = value;
            }

            return dictionary.ReadOnly();
        }

        public static IReadOnlyDictionary<TKey, TValue> ToReadOnlyDictionary<TSource, TKey, TValue, TState>(
            this IList<TSource> source, Func<TSource, TState, TKey> keySelector, Func<TSource, TState, TValue> valueSelector,
            TState state)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            foreach (var item in source)
            {
                var key = keySelector(item, state);
                var value = valueSelector(item, state);
                dictionary[key] = value;
            }

            return dictionary.ReadOnly();
        }

        public static IReadOnlyList<TResult> ToReadOnlyList<TSource, TResult>(
            this IList<TSource> source, Func<TSource, TResult> selector)
        {
            var array = new TResult[source.Count];
            for (var i = 0; i < source.Count; i++)
            {
                var value = selector(source[i]);
                array[i] = value;
            }

            return array.ReadOnly();
        }

        public static IReadOnlyList<TResult> ToReadOnlyList<TSource, TState, TResult>(
            this IList<TSource> source, TState state, Func<TSource, TState, TResult> selector)
        {
            var array = new TResult[source.Count];
            for (var i = 0; i < source.Count; i++)
            {
                var value = selector(source[i], state);
                array[i] = value;
            }

            return array.ReadOnly();
        }

        public static IReadOnlyList<T> ToReadOnlyList<T>(this IEnumerable<T> source)
            => source.ToArray().ReadOnly();
    }
}
