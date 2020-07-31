using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Disqord.Collections
{
    internal sealed class ReadOnlyPredicateCollection<T> : IReadOnlyCollection<T>
    {
        public int Count => this.Count();

        private readonly ICollection<T> _collection;
        private readonly Predicate<T> _predicate;

        public ReadOnlyPredicateCollection(ICollection<T> collection, Predicate<T> predicate)
        {
            _collection = collection;
            _predicate = predicate;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var value in _collection)
            {
                if (_predicate(value))
                    yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
