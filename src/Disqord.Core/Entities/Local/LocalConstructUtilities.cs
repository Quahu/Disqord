using System.Collections.Generic;
using Qommon;
using Qommon.Collections;

namespace Disqord
{
    internal static class LocalConstructUtilities
    {
        public static bool Add<T>(this Optional<IList<T>> optional, T item, out IList<T> list)
        {
            if (optional.TryGetValue(out list) && list != null && !list.IsReadOnly)
            {
                list.Add(item);
                return true;
            }

            list = list != null
                ? new List<T>(list)
                : new List<T>();

            list.Add(item);
            return false;
        }

        public static bool With<T>(this Optional<IList<T>> optional, IEnumerable<T> items, out IList<T> list)
        {
            if (optional.TryGetValue(out list) && list != null && !list.IsReadOnly)
            {
                list.Clear();
                list.AddRange(items);
                return true;
            }

            if (list != null)
            {
                list = new List<T>(list);
                list.AddRange(items);
            }
            else
            {
                list = new List<T>(items);
            }

            return false;
        }

        public static bool Add<TKey, TValue>(this Optional<IDictionary<TKey, TValue>> optional, TKey key, TValue value, out IDictionary<TKey, TValue> dictionary)
            where TKey : notnull
        {
            if (optional.TryGetValue(out dictionary) && dictionary != null && !dictionary.IsReadOnly)
            {
                dictionary.Add(key, value);
                return true;
            }

            dictionary = dictionary != null
                ? new Dictionary<TKey, TValue>(dictionary)
                : new Dictionary<TKey, TValue>();

            dictionary.Add(key, value);
            return false;
        }

        public static bool With<TKey, TValue>(this Optional<IDictionary<TKey, TValue>> optional, IEnumerable<KeyValuePair<TKey, TValue>> items, out IDictionary<TKey, TValue> dictionary)
            where TKey : notnull
        {
            if (optional.TryGetValue(out dictionary) && dictionary != null && !dictionary.IsReadOnly)
            {
                dictionary.Clear();
                foreach (var item in items)
                    dictionary.Add(item);

                return true;
            }

            if (dictionary != null)
            {
                dictionary = new Dictionary<TKey, TValue>(dictionary);
                foreach (var item in items)
                    dictionary.Add(item);
            }
            else
            {
                dictionary = new Dictionary<TKey, TValue>(items);
            }

            return false;
        }
    }
}
