using Qommon.Collections.ThreadSafe;

namespace Disqord;

internal static class ThreadSafeDictionaryExtensions
{
    public static TValue? GetValueOrDefault<TKey, TValue>(this IThreadSafeDictionary<TKey, TValue> dictionary, TKey key)
        where TKey : notnull
    {
        return dictionary.TryGetValue(key, out var value) ? value : default;
    }
}
