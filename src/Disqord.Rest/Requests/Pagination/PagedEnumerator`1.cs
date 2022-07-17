using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Rest.Pagination;
using Qommon;

namespace Disqord.Rest;

/// <inheritdoc cref="IPagedEnumerator{TEntity}"/>
public abstract class PagedEnumerator<TEntity> : IPagedEnumerator<TEntity>
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
    ///     Overridable logic that handles requesting the next page of entities.
    /// </summary>
    /// <param name="previousPage"> The previous value of <see cref="Current"/>. </param>
    /// <param name="options"> The request options for the page request. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    protected abstract Task<IReadOnlyList<TEntity>> NextPageAsync(IReadOnlyList<TEntity>? previousPage,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    public async ValueTask<bool> MoveNextAsync(IRestRequestOptions? options = null)
    {
        if (RemainingCount == 0 || CancellationToken.IsCancellationRequested)
        {
            Current = default!;
            return false;
        }

        var current = await NextPageAsync(Current, options ?? Options, CancellationToken).ConfigureAwait(false);
        Current = current;
        if (current.Count == 0)
        {
            RemainingCount = 0;
            Current = default!;
            return false;
        }

        if (current.Count < PageSize)
        {
            // If Discord returns less entities than the page size,
            // it means there are no more entities beyond the ones we just received.
            RemainingCount = 0;
        }
        else
        {
            RemainingCount -= current.Count;
        }

        return true;
    }

    /// <inheritdoc/>
    public virtual ValueTask DisposeAsync()
        => default;
}