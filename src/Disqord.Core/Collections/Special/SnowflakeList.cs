using System;
using System.Collections;
using System.Collections.Generic;

namespace Disqord.Collections
{
    internal sealed class SnowflakeList : IList<Snowflake>, IReadOnlyList<Snowflake>
    {
        public int Count => _array.Length;

        public bool IsReadOnly => true;

        public Snowflake this[int index] => _array[index];

        private readonly ulong[] _array;

        public SnowflakeList(ulong[] array)
        {
            _array = array;
        }

        public bool Contains(Snowflake item)
            => Array.IndexOf(_array, item.RawValue) != -1;

        public void CopyTo(Snowflake[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (arrayIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            if (arrayIndex >= _array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            if (_array.Length - arrayIndex != array.Length - arrayIndex)
                throw new ArgumentException("The destination array is too small.");

            for (var i = arrayIndex; i < array.Length; i++)
                array[i] = _array[i];
        }

        public int IndexOf(Snowflake item)
            => Array.IndexOf(_array, item.RawValue);

        public IEnumerator<Snowflake> GetEnumerator()
        {
            for (var i = 0; i < _array.Length; i++)
                yield return _array[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        Snowflake IList<Snowflake>.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException();
        }

        public void Add(Snowflake item)
            => throw new NotSupportedException();

        public void Clear()
            => throw new NotSupportedException();

        public void Insert(int index, Snowflake item)
            => throw new NotSupportedException();

        public bool Remove(Snowflake item)
            => throw new NotSupportedException();

        public void RemoveAt(int index)
            => throw new NotSupportedException();
    }
}
