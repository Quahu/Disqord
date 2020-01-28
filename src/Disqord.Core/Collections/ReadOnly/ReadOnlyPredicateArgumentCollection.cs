using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Disqord.Collections
{
    internal sealed class ReadOnlyPredicateArgumentCollection<T, TArg> : IReadOnlyCollection<T>
    {
        public int Count => this.Count();

        private readonly ICollection<T> _collection;
        private readonly Func<T, TArg, bool> _predicate;
        private readonly TArg _arg;

        public ReadOnlyPredicateArgumentCollection(ICollection<T> collection, Func<T, TArg, bool> predicate, TArg arg)
        {
            _collection = collection;
            _predicate = predicate;
            _arg = arg;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var value in _collection)
            {
                if (_predicate(value, _arg))
                    yield return value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
