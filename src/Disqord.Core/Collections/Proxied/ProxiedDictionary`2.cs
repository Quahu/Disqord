using System.Collections;
using System.Collections.Generic;

namespace Disqord.Collections.Proxied
{
    public abstract class ProxiedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue>
    {
        public virtual ICollection<TKey> Keys => Dictionary.Keys;

        public virtual ICollection<TValue> Values => Dictionary.Values;

        public virtual int Count => Dictionary.Count;

        public virtual bool IsReadOnly => Dictionary.IsReadOnly;

        public virtual TValue this[TKey key]
        {
            get => Dictionary[key];
            set => Dictionary[key] = value;
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        protected IDictionary<TKey, TValue> Dictionary { get; }

        protected ProxiedDictionary(IDictionary<TKey, TValue> dictionary)
        {
            Dictionary = dictionary;
        }

        public virtual void Add(TKey key, TValue value)
            => Dictionary.Add(key, value);

        public virtual void Clear()
            => Dictionary.Clear();

        public virtual bool ContainsKey(TKey key)
            => Dictionary.ContainsKey(key);

        public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            => Dictionary.CopyTo(array, arrayIndex);

        public virtual bool Remove(TKey key)
            => Dictionary.Remove(key);

        public virtual bool TryGetValue(TKey key, out TValue value)
            => Dictionary.TryGetValue(key, out value);

        public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
            => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
            => Add(item.Key, item.Value);

        bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
            => ContainsKey(item.Key);

        bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
            => Remove(item.Key);
    }
}
