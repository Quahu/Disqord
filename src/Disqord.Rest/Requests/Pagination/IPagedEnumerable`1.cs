using System.Collections.Generic;
using System.Threading;

namespace Disqord.Rest.Pagination;

/// <summary>
///     Represents an asynchronous enumerable that handles REST request pagination.
/// </summary>
/// <typeparam name="TEntity"> The type of the paged entities. </typeparam>
public interface IPagedEnumerable<out TEntity> : IAsyncEnumerable<IReadOnlyList<TEntity>>
{
    /// <summary>
    ///     Gets the paged enumerator.
    /// </summary>
    new IPagedEnumerator<TEntity> GetAsyncEnumerator(CancellationToken cancellationToken = default);

    IAsyncEnumerator<IReadOnlyList<TEntity>> IAsyncEnumerable<IReadOnlyList<TEntity>>.GetAsyncEnumerator(CancellationToken cancellationToken)
        => GetAsyncEnumerator(cancellationToken);
}