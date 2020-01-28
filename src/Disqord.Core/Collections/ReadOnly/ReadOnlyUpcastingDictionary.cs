using System.Collections;
using System.Collections.Generic;

namespace Disqord.Collections
{
    internal sealed class ReadOnlyUpcastingDictionary<TKey, TOriginalValue, TNewValue> : IReadOnlyDictionary<TKey, TNewValue>
        where TOriginalValue : class, TNewValue
    {
        public IEnumerable<TKey> Keys => _dictionary.Keys;

        public IEnumerable<TNewValue> Values => _dictionary.Values;

        public int Count => _dictionary.Count;

        private readonly IReadOnlyDictionary<TKey, TOriginalValue> _dictionary;

        public ReadOnlyUpcastingDictionary(IReadOnlyDictionary<TKey, TOriginalValue> dictionary)
        {
            _dictionary = dictionary;
        }

        public TNewValue this[TKey key]
            => _dictionary[key];

        public bool ContainsKey(TKey key)
            => _dictionary.ContainsKey(key);

        public bool TryGetValue(TKey key, out TNewValue value)
        {
            if (_dictionary.TryGetValue(key, out var oldValue))
            {
                value = oldValue;
                return true;
            }

            value = default;
            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TNewValue>> GetEnumerator()
        {
            foreach (var kvp in _dictionary)
                yield return KeyValuePair.Create(kvp.Key, (TNewValue) kvp.Value);
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
