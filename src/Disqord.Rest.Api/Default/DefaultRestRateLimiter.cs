using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Disqord.Http;
using Disqord.Logging;
using Disqord.Utilities.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qommon;
using Qommon.Binding;

namespace Disqord.Rest.Api.Default;

public sealed class DefaultRestRateLimiter : IRestRateLimiter, IDisposable
{
    /// <inheritdoc/>
    public ILogger Logger { get; }

    /// <inheritdoc/>
    public IRestApiClient ApiClient => _binder.Value;

    /// <summary>
    ///     Gets the maximum delay duration the rate-limiter will delay for
    ///     before throwing.
    /// </summary>
    public TimeSpan MaximumDelayDuration { get; }

    /// <summary>
    ///     Gets the maximum combined delay duration the rate-limiter will delay a request for
    ///     before throwing.
    /// </summary>
    public TimeSpan MaximumTotalDelayDuration { get; }

    /// <summary>
    ///     Gets the date at which the global rate-limit is reset.
    /// </summary>
    public DateTimeOffset? GlobalResetsAt
    {
        get
        {
            lock (_lock)
            {
                return _globalResetsAt;
            }
        }
    }

    /// <summary>
    ///     Gets the date at which the Cloudflare rate-limit is reset.
    /// </summary>
    public DateTimeOffset? CloudflareResetsAt
    {
        get
        {
            lock (_lock)
            {
                return _cloudflareResetsAt;
            }
        }
    }

    internal int BucketCount
    {
        get
        {
            lock (_lock)
            {
                return _buckets.Count;
            }
        }
    }

    private static readonly TimeSpan GlobalRequestWindow = TimeSpan.FromSeconds(1);

    private DateTimeOffset? _globalResetsAt;
    private DateTimeOffset? _cloudflareResetsAt;

    private readonly Binder<IRestApiClient> _binder;
    private readonly TimeProvider _timeProvider;
    private readonly bool _sendsConcurrently;
    private readonly int? _globalRequestsPerSecond;
    private readonly object _lock = new();

    private readonly Dictionary<IFormattableRoute, string> _hashes;
    private readonly Dictionary<string, Bucket> _buckets;
    private readonly HashSet<IFormattableRoute> _hitRateLimits;
    private readonly Queue<long> _globalRequestTimestamps;
    private readonly BucketCleanupTimer _bucketCleanupTimer;

    public DefaultRestRateLimiter(
        IOptions<DefaultRestRateLimiterConfiguration> options,
        ILogger<DefaultRestRateLimiter> logger)
        : this(options, logger, TimeProvider.System)
    { }

    public DefaultRestRateLimiter(
        IOptions<DefaultRestRateLimiterConfiguration> options,
        ILogger<DefaultRestRateLimiter> logger,
        TimeProvider timeProvider)
    {
        var configuration = options.Value;
        Logger = logger;
        MaximumDelayDuration = configuration.MaximumDelayDuration;
        MaximumTotalDelayDuration = configuration.MaximumTotalDelayDuration;

        _binder = new Binder<IRestApiClient>(this);
        _timeProvider = timeProvider;
        if (timeProvider != TimeProvider.System)
        {
            Logger.LogWarning("Rate-limiting with the time provider {0}. A fake or test time provider will break rate-limit delays.", timeProvider.GetType());
        }

        _sendsConcurrently = configuration.SendsSameBucketRequestsConcurrently;

        var globalRequestsPerSecond = configuration.GlobalRequestsPerSecond;
        if (globalRequestsPerSecond != null)
        {
            Guard.IsPositive(globalRequestsPerSecond.Value);
        }

        _globalRequestsPerSecond = globalRequestsPerSecond;

        _hashes = new Dictionary<IFormattableRoute, string>();
        _buckets = new Dictionary<string, Bucket>();
        _hitRateLimits = new HashSet<IFormattableRoute>();
        _globalRequestTimestamps = new Queue<long>();

        var bucketCleanupInterval = configuration.BucketCleanupInterval;
        if (bucketCleanupInterval != Timeout.InfiniteTimeSpan)
        {
            Guard.IsGreaterThan(bucketCleanupInterval, TimeSpan.Zero);
        }

        _bucketCleanupTimer = new BucketCleanupTimer(this, timeProvider, bucketCleanupInterval);
    }

    public void Bind(IRestApiClient value)
    {
        _binder.Bind(value);
    }

    /// <inheritdoc/>
    public bool IsRateLimited(IFormattedRoute? route = null)
    {
        lock (_lock)
        {
            var now = _timeProvider.GetUtcNow();
            if (route == null)
            {
                return _globalResetsAt > now || _cloudflareResetsAt > now || GetGlobalWindowDelay() > TimeSpan.Zero;
            }

            var bucket = GetBucket(route, create: false);
            var delay = bucket != null
                ? bucket.GetDelay(route, now, out _)
                : GetGlobalDelay(route, now);

            var mustWaitForResponse = bucket != null && bucket.MustWaitForResponse(now);
            return delay > TimeSpan.Zero || mustWaitForResponse;
        }
    }

    /// <inheritdoc/>
    public async Task<IRestResponse> ExecuteAsync(IRestRequest request, CancellationToken cancellationToken)
    {
        using (var pendingRequest = new Bucket.PendingRequest(request, cancellationToken))
        {
            lock (_lock)
            {
                GetBucket(request.Route, create: true)!.Post(pendingRequest);
            }

            return await pendingRequest.Task.ConfigureAwait(false);
        }
    }

    public void Dispose()
    {
        _bucketCleanupTimer.Dispose();
    }

    private Bucket? GetBucket(IFormattedRoute route, bool create)
    {
        lock (_lock)
        {
            var isUnlimited = !_hashes.TryGetValue(route.BaseRoute, out var hash);
            hash ??= $"unlimited+{route}";
            var parameters = route.Parameters;
            var bucketId = $"{hash}:{parameters.GetGuildId()}:{parameters.GetChannelId()}:{parameters.GetWebhookId()}:{parameters.GetInteractionToken()}";
            if (!_buckets.TryGetValue(bucketId, out var bucket) && create)
            {
                bucket = new Bucket(this, isUnlimited);
                _buckets.Add(bucketId, bucket);
            }

            return bucket;
        }
    }

    private void RemoveExpiredBuckets()
    {
        lock (_lock)
        {
            var now = _timeProvider.GetUtcNow();
            var removedBucketCount = 0;
            foreach (var (bucketId, bucket) in _buckets)
            {
                if (!bucket.CanBeRemoved(now))
                {
                    continue;
                }

                _buckets.Remove(bucketId);
                bucket.Close();
                removedBucketCount++;
            }

            if (removedBucketCount > 0)
            {
                Logger.LogDebug("Removed {0} expired buckets.", removedBucketCount);
            }
        }
    }

    private TimeSpan GetGlobalDelay(IFormattedRoute route, DateTimeOffset now)
    {
        if (!IsBoundToGlobalRateLimit(route))
        {
            return GetDelayUntil(_cloudflareResetsAt, now);
        }

        var resetDelay = GetDelayUntil(Later(_globalResetsAt, _cloudflareResetsAt), now);
        var windowDelay = GetGlobalWindowDelay();
        return resetDelay > windowDelay
            ? resetDelay
            : windowDelay;
    }

    private TimeSpan GetGlobalWindowDelay()
    {
        if (_globalRequestsPerSecond == null)
        {
            return TimeSpan.Zero;
        }

        var now = _timeProvider.GetTimestamp();
        while (_globalRequestTimestamps.TryPeek(out var oldestTimestamp) && _timeProvider.GetElapsedTime(oldestTimestamp, now) >= GlobalRequestWindow)
        {
            _globalRequestTimestamps.Dequeue();
        }

        return _globalRequestTimestamps.Count < _globalRequestsPerSecond
            ? TimeSpan.Zero
            : GlobalRequestWindow - _timeProvider.GetElapsedTime(_globalRequestTimestamps.Peek(), now);
    }

    private void RecordGlobalRequest(IFormattedRoute route)
    {
        if (_globalRequestsPerSecond != null && IsBoundToGlobalRateLimit(route))
        {
            _globalRequestTimestamps.Enqueue(_timeProvider.GetTimestamp());
        }
    }

    private async Task<bool> HandleResponseAsync(Bucket.PendingRequest pendingRequest, IRestResponse response)
    {
        var httpResponse = response.HttpResponse;
        TimeSpan? bodyRetryAfter = null;
        if (httpResponse.StatusCode == HttpResponseStatusCode.TooManyRequests)
        {
            try
            {
                bodyRetryAfter = await ReadBodyRetryAfterAsync(httpResponse, pendingRequest.CancellationToken).ConfigureAwait(false);
            }
            catch
            {
                response.Dispose();
                throw;
            }
        }

        if (!UpdateBucket(pendingRequest, httpResponse, bodyRetryAfter))
        {
            return false;
        }

        response.Dispose();
        return true;
    }

    private async Task<TimeSpan?> ReadBodyRetryAfterAsync(IHttpResponse response, CancellationToken cancellationToken)
    {
        var stream = await response.ReadAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            var model = ApiClient.Serializer.Deserialize<RestApiRateLimitedJsonModel>(stream);
            if (model != null && model.RetryAfter.HasValue)
            {
                return TimeSpan.FromSeconds(model.RetryAfter.Value);
            }
        }
        catch (Exception ex)
        {
            Logger.LogDebug(ex, "Failed to deserialize the rate-limit response body.");
        }
        finally
        {
            if (stream.CanSeek)
            {
                stream.Position = 0;
            }
        }

        return null;
    }

    private bool UpdateBucket(Bucket.PendingRequest pendingRequest, IHttpResponse response, TimeSpan? bodyRetryAfter)
    {
        var route = pendingRequest.Request.Route;
        lock (_lock)
        {
            try
            {
                var now = _timeProvider.GetUtcNow();
                var headers = new DefaultRestResponseHeaders(response.Headers);
                if (headers.Bucket.HasValue)
                {
                    if (_hashes.TryAdd(route.BaseRoute, headers.Bucket.Value))
                    {
                        Logger.LogTrace("Cached bucket hash {0} -> {1}.", new LoggedRoute(route), headers.Bucket.Value);
                    }
                }

                var bucket = GetBucket(route, create: true)!;
                if (response.StatusCode == HttpResponseStatusCode.TooManyRequests)
                {
                    var headerRetryAfter = headers.RetryAfter;
                    if (bodyRetryAfter == null && !headerRetryAfter.HasValue)
                    {
                        Logger.LogError("Hit a rate-limit on route {0} without a retry-after duration.", new LoggedRoute(route));
                        return false;
                    }

                    var retryAfter = bodyRetryAfter ?? headerRetryAfter.Value;
                    if (headers.IsGlobal.GetValueOrDefault() && IsBoundToGlobalRateLimit(route))
                    {
                        Logger.LogError("Hit the global rate-limit on route {0}! Expires after {1}ms.", new LoggedRoute(route), retryAfter.TotalMilliseconds);
                        _globalResetsAt = now + retryAfter;
                        return true;
                    }

                    if (!headers.GetHeader("Via").HasValue)
                    {
                        Logger.LogError("Hit a Cloudflare rate-limit on route {0}! Expires after {1}ms.", new LoggedRoute(route), retryAfter.TotalMilliseconds);
                        _cloudflareResetsAt = now + retryAfter;
                        return true;
                    }

                    bucket.SetRateLimited(now + retryAfter);
                    if (headers.Scope == "shared")
                    {
                        Logger.LogDebug("Hit a shared rate-limit on route {0}. Expires after {1}ms.", new LoggedRoute(route), retryAfter.TotalMilliseconds);
                        return true;
                    }

                    var level = _hitRateLimits.Add(route.BaseRoute)
                        ? LogLevel.Information
                        : LogLevel.Warning;

                    Logger.Log(level, "Hit a rate-limit on route {0}. Expires after {1}ms.", new LoggedRoute(route), retryAfter.TotalMilliseconds);
                    return true;
                }

                if (!headers.Bucket.HasValue)
                    return false;

                bucket.Update(pendingRequest, headers.Limit.Value, headers.Remaining.Value, now + headers.ResetsAfter.Value, now);
                Logger.LogDebug("Updated the bucket for route {0} to ({1}/{2}, {3})", new LoggedRoute(route), bucket.Remaining, bucket.Limit, bucket.ResetsAt - now);
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Encountered an exception while updating bucket for {0}. Code: {1} Headers:\n{2}", new LoggedRoute(route), response.StatusCode, string.Join('\n', response.Headers));
                return false;
            }
        }
    }

    private static bool IsBoundToGlobalRateLimit(IFormattedRoute route)
    {
        var isInteractionRoute = route.Parameters.GetInteractionToken() != null;
        return !isInteractionRoute;
    }

    private static TimeSpan GetDelayUntil(DateTimeOffset? resetsAt, DateTimeOffset now)
    {
        return resetsAt > now
            ? resetsAt.Value - now
            : TimeSpan.Zero;
    }

    private static DateTimeOffset? Later(DateTimeOffset? first, DateTimeOffset? second)
    {
        return second == null || first > second
            ? first
            : second;
    }

    private static DateTimeOffset Later(DateTimeOffset first, DateTimeOffset second)
    {
        return first > second
            ? first
            : second;
    }

    private readonly struct LoggedRoute(IFormattedRoute route)
    {
        public override string ToString()
        {
            return $"{route.Method}|{route.RedactedPath}";
        }
    }

    private sealed class BucketCleanupTimer : IDisposable
    {
        private readonly WeakReference<DefaultRestRateLimiter> _rateLimiter;
        private readonly ITimer _timer;

        public BucketCleanupTimer(DefaultRestRateLimiter rateLimiter, TimeProvider timeProvider, TimeSpan interval)
        {
            _rateLimiter = new WeakReference<DefaultRestRateLimiter>(rateLimiter);

            var roundedUpInterval = TimeSpan.FromMilliseconds(Math.Ceiling(interval.TotalMilliseconds));
            _timer = timeProvider.CreateTimer(static state => ((BucketCleanupTimer) state!).Tick(), this, roundedUpInterval, roundedUpInterval);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }

        private void Tick()
        {
            if (!_rateLimiter.TryGetTarget(out var rateLimiter))
            {
                _timer.Dispose();
                return;
            }

            try
            {
                rateLimiter.RemoveExpiredBuckets();
            }
            catch (Exception ex)
            {
                try
                {
                    rateLimiter.Logger.LogError(ex, "An exception occurred while removing expired buckets.");
                }
                catch { }
            }
        }
    }

    private class Bucket : ILogging
    {
        public int Limit { get; private set; } = 1;

        public int Remaining { get; private set; } = 1;

        public DateTimeOffset ResetsAt { get; private set; }

        public ILogger Logger => _rateLimiter.Logger;

        private readonly DefaultRestRateLimiter _rateLimiter;
        private readonly bool _isUnlimited;
        private readonly bool _sendsConcurrently;
        private readonly Channel<PendingRequest> _pendingRequests;
        private readonly HashSet<PendingRequest> _inFlightRequests;
        private int _pendingRequestCount;
        private int _heldRequestCount;
        private Tcs? _availabilityChanged;

        public Bucket(DefaultRestRateLimiter rateLimiter, bool isUnlimited)
        {
            _rateLimiter = rateLimiter;
            _isUnlimited = isUnlimited;
            _sendsConcurrently = rateLimiter._sendsConcurrently && !isUnlimited;

            _pendingRequests = Channel.CreateUnbounded<PendingRequest>(new UnboundedChannelOptions
            {
                SingleReader = true,
            });

            _inFlightRequests = new HashSet<PendingRequest>();

            _ = RunAsync();
        }

        public class PendingRequest : IDisposable
        {
            public IRestRequest Request { get; }

            public CancellationToken CancellationToken { get; }

            public Task<IRestResponse> Task => _tcs.Task;

            public TimeSpan TotalDelay { get; set; }

            private readonly Tcs<IRestResponse> _tcs;
            private readonly CancellationTokenRegistration _reg;

            public PendingRequest(IRestRequest request, CancellationToken cancellationToken)
            {
                Request = request;
                CancellationToken = cancellationToken;

                static void CancellationCallback(object? state, CancellationToken cancellationToken)
                {
                    var tcs = Unsafe.As<Tcs<IRestResponse>>(state!);
                    tcs.Cancel(cancellationToken);
                }

                _tcs = new Tcs<IRestResponse>();
                _reg = cancellationToken.UnsafeRegister(CancellationCallback, _tcs);
            }

            public void Complete(IRestResponse response)
            {
                _tcs.Complete(response);
            }

            public void Complete(Exception exception)
            {
                _tcs.Throw(exception);
            }

            public void Dispose()
            {
                _reg.Dispose();
            }
        }

        public void Post(PendingRequest pendingRequest)
        {
            _pendingRequestCount++;
            var isWritten = _pendingRequests.Writer.TryWrite(pendingRequest);
            Debug.Assert(isWritten);
        }

        public TimeSpan GetDelay(IFormattedRoute route, DateTimeOffset now, out bool isGloballyRateLimited)
        {
            var globalDelay = _rateLimiter.GetGlobalDelay(route, now);
            var bucketDelay = Remaining == 0 && ResetsAt > now
                ? ResetsAt - now
                : TimeSpan.Zero;

            isGloballyRateLimited = globalDelay >= bucketDelay;
            return isGloballyRateLimited
                ? globalDelay
                : bucketDelay;
        }

        public bool MustWaitForResponse(DateTimeOffset now)
        {
            return ResetsAt <= now && _inFlightRequests.Count > 0 && _inFlightRequests.Count + _heldRequestCount >= Limit;
        }

        public void Update(PendingRequest pendingRequest, int limit, int remaining, DateTimeOffset resetsAt, DateTimeOffset now)
        {
            _inFlightRequests.Remove(pendingRequest);
            Limit = limit;
            if (!_sendsConcurrently)
            {
                Remaining = remaining;
                ResetsAt = resetsAt;
                return;
            }

            _heldRequestCount = 0;
            var remainingAfterInFlightRequests = Math.Max(remaining - _inFlightRequests.Count, 0);
            if (ResetsAt > now)
            {
                Remaining = Math.Min(Remaining, remainingAfterInFlightRequests);
                ResetsAt = Later(ResetsAt, resetsAt);
            }
            else
            {
                Remaining = remainingAfterInFlightRequests;
                ResetsAt = resetsAt;
            }

            NotifyAvailabilityChanged();
        }

        public void SetRateLimited(DateTimeOffset resetsAt)
        {
            _heldRequestCount = 0;
            Remaining = 0;
            ResetsAt = _sendsConcurrently
                ? Later(ResetsAt, resetsAt)
                : resetsAt;
        }

        public bool CanBeRemoved(DateTimeOffset now)
        {
            return _pendingRequestCount == 0 && ResetsAt <= now;
        }

        public void Close()
        {
            _pendingRequests.Writer.Complete();
        }

        private async Task RunAsync()
        {
            await foreach (var pendingRequest in _pendingRequests.Reader.ReadAllAsync().ConfigureAwait(false))
            {
                if (pendingRequest.CancellationToken.IsCancellationRequested)
                {
                    Release(pendingRequest);
                    continue;
                }

                if (!await TryWaitToSendAsync(pendingRequest).ConfigureAwait(false))
                {
                    continue;
                }

                var sending = SendAsync(pendingRequest);
                if (!_sendsConcurrently)
                {
                    await sending.ConfigureAwait(false);
                }
            }
        }

        private async Task SendAsync(PendingRequest pendingRequest)
        {
            try
            {
                do
                {
                    var response = await _rateLimiter.ApiClient.Requester.ExecuteAsync(pendingRequest.Request, pendingRequest.CancellationToken).ConfigureAwait(false);
                    if (!await _rateLimiter.HandleResponseAsync(pendingRequest, response).ConfigureAwait(false))
                    {
                        Complete(pendingRequest, response);
                        return;
                    }

                    lock (_rateLimiter._lock)
                    {
                        EndSending(pendingRequest);
                    }
                }
                while (await TryWaitToSendAsync(pendingRequest).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                Fail(pendingRequest, ex);
            }
        }

        private async Task<bool> TryWaitToSendAsync(PendingRequest pendingRequest)
        {
            if (_isUnlimited && TryMoveRequest(pendingRequest))
            {
                return false;
            }

            return await WaitForRateLimitsAsync(pendingRequest).ConfigureAwait(false);
        }

        private async Task<bool> WaitForRateLimitsAsync(PendingRequest pendingRequest)
        {
            var request = pendingRequest.Request;
            try
            {
                while (!TryStartSending(pendingRequest, out var delay, out var isGloballyRateLimited, out var availabilityChanged))
                {
                    if (availabilityChanged != null)
                    {
                        await availabilityChanged.WaitAsync(pendingRequest.CancellationToken).ConfigureAwait(false);
                        continue;
                    }

                    var options = request.Options as DefaultRestRequestOptions;
                    var maximumDelayDuration = options?.MaximumDelayDuration ?? _rateLimiter.MaximumDelayDuration;
                    if (Exceeds(delay, maximumDelayDuration))
                    {
                        Logger.LogDebug("Route {0} is rate-limited - throwing as the delay {1} exceeds the maximum delay duration.", new LoggedRoute(request.Route), delay);
                        Complete(pendingRequest, new MaximumRateLimitDelayExceededException(request, delay, isGloballyRateLimited));
                        return false;
                    }

                    var maximumTotalDelayDuration = options?.MaximumTotalDelayDuration ?? _rateLimiter.MaximumTotalDelayDuration;
                    var totalDelay = pendingRequest.TotalDelay + delay;
                    if (Exceeds(totalDelay, maximumTotalDelayDuration))
                    {
                        Logger.LogDebug("Route {0} is rate-limited - throwing as the total delay {1} exceeds the maximum total delay duration.", new LoggedRoute(request.Route), totalDelay);
                        Complete(pendingRequest, new MaximumRateLimitDelayExceededException(request, delay, totalDelay, isGloballyRateLimited));
                        return false;
                    }

                    pendingRequest.TotalDelay = totalDelay;
                    Logger.LogDebug("Route {0} is rate-limited - delaying for {1}.", new LoggedRoute(request.Route), delay);

                    var roundedUpDelay = TimeSpan.FromMilliseconds(Math.Ceiling(delay.TotalMilliseconds));
                    await Task.Delay(roundedUpDelay, _rateLimiter._timeProvider, pendingRequest.CancellationToken).ConfigureAwait(false);
                }

                return true;
            }
            catch (Exception ex)
            {
                Fail(pendingRequest, ex);
                return false;
            }
        }

        private bool TryStartSending(PendingRequest pendingRequest, out TimeSpan delay, out bool isGloballyRateLimited, out Task? availabilityChanged)
        {
            lock (_rateLimiter._lock)
            {
                var route = pendingRequest.Request.Route;
                var now = _rateLimiter._timeProvider.GetUtcNow();
                availabilityChanged = null;
                delay = GetDelay(route, now, out isGloballyRateLimited);
                if (delay > TimeSpan.Zero)
                {
                    return false;
                }

                if (_sendsConcurrently)
                {
                    if (MustWaitForResponse(now))
                    {
                        _availabilityChanged ??= new Tcs();
                        availabilityChanged = _availabilityChanged.Task;
                        return false;
                    }

                    if (ResetsAt > now)
                    {
                        Remaining--;
                    }

                    _inFlightRequests.Add(pendingRequest);
                }

                _rateLimiter.RecordGlobalRequest(route);
                return true;
            }
        }

        private bool TryMoveRequest(PendingRequest pendingRequest)
        {
            lock (_rateLimiter._lock)
            {
                var bucket = _rateLimiter.GetBucket(pendingRequest.Request.Route, create: true)!;
                if (bucket == this)
                {
                    return false;
                }

                Logger.LogDebug("Route {0} is moving the request to the limited bucket.", new LoggedRoute(pendingRequest.Request.Route));
                bucket.Post(pendingRequest);
                _pendingRequestCount--;
                return true;
            }
        }

        private void Complete(PendingRequest pendingRequest, IRestResponse response)
        {
            Release(pendingRequest);
            pendingRequest.Complete(response);
        }

        private void Complete(PendingRequest pendingRequest, Exception exception)
        {
            Release(pendingRequest);
            pendingRequest.Complete(exception);
        }

        private void Fail(PendingRequest pendingRequest, Exception exception)
        {
            Complete(pendingRequest, exception);

            if (exception is not (OperationCanceledException or TimeoutException))
            {
                Logger.LogError(exception, "Route {0} encountered an exception while processing the request.", new LoggedRoute(pendingRequest.Request.Route));
            }
        }

        private void Release(PendingRequest pendingRequest)
        {
            lock (_rateLimiter._lock)
            {
                EndSending(pendingRequest);
                _pendingRequestCount--;
            }
        }

        private void EndSending(PendingRequest pendingRequest)
        {
            if (!_inFlightRequests.Remove(pendingRequest))
            {
                return;
            }

            if (_inFlightRequests.Count == 0)
            {
                _heldRequestCount = 0;
            }
            else if (ResetsAt <= _rateLimiter._timeProvider.GetUtcNow())
            {
                _heldRequestCount++;
            }

            NotifyAvailabilityChanged();
        }

        private void NotifyAvailabilityChanged()
        {
            _availabilityChanged?.Complete();
            _availabilityChanged = null;
        }

        private static bool Exceeds(TimeSpan delay, TimeSpan maximumDelay)
        {
            return maximumDelay != Timeout.InfiniteTimeSpan && delay > maximumDelay;
        }
    }
}
