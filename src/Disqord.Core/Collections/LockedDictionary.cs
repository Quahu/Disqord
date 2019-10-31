using System;
using System.Collections;
using System.Collections.Generic;

namespace Disqord.Collections
{
    internal sealed class LockedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        public ICollection<TKey> Keys => _keys;

        public ICollection<TValue> Values => _values;

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

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;

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

        private readonly object _lock;
        private readonly Dictionary<TKey, TValue> _dictionary;
        private readonly LockedDictionaryCollection<TKey> _keys;
        private readonly LockedDictionaryCollection<TValue> _values;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        public LockedDictionary() : this(0)
        { }

        public LockedDictionary(int capacity)
        {
            _lock = new object();
            _dictionary = new Dictionary<TKey, TValue>(capacity);
            _keys = new LockedDictionaryCollection<TKey>(_lock, _dictionary.Keys);
            _values = new LockedDictionaryCollection<TValue>(_lock, _dictionary.Values);
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

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            KeyValuePair<TKey, TValue>[] array;
            lock (_lock)
            {
                array = new KeyValuePair<TKey, TValue>[Count];
                (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, 0);
            }

            return (array as IReadOnlyList<KeyValuePair<TKey, TValue>>).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
        {
            lock (_lock)
            {
                (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).Add(item);
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
        {
            lock (_lock)
            {
                return (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).Contains(item);
            }
        }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock (_lock)
            {
                (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, arrayIndex);
            }
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (_lock)
            {
                return (_dictionary as ICollection<KeyValuePair<TKey, TValue>>).Remove(item);
            }
        }

        private sealed class LockedDictionaryCollection<T> : ICollection<T>
        {
            public int Count
            {
                get
                {
                    lock (_lock)
                    {
                        return _collection.Count;
                    }
                }
            }

            public bool IsReadOnly => false;

            private readonly ICollection<T> _collection;
            private readonly object _lock;

            public LockedDictionaryCollection(object @lock, ICollection<T> collection)
            {
                _lock = @lock;
                _collection = collection;
            }

            public void Add(T item)
            {
                lock (_lock)
                {
                    _collection.Add(item);
                }
            }

            public void Clear()
            {
                lock (_lock)
                {
                    _collection.Clear();
                }
            }

            public bool Contains(T item)
            {
                lock (_lock)
                {
                    return this._collection.Contains(item);
                }
            }

            public void CopyTo(T[] array, int arrayIndex)
            {
                lock (_lock)
                {
                    _collection.CopyTo(array, arrayIndex);
                }
            }

            public bool Remove(T item)
            {
                lock (_lock)
                {
                    return _collection.Remove(item);
                }
            }

            public IEnumerator<T> GetEnumerator()
            {
                T[] array;
                lock (_lock)
                {
                    array = new T[Count];
                    _collection.CopyTo(array, 0);
                }

                return (array as IReadOnlyList<T>).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();
        }
    }
}
