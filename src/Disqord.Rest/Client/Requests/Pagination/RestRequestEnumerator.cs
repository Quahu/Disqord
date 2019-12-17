using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    internal delegate ValueTask<IReadOnlyList<T>> RestRequestPage<T>(IReadOnlyList<T> previousPage, RestRequestOptions options = null);

    public abstract class RestRequestEnumerator<T> : IAsyncEnumerator<IReadOnlyList<T>>
    {
        public IReadOnlyList<T> Current { get; private set; }

        protected readonly RestDiscordClient Client;

        private readonly int _pageSize;

        protected int Remaining { get; private set; }

        private bool _isDisposed;

        internal RestRequestEnumerator(RestDiscordClient client, int pageSize, int remaining)
        {
            Client = client;
            _pageSize = pageSize;
            Remaining = remaining;
        }

        protected abstract Task<IReadOnlyList<T>> NextPageAsync(IReadOnlyList<T> previous, RestRequestOptions options = null);

        public async ValueTask<bool> MoveNextAsync(RestRequestOptions options = null)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(null);

            if (Remaining == 0)
            {
                Current = default;
                return false;
            }

            Current = await NextPageAsync(Current, options).ConfigureAwait(false);
            if (Current.Count == 0)
            {
                Remaining = 0;
                Current = default;
                return false;
            }

            if (Current.Count != _pageSize)
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
