using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Disqord.Collections
{
    internal sealed class ReadOnlyValuePredicateArgumentDictionary<TKey, TValue, TArg> : IReadOnlyDictionary<TKey, TValue>
    {
        public IEnumerable<TKey> Keys => _dictionary is IDictionary<TKey, TValue> dictionary
            ? new ReadOnlyPredicateCollection<TKey>(dictionary.Keys, ContainsKey)
            : this.Select(x => x.Key);

        public IEnumerable<TValue> Values => _dictionary is IDictionary<TKey, TValue> dictionary
            ? new ReadOnlyPredicateArgumentCollection<TValue, TArg>(dictionary.Values, _predicate, _arg)
            : this.Select(x => x.Value);

        public int Count => Values.Count();

        private readonly IReadOnlyDictionary<TKey, TValue> _dictionary;
        private readonly Func<TValue, TArg, bool> _predicate;
        private readonly TArg _arg;

        public ReadOnlyValuePredicateArgumentDictionary(IReadOnlyDictionary<TKey, TValue> dictionary, Func<TValue, TArg, bool> predicate, TArg arg)
        {
            _dictionary = dictionary;
            _predicate = predicate;
            _arg = arg;
        }

        public TValue this[TKey key] => TryGetValue(key, out var value)
            ? value
            : throw new KeyNotFoundException();

        public bool ContainsKey(TKey key)
            => TryGetValue(key, out var value) && _predicate(value, _arg);

        public bool TryGetValue(TKey key, out TValue value)
            => _dictionary.TryGetValue(key, out value) && _predicate(value, _arg);

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var kvp in _dictionary)
            {
                if (_predicate(kvp.Value, _arg))
                    yield return kvp;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
