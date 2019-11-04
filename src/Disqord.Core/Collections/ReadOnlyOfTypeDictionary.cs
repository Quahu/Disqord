using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Disqord.Collections
{
    internal sealed class ReadOnlyOfTypeDictionary<TKey, TOriginal, TNew> : IReadOnlyDictionary<TKey, TNew> where TNew : class, TOriginal
    {
        public IEnumerable<TKey> Keys => new ReadOnlyPredicateCollection<TKey>(_dictionary.Keys, ContainsKey);

        public IEnumerable<TNew> Values => new ReadOnlyOfTypeCollection<TOriginal, TNew>(_dictionary.Values);

        public int Count => Values.Count();

        private readonly IDictionary<TKey, TOriginal> _dictionary;

        public ReadOnlyOfTypeDictionary(IDictionary<TKey, TOriginal> dictionary)
        {
            _dictionary = dictionary;
        }

        public TNew this[TKey key] => TryGetValue(key, out var value)
            ? value
            : throw new KeyNotFoundException();

        public bool ContainsKey(TKey key)
            => TryGetValue(key, out var value) && value is TNew;

        public bool TryGetValue(TKey key, out TNew value)
        {
            if (_dictionary.TryGetValue(key, out var originalValue) && originalValue is TNew newValue)
            {
                value = newValue;
                return true;
            }

            value = default;
            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TNew>> GetEnumerator()
        {
            foreach (var kvp in _dictionary)
            {
                if (kvp.Value is TNew newValue)
                    yield return KeyValuePair.Create(kvp.Key, newValue);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
