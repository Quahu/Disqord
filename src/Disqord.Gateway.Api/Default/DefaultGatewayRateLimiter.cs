using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Collections.Synchronized;
using Disqord.Gateway.Api.Models;
using Disqord.Utilities.Binding;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Gateway.Api.Default
{
    public class DefaultGatewayRateLimiter : IGatewayRateLimiter
    {
        // The buckets that are shared between all gateway connections.
        private static readonly ISynchronizedDictionary<Snowflake, ISynchronizedDictionary<GatewayPayloadOperation, Bucket>> _sharedBuckets;

        private const int HEARTBEATS = 2;

        /// <inheritdoc/>
        public ILogger Logger { get; }

        /// <inheritdoc/>
        public IGatewayApiClient ApiClient => _binder.Value;

        private readonly Binder<IGatewayApiClient> _binder;
        private readonly Bucket _masterBucket;
        private readonly Dictionary<GatewayPayloadOperation, Bucket> _buckets;

        public DefaultGatewayRateLimiter(
            IOptions<DefaultGatewayRateLimiterConfiguration> options,
            ILogger<DefaultGatewayRateLimiter> logger)
        {
            Logger = logger;

            _binder = new Binder<IGatewayApiClient>(this, x =>
            {
                if (x.Token is not BotToken)
                    throw new ArgumentException("The default gateway rate-limiter supports only bot tokens.");
            });

            _masterBucket = new Bucket(120 - HEARTBEATS, TimeSpan.FromSeconds(60));
            _buckets = new Dictionary<GatewayPayloadOperation, Bucket>(2);
        }

        public void Bind(IGatewayApiClient apiClient)
        {
            _binder.Bind(apiClient);
            // TODO: identify concurrency
            _buckets[GatewayPayloadOperation.Identify] = GetSharedBucket(ApiClient.Token as BotToken, GatewayPayloadOperation.Identify, 1, TimeSpan.FromSeconds(5));
            _buckets[GatewayPayloadOperation.UpdatePresence] = new Bucket(5, TimeSpan.FromSeconds(60));
        }

        /// <inheritdoc/>
        public bool IsRateLimited(GatewayPayloadOperation? operation = null)
        {
            if (operation != null)
            {
                if (operation.Value == GatewayPayloadOperation.Heartbeat)
                    return false;

                var bucket = _buckets.GetValueOrDefault(operation.Value);
                if (bucket?.CurrentCount == 0)
                    return true;
            }

            return _masterBucket.CurrentCount == 0;
        }

        /// <inheritdoc/>
        public async Task WaitAsync(GatewayPayloadOperation? operation = null, CancellationToken cancellationToken = default)
        {
            if (operation != null)
            {
                if (operation.Value == GatewayPayloadOperation.Heartbeat)
                    return;

                var bucket = _buckets.GetValueOrDefault(operation.Value);
                if (bucket != null)
                    await bucket.WaitAsync(cancellationToken).ConfigureAwait(false);
            }

            await _masterBucket.WaitAsync(cancellationToken).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public void Release(GatewayPayloadOperation? operation = null)
        {
            if (operation != null)
            {
                if (operation.Value == GatewayPayloadOperation.Heartbeat)
                    return;

                var bucket = _buckets.GetValueOrDefault(operation.Value);
                bucket?.Release();
            }

            _masterBucket.Release();
        }

        /// <inheritdoc/>
        public virtual void Dispose()
        { }

        private static Bucket GetSharedBucket(BotToken token, GatewayPayloadOperation operation, int uses, TimeSpan resetDelay)
        {
            var dictionary = _sharedBuckets.GetOrAdd(token.Id, _ => new SynchronizedDictionary<GatewayPayloadOperation, Bucket>(1));
            var bucket = dictionary.GetOrAdd(operation, (_, tuple) =>
            {
                var (uses, resetDelay) = tuple;
                return new Bucket(uses, resetDelay);
            }, (uses, resetDelay));
            return bucket;
        }

        static DefaultGatewayRateLimiter()
        {
            _sharedBuckets = new SynchronizedDictionary<Snowflake, ISynchronizedDictionary<GatewayPayloadOperation, Bucket>>(1);
        }

        private class Bucket
        {
            public int CurrentCount
            {
                get
                {
                    lock (_semaphore)
                    {
                        return _semaphore.CurrentCount;
                    }
                }
            }

            private readonly BetterSemaphoreSlim _semaphore;
            private readonly TimeSpan _resetDelay;
            private bool _isResetting;

            public Bucket(int uses, TimeSpan resetDelay)
            {
                _semaphore = new BetterSemaphoreSlim(uses, uses);
                _resetDelay = resetDelay;
            }

            public Task WaitAsync(CancellationToken cancellationToken)
            {
                lock (_semaphore)
                {
                    var task = _semaphore.WaitAsync(cancellationToken);
                    if (!_isResetting)
                    {
                        _isResetting = true;
                        _ = ResetAsync();
                    }

                    return task;
                }
            }

            public void Release()
            {
                lock (_semaphore)
                {
                    try
                    {
                        _semaphore.Release();
                    }
                    catch { }
                }
            }

            private async Task ResetAsync()
            {
                await Task.Delay(_resetDelay).ConfigureAwait(false);
                lock (_semaphore)
                {
                    _semaphore.Release(_semaphore.MaximumCount - _semaphore.CurrentCount);
                    _isResetting = false;
                }
            }
        }
    }
}
