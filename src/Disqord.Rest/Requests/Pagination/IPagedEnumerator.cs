using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord.Rest.Pagination
{
    /// <summary>
    ///     Represents an asynchronous enumerator that handles REST request pagination.
    /// </summary>
    /// <typeparam name="TEntity"> The paged entity type. </typeparam>
    public interface IPagedEnumerator<TEntity> : IAsyncEnumerator<IReadOnlyList<TEntity>>
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
        ///     Gets the remaining amount of entities.
        /// </summary>
        int Remaining { get; }

        /// <summary>
        ///     Gets the default <see cref="IRestRequestOptions"/> for each request.
        /// </summary>
        IRestRequestOptions Options { get; }

        /// <summary>
        ///     Executes the next REST request and sets the next page.
        /// </summary>
        /// <param name="options"> The <see cref="IRestRequestOptions"/> to use for the next request. If not specified, will default to <see cref="Options"/>. </param>
        ValueTask<bool> MoveNextAsync(IRestRequestOptions options = null);

        ValueTask<bool> IAsyncEnumerator<IReadOnlyList<TEntity>>.MoveNextAsync()
            => MoveNextAsync();
    }
}
