using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qommon.Binding;

namespace Disqord.Gateway.Api.Default;

public class DefaultGatewayRateLimiter : IGatewayRateLimiter
{
    private const int HeartbeatCount = 2;

    /// <inheritdoc/>
    public ILogger Logger => Shard.Logger;

    /// <inheritdoc/>
    public IShard Shard => _binder.Value;

    private readonly ILoggerFactory _loggerFactory;

    private readonly Binder<IShard> _binder;

    private readonly Bucket _masterBucket;
    private readonly Dictionary<GatewayPayloadOperation, Bucket> _buckets;

    public DefaultGatewayRateLimiter(
        IOptions<DefaultGatewayRateLimiterConfiguration> options,
        ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;

        _binder = new Binder<IShard>(this);

        _masterBucket = new Bucket(_loggerFactory.CreateLogger("Master Bucket"), 120 - HeartbeatCount, TimeSpan.FromSeconds(60));
        _buckets = new Dictionary<GatewayPayloadOperation, Bucket>(2);
    }

    public void Bind(IShard apiClient)
    {
        _binder.Bind(apiClient);

        _buckets[GatewayPayloadOperation.UpdatePresence] = new Bucket(_loggerFactory.CreateLogger("Presence Bucket"), 5, TimeSpan.FromSeconds(60));
    }

    /// <inheritdoc/>
    public bool IsRateLimited(GatewayPayloadOperation? operation = null)
    {
        if (operation != null)
        {
            var bucket = _buckets.GetValueOrDefault(operation.Value);
            if (bucket?.CurrentCount == 0)
                return true;
        }

        return _masterBucket.CurrentCount == 0;
    }

    /// <inheritdoc/>
    public int GetRemainingRequests(GatewayPayloadOperation? operation = null)
    {
        if (operation != null)
        {
            var bucket = _buckets.GetValueOrDefault(operation.Value);
            if (bucket != null)
                return bucket.CurrentCount;
        }

        return _masterBucket.CurrentCount;
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

            // TODO verify: identify payloads don't count towards the master limit.
            if (operation.Value == GatewayPayloadOperation.Identify)
                return;
        }

        await _masterBucket.WaitAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public void NotifyCompletion(GatewayPayloadOperation? operation = null)
    {
        if (operation != null)
        {
            if (operation.Value == GatewayPayloadOperation.Heartbeat)
                return;

            var bucket = _buckets.GetValueOrDefault(operation.Value);
            bucket?.NotifyCompletion();

            // TODO verify: identify payloads don't count towards the master limit.
            if (operation.Value == GatewayPayloadOperation.Identify)
                return;
        }

        _masterBucket.NotifyCompletion();
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

            // TODO verify: identify payloads don't count towards the master limit.
            if (operation.Value == GatewayPayloadOperation.Identify)
                return;
        }

        _masterBucket.Release();
    }

    public void Reset()
    {
        foreach (var kvp in _buckets)
        {
            if (kvp.Key == GatewayPayloadOperation.Identify)
                continue;

            kvp.Value.Reset();
        }

        _masterBucket.Reset();
    }

    private class Bucket
    {
        public int CurrentCount
        {
            get
            {
                lock (this)
                {
                    return _semaphore.CurrentCount;
                }
            }
        }

        private readonly ILogger _logger;
        private readonly SemaphoreSlim _semaphore;
        private readonly int _limit;
        private readonly TimeSpan _resetDelay;
        private Cts? _resetCts;
        private bool _isResetting;

        public Bucket(ILogger logger, int limit, TimeSpan resetDelay)
        {
            _logger = logger;
            _semaphore = new SemaphoreSlim(limit, limit);
            _limit = limit;
            _resetDelay = resetDelay;
        }

        // TODO edge: release waiters on a session reset?
        public Task WaitAsync(CancellationToken cancellationToken)
        {
            return _semaphore.WaitAsync(cancellationToken);
        }

        public void NotifyCompletion()
        {
            lock (this)
            {
                if (!_isResetting)
                {
                    _isResetting = true;
                    _resetCts = new Cts();
                    _ = ResetAsync(_resetCts.Token);
                }
            }
        }

        public void Release()
        {
            lock (this)
            {
                try
                {
                    _semaphore.Release();
                }
                catch { }
            }
        }

        public void Reset()
        {
            lock (this)
            {
                _resetCts?.Cancel();
                _resetCts?.Dispose();

                var currentCount = _semaphore.CurrentCount;
                var releaseCount = _limit - currentCount;
                try
                {
                    if (releaseCount != 0)
                    {
                        _semaphore.Release(releaseCount);
                        _logger.LogDebug("Reset the semaphore: {0} -> {1}.", currentCount, _limit);
                    }
                }
                finally
                {
                    _isResetting = false;
                }
            }
        }

        private async Task ResetAsync(CancellationToken cancellationToken)
        {
            await Task.Delay(_resetDelay, cancellationToken).ConfigureAwait(false);
            lock (this)
            {
                if (!_isResetting)
                    return;

                var releaseCount = _limit - _semaphore.CurrentCount;
                if (releaseCount == 0)
                    _logger.LogWarning("Releasing the semaphore by {0}.", 0);

                Reset();
            }
        }
    }
}
