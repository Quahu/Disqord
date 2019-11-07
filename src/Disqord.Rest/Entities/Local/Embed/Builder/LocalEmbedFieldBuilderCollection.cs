using System;
using System.Collections;
using System.Collections.Generic;

namespace Disqord
{
    /// <summary>
    ///     Represents a collection of <see cref="LocalEmbedFieldBuilder"/>s.
    ///     This class is not thread-safe.
    /// </summary>
    /// <remarks>
    ///     Explicitly implemented collection members might not be supported.
    /// </remarks>
    public sealed class LocalEmbedFieldBuilderCollection : IList<LocalEmbedFieldBuilder>, IReadOnlyList<LocalEmbedFieldBuilder>
    {
        public int Count => _list.Count;

        public LocalEmbedFieldBuilder this[int index]
        {
            get => _list[index];
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                _list[index] = value;
            }
        }

        private readonly List<LocalEmbedFieldBuilder> _list;

        bool ICollection<LocalEmbedFieldBuilder>.IsReadOnly => false;

        internal LocalEmbedFieldBuilderCollection()
        {
            _list = new List<LocalEmbedFieldBuilder>();
        }


        /// <exception cref="ArgumentNullException"></exception>
        public void Add(LocalEmbedFieldBuilder field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            if (_list.Count == LocalEmbedBuilder.MAX_FIELDS_AMOUNT)
                throw new InvalidOperationException($"The embed builder must not contain more than {LocalEmbedBuilder.MAX_FIELDS_AMOUNT} fields.");

            _list.Add(field);
        }

        public void Clear()
            => _list.Clear();

        /// <exception cref="ArgumentNullException"></exception>
        public void Insert(int index, LocalEmbedFieldBuilder field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            if (_list.Count == LocalEmbedBuilder.MAX_FIELDS_AMOUNT)
                throw new InvalidOperationException($"The embed builder must not contain more than {LocalEmbedBuilder.MAX_FIELDS_AMOUNT} fields.");

            _list.Insert(index, field);
        }

        /// <exception cref="ArgumentNullException"></exception>
        public bool Remove(LocalEmbedFieldBuilder field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            return _list.Remove(field);
        }

        public void RemoveAt(int index)
            => _list.RemoveAt(index);

        public IEnumerator<LocalEmbedFieldBuilder> GetEnumerator()
            => _list.GetEnumerator();

        int IList<LocalEmbedFieldBuilder>.IndexOf(LocalEmbedFieldBuilder item)
            => throw new NotSupportedException();

        bool ICollection<LocalEmbedFieldBuilder>.Contains(LocalEmbedFieldBuilder item)
            => throw new NotSupportedException();

        void ICollection<LocalEmbedFieldBuilder>.CopyTo(LocalEmbedFieldBuilder[] array, int arrayIndex)
            => throw new NotSupportedException();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
