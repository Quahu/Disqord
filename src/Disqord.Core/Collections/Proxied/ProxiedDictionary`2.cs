using System.Collections;
using System.Collections.Generic;

namespace Disqord.Collections.Proxied
{
    /// <summary>
    ///     Represents an implementation of <see cref="IDictionary{TKey, TValue}"/> and <see cref="IReadOnlyDictionary{TKey, TValue}"/>
    ///     by wrapping an <see cref="IDictionary{TKey, TValue}"/> with virtual implementations making overriding individual members very simple.
    /// </summary>
    /// <typeparam name="TKey"> The type of keys in the dictionary. </typeparam>
    /// <typeparam name="TValue"> The type of values in the dictionary. </typeparam>
    public abstract class ProxiedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IReadOnlyDictionary<TKey, TValue> where TKey: notnull
    {
        /// <inheritdoc/>
        public virtual ICollection<TKey> Keys => Dictionary.Keys;

        /// <inheritdoc/>
        public virtual ICollection<TValue> Values => Dictionary.Values;

        /// <inheritdoc/>
        public virtual int Count => Dictionary.Count;

        /// <inheritdoc/>
        public virtual bool IsReadOnly => Dictionary.IsReadOnly;

        /// <inheritdoc/>
        public virtual TValue this[TKey key]
        {
            get => Dictionary[key];
            set => Dictionary[key] = value;
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => Keys;
        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => Values;

        /// <summary>
        ///     Gets the wrapped dictionary.
        /// </summary>
        protected IDictionary<TKey, TValue> Dictionary { get; }

        /// <summary>
        ///     Instantiates a new <see cref="ProxiedDictionary{TKey, TValue}"/>.
        /// </summary>
        /// <param name="dictionary"> The dictionary to wrap. </param>
        protected ProxiedDictionary(IDictionary<TKey, TValue> dictionary)
        {
            Dictionary = dictionary;
        }

        /// <inheritdoc/>
        public virtual void Add(TKey key, TValue value)
            => Dictionary.Add(key, value);

        /// <inheritdoc/>
        public virtual void Clear()
            => Dictionary.Clear();

        /// <inheritdoc/>
        public virtual bool ContainsKey(TKey key)
            => Dictionary.ContainsKey(key);

        /// <inheritdoc/>
        public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
            => Dictionary.CopyTo(array, arrayIndex);

        /// <inheritdoc/>
        public virtual bool Remove(TKey key)
            => Dictionary.Remove(key);

        /// <inheritdoc/>
        public virtual bool TryGetValue(TKey key, out TValue value)
            => Dictionary.TryGetValue(key, out value);

        /// <inheritdoc/>
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
