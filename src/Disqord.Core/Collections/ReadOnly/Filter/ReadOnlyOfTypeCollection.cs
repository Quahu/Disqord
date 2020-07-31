using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Disqord.Collections
{
    internal sealed class ReadOnlyOfTypeCollection<TOriginal, TNew> : IReadOnlyCollection<TNew> where TNew : class, TOriginal
    {
        public int Count => this.Count();

        private readonly ICollection<TOriginal> _collection;

        public ReadOnlyOfTypeCollection(ICollection<TOriginal> collection)
        {
            _collection = collection;
        }

        public IEnumerator<TNew> GetEnumerator()
        {
            foreach (var value in _collection)
            {
                if (value is TNew newValue)
                    yield return newValue;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
