using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    public abstract class RestRequestEnumerator<T> : IAsyncEnumerator<IReadOnlyList<T>>
    {
        public IReadOnlyList<T> Current { get; private set; }

        public int PageSize { get; }

        protected readonly RestDiscordClient Client;

        protected int Remaining { get; private set; }

        private readonly RestRequestOptions _options;

        private bool _isDisposed;

        internal RestRequestEnumerator(RestDiscordClient client, int pageSize, int remaining, RestRequestOptions options)
        {
            Client = client;
            PageSize = pageSize;
            Remaining = remaining;
            _options = options;
        }

        protected abstract Task<IReadOnlyList<T>> NextPageAsync(IReadOnlyList<T> previous, RestRequestOptions options);

        public async ValueTask<bool> MoveNextAsync(RestRequestOptions options = null)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(null);

            if (Remaining == 0)
            {
                Current = default;
                return false;
            }

            Current = await NextPageAsync(Current, options ?? _options).ConfigureAwait(false);
            if (Current.Count == 0)
            {
                Remaining = 0;
                Current = default;
                return false;
            }

            if (Current.Count != PageSize)
            {
                Remaining = 0;
            }
            else
            {
                Remaining -= Current.Count;
            }

            return true;
        }

        ValueTask<bool> IAsyncEnumerator<IReadOnlyList<T>>.MoveNextAsync()
            => MoveNextAsync();

        public ValueTask DisposeAsync()
        {
            if (_isDisposed)
                return default;

            _isDisposed = true;
            return default;
        }
    }
}
