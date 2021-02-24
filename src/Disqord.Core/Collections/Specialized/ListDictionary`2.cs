using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Disqord.Collections.Specialized
{
    public class ListDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>, IList<KeyValuePair<TKey, TValue>>, IReadOnlyList<KeyValuePair<TKey, TValue>> where TKey: notnull
    {
        public int Count => _items.Count;

        public IEnumerable<TKey> Keys => _items.Select(x => x.Key);

        public IEnumerable<TValue> Values => _items.Select(x => x.Value);

        public TValue this[TKey key]
        {
            get => _items.Find(x => x.Key.Equals(key)).Value;
            set
            {
                var index = _items.FindIndex(x => x.Key.Equals(key));
                if (index != -1)
                    _items[index] = KeyValuePair.Create(key, value);
                else
                    Add(key, value);
            }
        }

        public KeyValuePair<TKey, TValue> this[int index]
        {
            get => _items[index];
            set => _items[index] = value;
        }

        bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => false;
        ICollection<TKey> IDictionary<TKey, TValue>.Keys => _items.Select(x => x.Key).ToArray();
        ICollection<TValue> IDictionary<TKey, TValue>.Values => _items.Select(x => x.Value).ToArray();

        private readonly List<KeyValuePair<TKey, TValue>> _items;

        public ListDictionary()
        {
            _items = new List<KeyValuePair<TKey, TValue>>();
        }

        public ListDictionary(int capacity)
        {
            _items = new List<KeyValuePair<TKey, TValue>>(capacity);
        }

        public void Add(TKey key, TValue value)
            => _items.Add(KeyValuePair.Create(key, value));

        public void Add(KeyValuePair<TKey, TValue> item)
            => _items.Add(item);

        public void Clear()
            => _items.Clear();

        public bool Contains(KeyValuePair<TKey, TValue> item)
            => _items.Contains(item);

        public bool ContainsKey(TKey key)
            => Keys.Contains(key);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            => _items.CopyTo(array, arrayIndex);

        public int IndexOf(KeyValuePair<TKey, TValue> item)
            => _items.IndexOf(item);

        public void Insert(int index, KeyValuePair<TKey, TValue> item)
            => _items.Insert(index, item);

        public bool Remove(TKey key)
        {
            var index = _items.FindIndex(x => x.Key.Equals(key));
            if (index != -1)
            {
                _items.RemoveAt(index);
                return true;
            }

            return false;
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
            => _items.Remove(item);

        public void RemoveAt(int index)
            => _items.RemoveAt(index);

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            var index = _items.FindIndex(x => x.Key.Equals(key));
            if (index != -1)
            {
                value = _items[index].Value;
                return true;
            }

            value = default;
            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            => _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
