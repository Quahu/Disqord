using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Disqord.Collections
{
    /// <summary>
    ///     Represents a generic list of items almost identical to the <see cref="List{T}"/> except it exposes the underlying array containing the elements.
    /// </summary>
    /// <typeparam name="T"> The type of items. </typeparam>
    public sealed class FastList<T> : IList<T>, IReadOnlyList<T>
    {
        public T[] Items => _items;

        private T[] _items;
        private int _size;

        public FastList()
        {
            _items = Array.Empty<T>();
        }

        public FastList(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity));

            _items = capacity != 0
                ? new T[capacity]
                : Array.Empty<T>();
        }

        public FastList(IEnumerable<T> enumerable)
        {
            if (enumerable == null)
                throw new ArgumentNullException(nameof(enumerable));

            if (enumerable is ICollection<T> collection)
            {
                var count = collection.Count;
                if (count == 0)
                {
                    _items = Array.Empty<T>();
                }
                else
                {
                    _items = new T[count];
                    collection.CopyTo(_items, 0);
                    _size = count;
                }
            }
            else
            {
                _items = Array.Empty<T>();
                foreach (var item in enumerable)
                    Add(item);
            }
        }

        public int Capacity
        {
            get => _items.Length;
            set
            {
                if (value < _size)
                    throw new ArgumentOutOfRangeException(nameof(value));

                if (value == _items.Length)
                    return;

                if (value <= 0)
                {
                    _items = Array.Empty<T>();
                    return;
                }

                var newItems = new T[value];
                if (_size > 0)
                    Array.Copy(_items, newItems, _size);

                _items = newItems;
            }
        }

        public int Count => _size;

        bool ICollection<T>.IsReadOnly => false;

        public T this[int index]
        {
            get
            {
                // Following trick can reduce the range check by one
                if ((uint) index >= (uint) _size)
                    throw new ArgumentOutOfRangeException(nameof(index));

                return _items[index];
            }

            set
            {
                if ((uint) index >= (uint) _size)
                    throw new ArgumentOutOfRangeException(nameof(index));

                _items[index] = value;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            var array = _items;
            var size = _size;
            if ((uint) size < (uint) array.Length)
            {
                _size = size + 1;
                array[size] = item;
            }
            else
            {
                AddWithResize(item);
            }
        }

        // Non-inline from List.Add to improve its code quality as uncommon path
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void AddWithResize(T item)
        {
            var size = _size;
            EnsureCapacity(size + 1);
            _size = size + 1;
            _items[size] = item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear()
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                var size = _size;
                _size = 0;
                if (size > 0)
                    Array.Clear(_items, 0, size);
            }
            else
            {
                _size = 0;
            }
        }

        public bool Contains(T item)
            => _size != 0 && IndexOf(item) != -1;

        public void CopyTo(T[] array)
            => CopyTo(array, 0);

        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            if (_size - index < count)
                throw new ArgumentException();

            Array.Copy(_items, index, array, arrayIndex, count);
        }

        public void CopyTo(T[] array, int arrayIndex)
            => Array.Copy(_items, 0, array, arrayIndex, _size);

        private void EnsureCapacity(int min)
        {
            if (_items.Length >= min)
                return;

            Capacity = _items.Length != 0
                ? _items.Length * 2
                : 4;
        }

        public int IndexOf(T item)
            => Array.IndexOf(_items, item, 0, _size);

        public void Insert(int index, T item)
        {
            // Note that insertions at the end are legal.
            if ((uint) index > (uint) _size)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (_size == _items.Length)
                EnsureCapacity(_size + 1);

            if (index < _size)
                Array.Copy(_items, index, _items, index + 1, _size - index);

            _items[index] = item;
            _size++;
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            if ((uint) index >= (uint) _size)
                throw new ArgumentOutOfRangeException(nameof(index));

            _size--;
            if (index < _size)
                Array.Copy(_items, index + 1, _items, index, _size - index);

            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
                _items[_size] = default;
        }

        public T[] ToArray()
        {
            if (_size == 0)
                return Array.Empty<T>();

            var array = new T[_size];
            Array.Copy(_items, array, _size);
            return array;
        }

        public void TrimExcess()
        {
            var threshold = (int)(_items.Length * 0.9);
            if (_size < threshold)
            {
                Capacity = _size;
            }
        }

        public ArraySegment<T> AsSegment()
            => new ArraySegment<T>(_items, 0, _size);

        public Enumerator GetEnumerator()
            => new Enumerator(this);

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator()
            => new Enumerator(this);

        public struct Enumerator : IEnumerator<T>
        {
            public T Current => _current;

            object IEnumerator.Current => Current;

            private readonly FastList<T> _list;
            private int _index;
            private T _current;

            internal Enumerator(FastList<T> list)
            {
                _list = list;
                _index = 0;
                _current = default;
            }

            public bool MoveNext()
            {
                var localList = _list;
                if ((uint) _index < (uint) localList._size)
                {
                    _current = localList._items[_index];
                    _index++;
                    return true;
                }

                return MoveNextRare();
            }

            private bool MoveNextRare()
            {
                _index = _list._size + 1;
                _current = default;
                return false;
            }

            void IEnumerator.Reset()
            {
                _index = 0;
                _current = default;
            }

            public void Dispose()
            { }
        }
    }
}