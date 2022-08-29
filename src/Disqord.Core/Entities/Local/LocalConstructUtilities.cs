using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Qommon;
using Qommon.Collections;

namespace Disqord;

internal static class LocalConstructUtilities
{
    public static bool Add<T>(this Optional<IList<T>> optional, T item, [MaybeNullWhen(false)] out IList<T> list)
    {
        if (optional.TryGetValue(out list!) && list != null && !list.IsReadOnly)
        {
            list.Add(item);
            return false;
        }

        list = list != null
            ? new List<T>(list)
            : new List<T>();

        list.Add(item);
        return true;
    }

    public static bool With<T>(this Optional<IList<T>> optional, IEnumerable<T> items, [MaybeNullWhen(false)] out IList<T> list)
    {
        if (optional.TryGetValue(out list!) && list != null && !list.IsReadOnly)
        {
            list.Clear();
            list.AddRange(items);
            return false;
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

        return true;
    }

    public static bool Add<TKey, TValue>(this Optional<IDictionary<TKey, TValue>> optional, TKey key, TValue value, [MaybeNullWhen(false)] out IDictionary<TKey, TValue> dictionary)
        where TKey : notnull
    {
        if (optional.TryGetValue(out dictionary!) && dictionary != null && !dictionary.IsReadOnly)
        {
            dictionary.Add(key, value);
            return false;
        }

        dictionary = dictionary != null
            ? new Dictionary<TKey, TValue>(dictionary)
            : new Dictionary<TKey, TValue>();

        dictionary.Add(key, value);
        return true;
    }

    public static bool With<TKey, TValue>(this Optional<IDictionary<TKey, TValue>> optional, IEnumerable<KeyValuePair<TKey, TValue>> items, [MaybeNullWhen(false)] out IDictionary<TKey, TValue> dictionary)
        where TKey : notnull
    {
        if (optional.TryGetValue(out dictionary!) && dictionary != null && !dictionary.IsReadOnly)
        {
            dictionary.Clear();
            foreach (var item in items)
                dictionary.Add(item);

            return false;
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

        return true;
    }

#nullable disable // Makes the clone methods compatible with both Optional<T> and Optional<T?>.
    public static Optional<TConstruct> Clone<TConstruct>(this Optional<TConstruct> optional)
        where TConstruct : class, ILocalConstruct<TConstruct>
    {
        if (!optional.HasValue)
            return default;

        var construct = optional.Value;
        return construct?.Clone();
    }

    public static Optional<IList<TConstruct>> DeepClone<TConstruct>(this Optional<IList<TConstruct>> optional)
        where TConstruct : class, ILocalConstruct<TConstruct>
    {
        if (!optional.HasValue)
            return default;

        var list = optional.Value;
        var listCount = list.Count;
        var copyList = new List<TConstruct>(listCount);
        for (var i = 0; i < listCount; i++)
        {
            var construct = list[i];
            copyList.Add(construct.Clone());
        }

        return copyList;
    }
#nullable enable

    public static Optional<IList<T>> Clone<T>(this Optional<IList<T>> optional)
    {
        if (!optional.HasValue)
            return default;

        var list = optional.Value;
        var listCount = list.Count;
        var copyList = new List<T>(listCount);
        for (var i = 0; i < listCount; i++)
        {
            var value = list[i];
            copyList.Add(value);
        }

        return copyList;
    }

    public static Optional<T[]> ToArray<T>(this Optional<IList<T>> optional)
    {
        if (!optional.HasValue)
            return default;

        var list = optional.Value;
        var array = new T[list.Count];
        list.CopyTo(array, 0);
        return array;
    }
}
