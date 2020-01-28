using System;
using System.Collections;
using System.Collections.Generic;

namespace Disqord.Collections
{
    internal sealed class ReadOnlyList<T> : IList<T>, IReadOnlyList<T>
    {
        public static IReadOnlyList<T> Empty => Array.Empty<T>().ReadOnly();

        public int Count => _list.Count;

        public T this[int index] => _list[index];

        private readonly IList<T> _list;

        internal ReadOnlyList(IList<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            _list = list;
        }

        public int IndexOf(T item)
            => _list.IndexOf(item);

        public bool Contains(T item)
            => _list.Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
            => _list.CopyTo(array, arrayIndex);

        public IEnumerator<T> GetEnumerator()
            => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        bool ICollection<T>.IsReadOnly => true;

        T IList<T>.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException();
        }

        void IList<T>.Insert(int index, T item)
            => throw new NotSupportedException();

        void IList<T>.RemoveAt(int index)
            => throw new NotSupportedException();

        void ICollection<T>.Add(T item)
            => throw new NotSupportedException();

        void ICollection<T>.Clear()
            => throw new NotSupportedException();

        bool ICollection<T>.Remove(T item)
            => throw new NotSupportedException();
    }
}
