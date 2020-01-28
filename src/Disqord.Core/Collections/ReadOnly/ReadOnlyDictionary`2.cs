using System;
using System.Collections;
using System.Collections.Generic;

namespace Disqord.Collections
{
    internal sealed class ReadOnlyDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        public static readonly IReadOnlyDictionary<TKey, TValue> Empty = new Dictionary<TKey, TValue>(0).ReadOnly();

        public IReadOnlyCollection<TKey> Keys => _dictionary.Keys.ReadOnly();

        public IReadOnlyCollection<TValue> Values => _dictionary.Values.ReadOnly();

        public int Count => _dictionary.Count;

        public TValue this[TKey key] => _dictionary[key];

        private readonly IDictionary<TKey, TValue> _dictionary;

        public ReadOnlyDictionary(IDictionary<TKey, TValue> dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            _dictionary = dictionary;
        }

        public bool ContainsKey(TKey key)
            => _dictionary.ContainsKey(key);

        public bool TryGetValue(TKey key, out TValue value)
            => _dictionary.TryGetValue(key, out value);

        public bool Contains(KeyValuePair<TKey, TValue> item)
            => _dictionary.Contains(item);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            => _dictionary.CopyTo(array, arrayIndex);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            => _dictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => Keys as ICollection<TKey>;
        ICollection<TValue> IDictionary<TKey, TValue>.Values => Values as ICollection<TValue>;
        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;
        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => true;
        TValue IDictionary<TKey, TValue>.this[TKey key]
        {
            get => this[key];
            set => throw new NotSupportedException();
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
            => throw new NotSupportedException();

        bool IDictionary<TKey, TValue>.Remove(TKey key)
            => throw new NotSupportedException();

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
            => throw new NotSupportedException();

        void ICollection<KeyValuePair<TKey, TValue>>.Clear()
            => throw new NotSupportedException();

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
            => throw new NotSupportedException();
    }
}
