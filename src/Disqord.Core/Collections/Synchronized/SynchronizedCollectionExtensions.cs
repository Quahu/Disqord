using System.Collections.Generic;

namespace Disqord.Collections.Synchronized
{
    public static class SynchronizedCollectionExtensions
    {
        public static ISynchronizedDictionary<TKey, TValue> Synchronized<TKey, TValue>(this IDictionary<TKey, TValue> dictionary) where TKey : notnull
            => new SynchronizedDictionary<TKey, TValue>(dictionary);
    }
}
