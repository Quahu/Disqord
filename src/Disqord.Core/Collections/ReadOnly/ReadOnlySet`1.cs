using System;
using System.Collections;
using System.Collections.Generic;

namespace Disqord.Collections
{
    public sealed class ReadOnlySet<T> : ISet<T>, IReadOnlySet<T>
    {
        public static readonly IReadOnlySet<T> Empty = new HashSet<T>(0).ReadOnly();

        public int Count => _set.Count;

        private readonly ISet<T> _set;

        public ReadOnlySet(ISet<T> set)
        {
            _set = set ?? throw new ArgumentNullException(nameof(set));
        }

        /// <inheritdoc/>
        public bool IsProperSubsetOf(IEnumerable<T> other)
            => _set.IsProperSubsetOf(other);

        /// <inheritdoc/>
        public bool IsProperSupersetOf(IEnumerable<T> other)
            => _set.IsProperSupersetOf(other);

        /// <inheritdoc/>
        public bool IsSubsetOf(IEnumerable<T> other)
            => _set.IsSubsetOf(other);

        /// <inheritdoc/>
        public bool IsSupersetOf(IEnumerable<T> other)
            => _set.IsSupersetOf(other);

        /// <inheritdoc/>
        public bool Overlaps(IEnumerable<T> other)
            => _set.Overlaps(other);

        /// <inheritdoc/>
        public bool SetEquals(IEnumerable<T> other)
            => _set.SetEquals(other);

        /// <inheritdoc/>
        public bool Contains(T item)
            => _set.Contains(item);

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex)
            => _set.CopyTo(array, arrayIndex);

        /// <inheritdoc/>
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
