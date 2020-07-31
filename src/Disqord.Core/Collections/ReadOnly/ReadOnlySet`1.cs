using System;
using System.Collections;
using System.Collections.Generic;

namespace Disqord.Collections
{
    public sealed class ReadOnlySet<T> : ISet<T>, IReadOnlyCollection<T>
    {
        public static readonly ReadOnlySet<T> Empty = new ReadOnlySet<T>(new HashSet<T>(0));

        public int Count => _set.Count;

        private readonly ISet<T> _set;

        public ReadOnlySet(ISet<T> set)
        {
            if (set == null)
                throw new ArgumentNullException(nameof(set));

            _set = set;
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
            => _set.IsProperSubsetOf(other);

        public bool IsProperSupersetOf(IEnumerable<T> other)
            => _set.IsProperSupersetOf(other);

        public bool IsSubsetOf(IEnumerable<T> other)
            => _set.IsSubsetOf(other);

        public bool IsSupersetOf(IEnumerable<T> other)
            => _set.IsSupersetOf(other);

        public bool Overlaps(IEnumerable<T> other)
            => _set.Overlaps(other);

        public bool SetEquals(IEnumerable<T> other)
            => _set.SetEquals(other);

        public bool Contains(T item)
            => _set.Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
            => _set.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator()
            => _set.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        bool ICollection<T>.IsReadOnly => true;

        bool ISet<T>.Add(T item)
            => throw new NotSupportedException();

        void ISet<T>.ExceptWith(IEnumerable<T> other)
            => throw new NotSupportedException();

        void ISet<T>.IntersectWith(IEnumerable<T> other)
            => throw new NotSupportedException();

        void ISet<T>.SymmetricExceptWith(IEnumerable<T> other)
            => throw new NotSupportedException();

        void ISet<T>.UnionWith(IEnumerable<T> other)
            => throw new NotSupportedException();

        void ICollection<T>.Add(T item)
            => throw new NotSupportedException();

        void ICollection<T>.Clear()
            => throw new NotSupportedException();

        bool ICollection<T>.Remove(T item)
            => throw new NotSupportedException();
    }
}
