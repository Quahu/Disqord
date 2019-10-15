using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Disqord.Collections
{
    internal readonly struct ReadOnlyUpcastingDictionary<TKey, TOriginalValue, TNewValue> : IReadOnlyDictionary<TKey, TNewValue>
        where TOriginalValue : TNewValue
    {
        public IEnumerable<TKey> Keys => _dictionary.Keys;

        public IEnumerable<TNewValue> Values => _dictionary.Values.Select(x => (TNewValue) x);

        public int Count => _dictionary.Count;

        private readonly IReadOnlyDictionary<TKey, TOriginalValue> _dictionary;

        public ReadOnlyUpcastingDictionary(IReadOnlyDictionary<TKey, TOriginalValue> dictionary)
            => _dictionary = dictionary;

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
                yield return new KeyValuePair<TKey, TNewValue>(kvp.Key, kvp.Value);
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
