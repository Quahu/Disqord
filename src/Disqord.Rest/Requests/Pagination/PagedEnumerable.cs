using System;
using Disqord.Rest.Pagination;

namespace Disqord.Rest
{
    /// <inheritdoc cref="IPagedEnumerable{TEntity}"/>
    public class PagedEnumerable<TEntity> : IPagedEnumerable<TEntity>
    {
        private readonly IPagedEnumerator<TEntity> _enumerator;

        public PagedEnumerable(IPagedEnumerator<TEntity> enumerator)
        {
            if (enumerator == null)
                throw new ArgumentNullException(nameof(enumerator));

            _enumerator = enumerator;
        }

        /// <inheritdoc/>
        public IPagedEnumerator<TEntity> GetAsyncEnumerator()
            => _enumerator;
    }
}
