using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Collections;

namespace Disqord.Rest
{
    /// <summary>
    ///     Represents an asynchronously paged enumerable of <typeparamref name="T"/>s.
    ///     Does not support multiple enumerations.
    /// </summary>
    /// <typeparam name="T"> The <see cref="Type"/> of items in pages. </typeparam>
    public sealed class RestRequestEnumerable<T> : IAsyncEnumerable<IReadOnlyList<T>>
        where T : class
    {
        private readonly RestRequestEnumerator<T> _enumerator;

        internal RestRequestEnumerable(RestRequestEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        /// <summary>
        ///     Flattens this <see cref="RestRequestEnumerable{T}"/> into a single read-only list.
        /// </summary>
        /// <returns>
        ///     A flattened list of <typeparamref name="T"/>s.
        /// </returns>
        public async Task<IReadOnlyList<T>> FlattenAsync()
        {
            var list = new List<T>(_enumerator.PageSize);
            await foreach (var items in this.ConfigureAwait(false))
                list.AddRange(items);

            return list.ReadOnly();
        }

        public async Task<T> FirstOrDefaultAsync(Predicate<T> predicate)
        {
            await foreach (var items in this.ConfigureAwait(false))
            {
                for (var i = 0; i < items.Count; i++)
                {
                    var item = items[i];
                    if (predicate(item))
                        return item;
                }
            }

            return null;
        }

        public async Task<IReadOnlyList<T>> FirstOrDefaultPageAsync(Predicate<IReadOnlyList<T>> predicate)
        {
            await foreach (var items in this.ConfigureAwait(false))
            {
                if (predicate(items))
                    return items;
            }

            return null;
        }

        public RestRequestEnumerator<T> GetAsyncEnumerator()
            => _enumerator;

        IAsyncEnumerator<IReadOnlyList<T>> IAsyncEnumerable<IReadOnlyList<T>>.GetAsyncEnumerator(CancellationToken cancellationToken)
            => _enumerator;
    }
}
