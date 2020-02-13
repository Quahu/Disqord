using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Disqord.Collections
{
    internal sealed class LockedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        public ICollection<TKey> Keys
        {
            get
            {
                lock (_lock)
                {
                    return _dictionary.Keys.ToArray();
                }
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                lock (_lock)
                {
                    return _dictionary.Values.ToArray();
                }
            }
        }

        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _dictionary.Count;
                }
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (_lock)
                {
                    return _dictionary[key];
                }
            }
            set
            {
                lock (_lock)
                {
                    _dictionary[key] = value;
                }
            }
        }

        private readonly Dictionary<TKey, TValue> _dictionary;
        private readonly object _lock;

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        public LockedDictionary() : this(0)
        { }

        public LockedDictionary(int capacity)
        {
            _dictionary = new Dictionary<TKey, TValue>(capacity);
            _lock = new object();
        }

        public LockedDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            _dictionary = new Dictionary<TKey, TValue>(collection);
            _lock = new object();
        }

        public TValue GetOrAdd(TKey key, Func<TKey, TValue> factory)
        {
            lock (_lock)
            {
                if (_dictionary.TryGetValue(key, out var value))
                    return value;

                value = factory(key);
                _dictionary.Add(key, value);
                return value;
            }
        }

        public TValue GetOrAdd<TArg>(TKey key, Func<TKey, TArg, TValue> factory, TArg argument)
        {
            lock (_lock)
            {
                if (_dictionary.TryGetValue(key, out var value))
                    return value;

                value = factory(key, argument);
                _dictionary.Add(key, value);
                return value;
            }
        }

        public void Add(TKey key, TValue value)
        {
            lock (_lock)
            {
                _dictionary.Add(key, value);
            }
        }

        public TValue AddOrUpdate(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateFactory)
        {
            lock (_lock)
            {
                if (_dictionary.TryGetValue(key, out var value))
                {
                    value = updateFactory(key, value);
                    _dictionary[key] = value;
                    return value;
                }
                else
                {
                    _dictionary.Add(key, addValue);
                    return addValue;
                }
            }
        }

        public TValue AddOrUpdate(TKey key, Func<TKey, TValue> addFactory, Func<TKey, TValue, TValue> updateFactory)
        {
            lock (_lock)
            {
                if (_dictionary.TryGetValue(key, out var value))
                {
                    value = updateFactory(key, value);
                    _dictionary[key] = value;
                    return value;
                }
                else
                {
                    value = addFactory(key);
                    _dictionary.Add(key, value);
                    return value;
                }
            }
        }

        public bool TryAdd(TKey key, TValue value)
        {
            lock (_lock)
            {
                return _dictionary.TryAdd(key, value);
            }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _dictionary.Clear();
            }
        }

        public bool ContainsKey(TKey key)
        {
            lock (_lock)
            {
                return _dictionary.ContainsKey(key);
            }
        }

        public bool Remove(TKey key)
        {
            lock (_lock)
            {
                return _dictionary.Remove(key);
            }
        }

        public bool TryRemove(TKey key, out TValue value)
        {
            lock (_lock)
            {
                return _dictionary.Remove(key, out value);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_lock)
            {
                return _dictionary.TryGetValue(key, out value);
            }
        }

        public KeyValuePair<TKey, TValue>[] ToArray()
        {
            lock (_lock)
            {
                var array = new KeyValuePair<TKey, TValue>[_dictionary.Count];
                (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, 0);
                return array;
            }
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            => (ToArray() as IReadOnlyList<KeyValuePair<TKey, TValue>>).GetEnumerator();
        //=> _dictionary.GetEnumerator().Locked(_lock);

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        // Unused internally.
        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
            => ContainsKey(item.Key);

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock (_lock)
            {
                (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, arrayIndex);
            }
        }

        // Unused both internally and externally.
        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
            => throw new NotSupportedException();

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
            => throw new NotSupportedException();
    }
}
