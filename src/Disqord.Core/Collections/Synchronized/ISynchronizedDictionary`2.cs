using System;
using System.Collections.Generic;

namespace Disqord.Collections.Synchronized
{
    /// <summary>
    ///     Represents a dictionary to which access can be synchronized by <see langword="lock"/>ing it.
    /// </summary>
    public interface ISynchronizedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        TValue GetOrAdd(TKey key, TValue value);

        TValue GetOrAdd(TKey key, Func<TKey, TValue> valueFactory);

        TValue GetOrAdd<TState>(TKey key, Func<TKey, TState, TValue> valueFactory, TState state);

        TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateFactory);

        TValue AddOrUpdate(TKey key, Func<TKey, TValue> addFactory, Func<TKey, TValue, TValue> updateFactory);

        TValue AddOrUpdate<TState>(TKey key, Func<TKey, TState, TValue> addFactory, Func<TKey, TState, TValue, TValue> updateFactory, TState state);

        bool TryAdd(TKey key, TValue value);

        bool TryRemove(TKey key, out TValue value);

        void EnsureCapacity(int capacity);

        void TrimExcess();

        void TrimExcess(int capacity);

        KeyValuePair<TKey, TValue>[] ToArray();
    }
}
