using System;
using System.Collections;
using System.Collections.Generic;

namespace Disqord.Collections
{
    /// <summary>
    ///     Represents a first and last automatically pops out when the capacity is reached collection.
    /// </summary>
    /// <typeparam name="T"> The <see cref="Type"/> of the elements. </typeparam>
    internal sealed class CircularBuffer<T> : IReadOnlyList<T>, ICollection<T> where T : class
    {
        /// <summary>
        ///     Gets the current amount of items this <see cref="CircularBuffer{T}"/>.
        /// </summary>
        public int Count
        {
            get
            {
                lock (_lock)
                {
                    return _buffer.Count;
                }
            }
        }

        /// <summary>
        ///     Gets whether this <see cref="CircularBuffer{T}"/> is read-only. Always <see langword="false"/>.
        /// </summary>
        public bool IsReadOnly => false;

        /// <summary>
        ///     Gets the capacity of this <see cref="CircularBuffer{T}"/>.
        /// </summary>
        public int Capacity { get; }

        private readonly List<T> _buffer;

        private readonly object _lock = new object();

        /// <summary>
        ///     Initialises a new <see cref="CircularBuffer{T}"/> with the specified capacity.
        /// </summary>
        /// <param name="capacity"> The capacity to use. </param>
        public CircularBuffer(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Capacity must be a positive integer.");

            Capacity = capacity;
            _buffer = new List<T>(capacity);
        }

        /// <summary>
        ///     Gets the element at the specified index.
        /// </summary>
        /// <param name="index"> The zero-based index of the element to get. </param>
        /// <returns>
        ///     The element at the specified index.
        /// </returns>
        public T this[int index]
        {
            get
            {
                lock (_lock)
                {
                    return _buffer[index];
                }
            }
        }

        public void Add(T item)
        {
            lock (_lock)
            {
                if (Count + 1 == Capacity)
                    _buffer.RemoveAt(Count - 1);

                _buffer.Insert(0, item);
            }
        }

        public bool Remove(T item)
        {
            lock (_lock)
            {
                return _buffer.Remove(item);
            }
        }

        public bool TryRemove(Predicate<T> predicate, out T item)
        {
            lock (_lock)
            {
                item = _buffer.Find(predicate);
                if (item != null)
                {
                    _buffer.Remove(item);
                    return true;
                }
                else
                {
                    item = null;
                    return false;
                }
            }
        }

        public T Find(Predicate<T> predicate)
        {
            lock (_lock)
            {
                return _buffer.Find(predicate);
            }
        }

        /// <summary>
        ///     Removes all elements from this <see cref="CircularBuffer{T}"/>.
        /// </summary>
        public void Clear()
        {
            lock (_lock)
            {
                _buffer.Clear();
            }
        }

        /// <summary>
        ///     Determines whether an element is this <see cref="CircularBuffer{T}"/>.
        /// </summary>
        /// <param name="item"> The element to check for. </param>
        /// <returns>
        ///     <see langword="true"/> if the value was found, otherwise <see langword="false"/>.
        /// </returns>
        public bool Contains(T item)
        {
            lock (_lock)
            {
                return _buffer.Contains(item);
            }
        }

        /// <summary>
        ///     Copies this <see cref="CircularBuffer{T}"/> to a compatible one-dimensional array,
        ///     starting at the specified index the target array.
        /// </summary>
        /// <param name="array"> The array to copy to. </param>
        /// <param name="arrayIndex"> The zero-based index the array at which copying begins. </param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (_lock)
            {
                _buffer.CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a copy of this <see cref="CircularBuffer{T}"/>.
        /// </summary>
        /// <returns>
        ///     An enumerator.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            IEnumerable<T> enumerable;
            lock (_lock)
            {
                enumerable = _buffer.ToArray();
            }
            return enumerable.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
