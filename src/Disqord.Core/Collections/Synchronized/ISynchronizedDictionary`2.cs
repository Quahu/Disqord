using System;
using System.Collections;
using System.Collections.Generic;

namespace Disqord.Collections.Synchronized
{
    /// <summary>
    ///     Represents a dictionary of which methods are synchronized by <see langword="lock"/>ing its instance.
    /// </summary>
    public interface ISynchronizedDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        new TKey[] Keys { get; }

        new TValue[] Values { get; }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys;
        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values;

        bool TryAdd(TKey key, TValue value);

        bool TryRemove(TKey key, out TValue value);

        KeyValuePair<TKey, TValue>[] ToArray()
        {
            lock (this)
            {
                var array = new KeyValuePair<TKey, TValue>[Count];
                CopyTo(array, 0);
                return array;
            }
        }

        TValue GetValueOrDefault(TKey key)
            => TryGetValue(key, out var value)
                ? value
                : default;

        TValue GetOrAdd(TKey key, TValue value)
        {
            lock (this)
            {
                if (TryGetValue(key, out var existingValue))
                    return existingValue;

                Add(key, value);
                return value;
            }
        }

        TValue GetOrAdd(TKey key, Func<TKey, TValue> factory)
        {
            lock (this)
            {
                if (TryGetValue(key, out var value))
                    return value;

                value = factory(key);
                Add(key, value);
                return value;
            }
        }

        TValue GetOrAdd<TState>(TKey key, Func<TKey, TState, TValue> factory, TState state)
        {
            lock (this)
            {
                if (TryGetValue(key, out var value))
                    return value;

                value = factory(key, state);
                Add(key, value);
                return value;
            }
        }

        TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateFactory)
        {
            lock (this)
            {
                if (TryGetValue(key, out var value))
                {
                    value = updateFactory(key, value);
                    this[key] = value;
                    return value;
                }

                Add(key, addValue);
                return addValue;
            }
        }

        TValue AddOrUpdate(TKey key, Func<TKey, TValue> addFactory, Func<TKey, TValue, TValue> updateFactory)
        {
            lock (this)
            {
                if (TryGetValue(key, out var value))
                {
                    value = updateFactory(key, value);
                    this[key] = value;
                    return value;
                }

                value = addFactory(key);
                Add(key, value);
                return value;
            }
        }

        TValue AddOrUpdate<TState>(TKey key, Func<TKey, TState, TValue> addFactory, Func<TKey, TState, TValue, TValue> updateFactory, TState state)
        {
            lock (this)
            {
                if (TryGetValue(key, out var value))
                {
                    value = updateFactory(key, state, value);
                    this[key] = value;
                    return value;
                }

                value = addFactory(key, state);
                Add(key, value);
                return value;
            }
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
            => (ToArray() as IList<KeyValuePair<TKey, TValue>>).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
