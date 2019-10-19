using System.Collections.Concurrent;

namespace Disqord
{
    internal static class Extensions
    {
        public static ConcurrentDictionary<TKey, TValue> CreateConcurrentDictionary<TKey, TValue>(int capacity)
            => new ConcurrentDictionary<TKey, TValue>(1, capacity);
    }
}
