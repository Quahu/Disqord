using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    internal delegate ValueTask<IReadOnlyList<T>> RestRequestPage<T>(IReadOnlyList<T> previousPage, RestRequestOptions options = null);

    public sealed class RestRequestEnumerator<T> : IAsyncEnumerator<IReadOnlyList<T>>
    {
        public IReadOnlyList<T> Current { get; private set; }

        private readonly ConcurrentQueue<RestRequestPage<T>> _queue;

        private readonly CancellationTokenSource _cts;

        private bool _isDisposed;

        internal RestRequestEnumerator()
        {
            _queue = new ConcurrentQueue<RestRequestPage<T>>();
            _cts = new CancellationTokenSource();
        }

        internal void Enqueue(RestRequestPage<T> page)
            => _queue.Enqueue(page);

        internal void Cancel()
        {
            if (_isDisposed)
                throw new ObjectDisposedException(null);

            _cts.Cancel();
        }

        public ValueTask<IReadOnlyList<T>> FlattenAsync(RestRequestOptions options = null)
            => FlattenAsync(CancellationToken.None, options);

        public async ValueTask<IReadOnlyList<T>> FlattenAsync(CancellationToken cancellationToken, RestRequestOptions options = null)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(null);

            if (cancellationToken.IsCancellationRequested || _cts.IsCancellationRequested)
                return ImmutableArray<T>.Empty;

            var builder = ImmutableArray.CreateBuilder<T>();
            while (!cancellationToken.IsCancellationRequested && !_cts.IsCancellationRequested
                && await MoveNextAsync(options).ConfigureAwait(false))
            {
                builder.AddRange(Current);
            }

            return builder.ToImmutable();
        }

        public async ValueTask<bool> MoveNextAsync(RestRequestOptions options = null)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(null);

            if (_cts.IsCancellationRequested || options?.CancellationToken?.IsCancellationRequested == true || !_queue.TryDequeue(out var page))
            {
                Current = default;
                return false;
            }

            Current = await page(Current, options).ConfigureAwait(false);
            return true;
        }

        ValueTask<bool> IAsyncEnumerator<IReadOnlyList<T>>.MoveNextAsync()
            => MoveNextAsync();

        public ValueTask DisposeAsync()
        {
            if (_isDisposed)
                return default;

            _isDisposed = true;
            _cts.Dispose();
            return default;
        }
    }
}
