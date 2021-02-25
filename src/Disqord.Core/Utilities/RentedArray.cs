using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;

namespace Disqord.Utilities
{
// Unused, revisit if relevant
#nullable disable
    /// <summary>
    ///     Represents an array rented from an <see cref="ArrayPool{T}"/> that will be returned to it on disposal.
    ///     Does not perform any validation checks on the underlying array nor on the pool.
    /// </summary>
    /// <typeparam name="T"> The type of the elements in the array. </typeparam>
    public readonly struct RentedArray<T> : IList<T>, IReadOnlyList<T>, IDisposable
    {
        public int Length => _segment.Count;

        public T this[int index]
        {
            get => _segment[index];
            set => _segment[index] = value;
        }

        private readonly ArraySegment<T> _segment;
        private readonly ArrayPool<T> _pool;

        public RentedArray(ArraySegment<T> segment, ArrayPool<T> pool)
        {
            _segment = segment;
            _pool = pool;
        }

        public void CopyTo(T[] array)
            => _segment.CopyTo(array);

        public void CopyTo(Span<T> destination)
            => _segment.AsSpan().CopyTo(destination);

        public void CopyTo(T[] array, int arrayIndex)
            => _segment.CopyTo(array, arrayIndex);

        public ArraySegment<T>.Enumerator GetEnumerator()
            => _segment.GetEnumerator();

        public void Dispose()
        {
            _pool.Return(_segment.Array);
        }

        public static implicit operator ArraySegment<T>(RentedArray<T> value)
            => value._segment;

        public static implicit operator Memory<T>(RentedArray<T> value)
            => value._segment;

        public static implicit operator Span<T>(RentedArray<T> value)
            => value._segment;

        public static RentedArray<T> Rent(int length)
            => Rent(length, ArrayPool<T>.Shared);

        public static RentedArray<T> Rent(int length, ArrayPool<T> pool)
            => new RentedArray<T>(pool.Rent(length), pool);

        int ICollection<T>.Count => _segment.Count;
        int IReadOnlyCollection<T>.Count => _segment.Count;
        bool ICollection<T>.IsReadOnly => false;
        int IList<T>.IndexOf(T item) => throw new NotSupportedException();
        void IList<T>.Insert(int index, T item) => throw new NotSupportedException();
        void IList<T>.RemoveAt(int index) => throw new NotSupportedException();
        void ICollection<T>.Add(T item) => throw new NotSupportedException();
        void ICollection<T>.Clear() => throw new NotSupportedException();
        bool ICollection<T>.Contains(T item) => throw new NotSupportedException();
        bool ICollection<T>.Remove(T item) => throw new NotSupportedException();
        IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
