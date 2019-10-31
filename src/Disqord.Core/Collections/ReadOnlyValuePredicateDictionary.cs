using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Disqord.Collections
{
    internal sealed class ReadOnlyValuePredicateDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        public IEnumerable<TKey> Keys => _dictionary is IDictionary<TKey, TValue> dictionary
            ? new ReadOnlyPredicateCollection<TKey>(dictionary.Keys, ContainsKey)
            : this.Select(x => x.Key);

        public IEnumerable<TValue> Values => _dictionary is IDictionary<TKey, TValue> dictionary
            ? new ReadOnlyPredicateCollection<TValue>(dictionary.Values, _predicate)
            : this.Select(x => x.Value);

        public int Count => Values.Count();

        private readonly IReadOnlyDictionary<TKey, TValue> _dictionary;
        private readonly Predicate<TValue> _predicate;

        public ReadOnlyValuePredicateDictionary(IReadOnlyDictionary<TKey, TValue> dictionary, Predicate<TValue> predicate)
        {
            _dictionary = dictionary;
            _predicate = predicate;
        }

        public TValue this[TKey key] => TryGetValue(key, out var value)
            ? value
            : throw new KeyNotFoundException();

        public bool ContainsKey(TKey key)
            => TryGetValue(key, out var value) && _predicate(value);

        public bool TryGetValue(TKey key, out TValue value)
            => _dictionary.TryGetValue(key, out value) && _predicate(value);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var kvp in _dictionary)
            {
                if (_predicate(kvp.Value))
                    yield return kvp;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
