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

        internal LocalEmbedFieldBuilderCollection(IEnumerable<LocalEmbedFieldBuilder> fields)
        {
            _list = new List<LocalEmbedFieldBuilder>(fields);
        }

        /// <inheritdoc cref="List{T}.Add(T)"/>
        /// <exception cref="ArgumentNullException"></exception>
        public void Add(LocalEmbedFieldBuilder field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            if (_list.Count == LocalEmbedBuilder.MAX_FIELDS_AMOUNT)
                throw new InvalidOperationException($"The embed builder must not contain more than {LocalEmbedBuilder.MAX_FIELDS_AMOUNT} fields.");

            _list.Add(field);
        }

        /// <inheritdoc cref="List{T}.Clear()"/>
        public void Clear()
            => _list.Clear();

        /// <inheritdoc cref="List{T}.Insert(int, T)"/>
        /// <exception cref="ArgumentNullException"></exception>
        public void Insert(int index, LocalEmbedFieldBuilder field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            if (_list.Count == LocalEmbedBuilder.MAX_FIELDS_AMOUNT)
                throw new InvalidOperationException($"The embed builder must not contain more than {LocalEmbedBuilder.MAX_FIELDS_AMOUNT} fields.");

            _list.Insert(index, field);
        }

        /// <inheritdoc cref="List{T}.Remove(T)"/>
        /// <exception cref="ArgumentNullException"></exception>
        public bool Remove(LocalEmbedFieldBuilder field)
        {
            if (field == null)
                throw new ArgumentNullException(nameof(field));

            return _list.Remove(field);
        }

        /// <inheritdoc cref="List{T}.RemoveAll(Predicate{T})"/>
        public int RemoveAll(Predicate<LocalEmbedFieldBuilder> predicate)
            => _list.RemoveAll(predicate);

        /// <inheritdoc cref="List{T}.RemoveAt(int)"/>
        public void RemoveAt(int index)
            => _list.RemoveAt(index);

        /// <inheritdoc cref="List{T}.Sort(Comparison{T})"/>
        public void Sort(Comparison<LocalEmbedFieldBuilder> comparison)
            => _list.Sort(comparison);

        /// <inheritdoc cref="List{T}.Exists(Predicate{T})"/>
        public bool Exists(Predicate<LocalEmbedFieldBuilder> predicate)
            => _list.Exists(predicate);

        /// <inheritdoc cref="List{T}.GetEnumerator()"/>
        public IEnumerator<LocalEmbedFieldBuilder> GetEnumerator()
            => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        void ICollection<LocalEmbedFieldBuilder>.CopyTo(LocalEmbedFieldBuilder[] array, int arrayIndex)
            => _list.CopyTo(array, arrayIndex);

        int IList<LocalEmbedFieldBuilder>.IndexOf(LocalEmbedFieldBuilder item)
            => throw new NotSupportedException();

        bool ICollection<LocalEmbedFieldBuilder>.Contains(LocalEmbedFieldBuilder item)
            => throw new NotSupportedException();
    }
}
