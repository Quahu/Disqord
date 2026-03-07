using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Pagination;
using Qommon;

namespace Disqord.Rest;

/// <summary>
///     Represents a paged enumerator that separates the page response type from the entity type.
/// </summary>
/// <typeparam name="TPage"> The type of the page response from the API. </typeparam>
/// <typeparam name="TEntity"> The type of entities extracted from each page. </typeparam>
public abstract class PagedEnumerator<TPage, TEntity> : IPagedEnumerator<TEntity>
    where TPage : class
{
    /// <inheritdoc/>
    public abstract int PageSize { get; }

    /// <summary>
    ///     Gets the current page of entities.
    /// </summary>
    public IReadOnlyList<TEntity> Current { get; private set; } = null!;

    /// <inheritdoc/>
    public IRestClient Client { get; }

    /// <inheritdoc/>
    public int RemainingCount { get; protected set; }

    /// <inheritdoc/>
    public IRestRequestOptions? Options { get; }

    /// <inheritdoc/>
    public CancellationToken CancellationToken { get; }

    /// <summary>
    ///     Gets the current page response.
    /// </summary>
    protected TPage? CurrentPage { get; private set; }

    protected PagedEnumerator(IRestClient client, int remaining, IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        Guard.IsNotNull(client);
        Guard.IsGreaterThanOrEqualTo(remaining, 0);

        Client = client;
        RemainingCount = remaining;
        Options = options;
        CancellationToken = cancellationToken;
    }

    /// <summary>
    ///     Gets the amount of entities in the next request page.
    /// </summary>
    protected int NextPageSize
    {
        get
        {
            var pageSize = PageSize;
            var remaining = RemainingCount;
            return remaining > pageSize
                ? pageSize
                : remaining;
        }
    }

    /// <summary>
    ///     Fetches the next page response from the API.
    /// </summary>
    /// <param name="previousPage"> The previous page response. </param>
    /// <param name="options"> The request options for the page request. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    protected abstract Task<TPage?> NextPageAsync(TPage? previousPage,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Extracts the entities from a page response.
    /// </summary>
    /// <param name="page"> The page response to extract entities from. </param>
    protected abstract IReadOnlyList<TEntity> GetPageItems(TPage page);

    /// <summary>
    ///     Gets the number of items consumed by a page, used for progress tracking.
    /// </summary>
    /// <param name="page"> The page response. </param>
    protected abstract int GetConsumedCount(TPage page);

    /// <inheritdoc/>
    public async ValueTask<bool> MoveNextAsync(IRestRequestOptions? options = null)
    {
        if (RemainingCount == 0 || CancellationToken.IsCancellationRequested)
        {
            Current = default!;
            CurrentPage = null;
            return false;
        }

        var page = await NextPageAsync(CurrentPage, options ?? Options, CancellationToken).ConfigureAwait(false);
        if (page == null)
        {
            RemainingCount = 0;
            Current = default!;
            CurrentPage = null;
            return false;
        }

        CurrentPage = page;
        var items = GetPageItems(page);
        Current = items;

        if (items.Count == 0)
        {
            RemainingCount = 0;
            Current = default!;
            CurrentPage = null;
            return false;
        }

        var consumed = GetConsumedCount(page);
        if (consumed < PageSize)
        {
            // If the consumed count is less than the page size,
            // it means there are no more items beyond the ones we just received.
            RemainingCount = 0;
        }
        else
        {
            RemainingCount -= consumed;
        }

        return true;
    }

    /// <inheritdoc/>
    public virtual ValueTask DisposeAsync()
        => default;
}
