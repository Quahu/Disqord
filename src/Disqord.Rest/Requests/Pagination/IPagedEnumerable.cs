using System.Collections.Generic;
using System.Threading;

namespace Disqord.Rest.Pagination
{
    /// <summary>
    ///     Represents an asynchronous enumerable that handles REST request pagination.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IPagedEnumerable<TEntity> : IAsyncEnumerable<IReadOnlyList<TEntity>>
    {
        /// <summary>
        ///     Gets the paged enumerator.
        /// </summary>
        IPagedEnumerator<TEntity> GetAsyncEnumerator();

        IAsyncEnumerator<IReadOnlyList<TEntity>> IAsyncEnumerable<IReadOnlyList<TEntity>>.GetAsyncEnumerator(CancellationToken cancellationToken)
            => GetAsyncEnumerator();
    }
}
