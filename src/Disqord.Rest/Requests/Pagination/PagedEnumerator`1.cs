using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest;

/// <inheritdoc cref="PagedEnumerator{TPage,TEntity}"/>
/// <typeparam name="TEntity"> The type of the paged entities. </typeparam>
public abstract class PagedEnumerator<TEntity> : PagedEnumerator<IReadOnlyList<TEntity>, TEntity>
{
    protected PagedEnumerator(IRestClient client, int remaining, IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
        : base(client, remaining, options, cancellationToken)
    { }

    /// <summary>
    ///     Overridable logic that handles requesting the next page of entities.
    /// </summary>
    /// <param name="previousPage"> The previous page of entities. </param>
    /// <param name="options"> The request options for the page request. </param>
    /// <param name="cancellationToken"> The cancellation token to observe. </param>
    protected abstract Task<IReadOnlyList<TEntity>> NextPageCoreAsync(IReadOnlyList<TEntity>? previousPage,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default);

    /// <inheritdoc/>
    protected sealed override async Task<IReadOnlyList<TEntity>?> NextPageAsync(IReadOnlyList<TEntity>? previousPage,
        IRestRequestOptions? options = null, CancellationToken cancellationToken = default)
    {
        return await NextPageCoreAsync(previousPage, options, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    protected sealed override IReadOnlyList<TEntity> GetPageItems(IReadOnlyList<TEntity> page)
    {
        return page;
    }

    /// <inheritdoc/>
    protected sealed override int GetConsumedCount(IReadOnlyList<TEntity> page)
    {
        return page.Count;
    }
}