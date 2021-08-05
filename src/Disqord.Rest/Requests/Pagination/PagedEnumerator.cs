using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Disqord.Rest.Pagination;

namespace Disqord.Rest
{
    /// <inheritdoc cref="IPagedEnumerator{TEntity}"/>
    public abstract class PagedEnumerator<TEntity> : IPagedEnumerator<TEntity>
    {
        /// <inheritdoc/>
        public abstract int PageSize { get; }

        /// <summary>
        ///     Gets the current page of entities.
        /// </summary>
        public IReadOnlyList<TEntity> Current { get; private set; }

        /// <inheritdoc/>
        public IRestClient Client { get; }

        /// <inheritdoc/>
        public int Remaining { get; protected set; }

        /// <inheritdoc/>
        public IRestRequestOptions Options { get; }

        protected PagedEnumerator(IRestClient client, int remaining, IRestRequestOptions options = null)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            Client = client;
            Remaining = remaining;
            Options = options;
        }

        /// <summary>
        ///     Gets the amount of entities in the next request page.
        /// </summary>
        protected int NextAmount => Remaining > PageSize
            ? PageSize
            : Remaining;

        /// <summary>
        ///     Overridable logic that handles requesting the next page of entities.
        /// </summary>
        /// <param name="previousPage"> The previous value of <see cref="Current"/>. </param>
        /// <param name="options"> The <see cref="IRestRequestOptions"/> for the next page request. </param>
        protected abstract Task<IReadOnlyList<TEntity>> NextPageAsync(IReadOnlyList<TEntity> previousPage, IRestRequestOptions options = null);

        /// <inheritdoc/>
        public async ValueTask<bool> MoveNextAsync(IRestRequestOptions options = null)
        {
            if (Remaining == 0)
            {
                Current = default;
                return false;
            }

            Current = await NextPageAsync(Current, options ?? Options).ConfigureAwait(false);
            if (Current.Count == 0)
            {
                Remaining = 0;
                Current = default;
                return false;
            }

            if (Current.Count < PageSize)
            {
                // If Discord returns less entities than the page size,
                // it means there are no more entities beyond the ones we just received.
                Remaining = 0;
            }
            else
            {
                Remaining -= Current.Count;
            }

            return true;
        }

        /// <inheritdoc/>
        public virtual ValueTask DisposeAsync()
            => default;
    }
}
