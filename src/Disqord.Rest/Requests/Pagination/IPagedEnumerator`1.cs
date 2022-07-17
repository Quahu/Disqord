using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest.Pagination;

/// <summary>
///     Represents an asynchronous enumerator that handles REST request pagination.
/// </summary>
/// <typeparam name="TEntity"> The type of the paged entities. </typeparam>
public interface IPagedEnumerator<out TEntity> : IAsyncEnumerator<IReadOnlyList<TEntity>>
{
    /// <summary>
    ///     Gets the constant page size (per-request entity count).
    /// </summary>
    int PageSize { get; }

    /// <summary>
    ///     Gets the client of this enumerator.
    /// </summary>
    IRestClient Client { get; }

    /// <summary>
    ///     Gets the amount of remaining entities.
    /// </summary>
    int RemainingCount { get; }

    /// <summary>
    ///     Gets the default request options for each request.
    /// </summary>
    IRestRequestOptions? Options { get; }

    /// <summary>
    ///     Gets the cancellation token passed when requesting this enumerator from the enumerable.
    /// </summary>
    CancellationToken CancellationToken { get; }

    /// <summary>
    ///     Executes the next REST request and sets the next page.
    /// </summary>
    /// <param name="options"> The <see cref="IRestRequestOptions"/> to use for the next request. If not specified, will default to <see cref="Options"/>. </param>
    ValueTask<bool> MoveNextAsync(IRestRequestOptions? options = null);

    ValueTask<bool> IAsyncEnumerator<IReadOnlyList<TEntity>>.MoveNextAsync()
        => MoveNextAsync();
}