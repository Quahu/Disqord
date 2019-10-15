using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Disqord.Collections
{
    internal readonly struct ReadOnlyKeyPredicateDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        public IEnumerable<TKey> Keys
        {
            get
            {
                var predicate = _predicate;
                return _dictionary.Keys.Where(x => predicate(x));
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                var predicate = _predicate;
                return _dictionary.Where(x => predicate(x.Key)).Select(x => x.Value);
            }
        }

        public int Count => Keys.Count();

        private readonly IReadOnlyDictionary<TKey, TValue> _dictionary;
        private readonly Predicate<TKey> _predicate;

        public ReadOnlyKeyPredicateDictionary(IReadOnlyDictionary<TKey, TValue> dictionary, Predicate<TKey> predicate)
        {
            _dictionary = dictionary;
            _predicate = predicate;
        }

        public TValue this[TKey key] => TryGetValue(key, out var value)
            ? value
            : throw new KeyNotFoundException();

        public bool ContainsKey(TKey key)
            => _predicate(key) && _dictionary.ContainsKey(key);

        public bool TryGetValue(TKey key, out TValue value)
        {
            if (_predicate(key) && _dictionary.TryGetValue(key, out value))
                return true;

            value = default;
            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var kvp in _dictionary)
            {
                if (_predicate(kvp.Key))
                    yield return kvp;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
