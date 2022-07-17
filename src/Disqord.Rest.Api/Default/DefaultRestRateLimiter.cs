using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Disqord.Http;
using Disqord.Logging;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Qommon;
using Qommon.Binding;

namespace Disqord.Rest.Api.Default;

public sealed class DefaultRestRateLimiter : IRestRateLimiter
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

    private DateTimeOffset? _globalResetsAt;

    private readonly Binder<IRestApiClient> _binder;

    private readonly Dictionary<IFormattableRoute, string> _hashes;
    private readonly Dictionary<string, Bucket> _buckets;
    private readonly HashSet<IFormattableRoute> _hitRateLimits;

    public DefaultRestRateLimiter(
        IOptions<DefaultRestRateLimiterConfiguration> options,
        ILogger<DefaultRestRateLimiter> logger)
    {
        var configuration = options.Value;
        Logger = logger;
        MaximumDelayDuration = configuration.MaximumDelayDuration;

        _binder = new Binder<IRestApiClient>(this);

        _hashes = new Dictionary<IFormattableRoute, string>();
        _buckets = new Dictionary<string, Bucket>();
        _hitRateLimits = new HashSet<IFormattableRoute>();
    }

    public void Bind(IRestApiClient apiClient)
    {
        _binder.Bind(apiClient);
    }

    /// <inheritdoc/>
    public bool IsRateLimited(IFormattedRoute? route = null)
    {
        if (route == null)
            return _globalResetsAt > DateTimeOffset.UtcNow;

        var bucket = GetBucket(route, false);
        if (bucket == null)
            return false;

        return bucket.Remaining == 0;
    }

    /// <inheritdoc/>
    public ValueTask EnqueueRequestAsync(IRestRequest request)
    {
        var bucket = GetBucket(request.Route, true)!;
        bucket.Post(request);
        return default;
    }

    private Bucket? GetBucket(IFormattedRoute route, bool create)
    {
        lock (this)
        {
            var isUnlimited = !_hashes.TryGetValue(route.BaseRoute, out var hash);
            hash ??= $"unlimited+{route}";
            var parameters = $"{route.Parameters.GetGuildId()}:{route.Parameters.GetChannelId()}:{route.Parameters.GetWebhookId()}";
            var bucketId = $"{hash}:{parameters}";
            if (!_buckets.TryGetValue(bucketId, out var bucket) && create)
            {
                bucket = new Bucket(this, isUnlimited);
                _buckets.Add(bucketId, bucket);
            }

            return bucket;
        }
    }

    private bool UpdateBucket(IFormattedRoute route, IHttpResponse response)
    {
        lock (this)
        {
            try
            {
                var now = DateTimeOffset.UtcNow;
                var headers = new DefaultRestResponseHeaders(response.Headers);
                if (headers.Bucket.HasValue)
                {
                    if (_hashes.TryAdd(route.BaseRoute, headers.Bucket.Value))
                        Logger.LogTrace("Cached bucket hash {0} -> {1}.", route, headers.Bucket.Value);
                }

                var bucket = GetBucket(route, true)!;
                if (response.StatusCode == HttpResponseStatusCode.TooManyRequests)
                {
                    if (headers.IsGlobal.GetValueOrDefault() || !headers.GetHeader("Via").HasValue)
                    {
                        var type = headers.IsGlobal.GetValueOrDefault()
                            ? "global"
                            : "Cloudflare";

                        Logger.LogError("Hit a {0} rate-limit! Expires after {1}.", type, headers.RetryAfter.Value);
                        _globalResetsAt = now + headers.RetryAfter.Value;
                    }
                    else
                    {
                        bucket.Remaining = 0;
                        bucket.ResetsAt = now + headers.RetryAfter.Value;
                        var isShared = headers.Scope == "shared";
                        var level = _hitRateLimits.Add(route.BaseRoute) && headers.RetryAfter.Value.TotalSeconds < 30 || isShared
                            ? LogLevel.Information
                            : LogLevel.Warning;

                        var message = isShared
                            ? "Hit a shared rate-limit on route {0}. Expires after {1}ms."
                            : "Hit a rate-limit on route {0}. Expires after {1}ms.";

                        Logger.Log(level, message, route, headers.RetryAfter.Value.TotalMilliseconds);
                        return true;
                    }
                }

                if (!headers.Bucket.HasValue)
                    return false;

                bucket.Limit = headers.Limit.Value;
                bucket.Remaining = headers.Remaining.Value;
                bucket.ResetsAt = now + headers.ResetsAfter.Value;
                Logger.LogDebug("Updated the bucket for route {0} to ({1}/{2}, {3})", route, bucket.Remaining, bucket.Limit, bucket.ResetsAt - now);
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Encountered an exception while updating bucket for {0}. Code: {2} Headers:\n{3}", route, response.StatusCode, string.Join('\n', response.Headers));
                return false;
            }
        }
    }

    private class Bucket : ILogging
    {
        public int Limit { get; internal set; } = 1;

        public int Remaining { get; internal set; } = 1;

        public DateTimeOffset ResetsAt { get; internal set; }

        public ILogger Logger => _rateLimiter.Logger;

        private readonly DefaultRestRateLimiter _rateLimiter;
        private readonly bool _isUnlimited;
        private readonly Channel<IRestRequest> _requests;

        public Bucket(DefaultRestRateLimiter rateLimiter, bool isUnlimited)
        {
            _rateLimiter = rateLimiter;
            _isUnlimited = isUnlimited;

            _requests = Channel.CreateUnbounded<IRestRequest>(new UnboundedChannelOptions
            {
                SingleReader = true,
            });

            _ = RunAsync();
        }

        public void Post(IRestRequest request)
        {
            _requests.Writer.TryWrite(request);
        }

        private async Task RunAsync()
        {
            var reader = _requests.Reader;
            await foreach (var request in reader.ReadAllAsync().ConfigureAwait(false))
            {
                bool retry;
                do
                {
                    retry = false;
                    try
                    {
                        if (_isUnlimited)
                        {
                            var bucket = _rateLimiter.GetBucket(request.Route, false)!;
                            if (bucket != this)
                            {
                                Logger.LogDebug("Route {0} is moving the request to the limited bucket.", request.Route);
                                bucket.Post(request);
                                break;
                            }
                        }

                        var now = DateTimeOffset.UtcNow;
                        var globalResetsAt = _rateLimiter._globalResetsAt;
                        var isGloballyRateLimited = globalResetsAt > now;
                        if (Remaining == 0 || isGloballyRateLimited)
                        {
                            var delay = isGloballyRateLimited
                                ? globalResetsAt!.Value - now
                                : ResetsAt - now;

                            if (delay > TimeSpan.Zero)
                            {
                                var maximumDelayDuration = (request.Options as DefaultRestRequestOptions)?.MaximumDelayDuration ?? _rateLimiter.MaximumDelayDuration;
                                if (maximumDelayDuration != Timeout.InfiniteTimeSpan && delay > maximumDelayDuration)
                                {
                                    Logger.LogDebug("Route {0} is rate-limited - throwing as the delay {1} exceeds the maximum delay duration.", request.Route, delay);
                                    request.Complete(new MaximumRateLimitDelayExceededException(request, delay, isGloballyRateLimited));
                                    break;
                                }

                                var level = request.Route.BaseRoute.Equals(Route.Channel.CreateReaction)
                                    ? LogLevel.Debug
                                    : LogLevel.Information;

                                Logger.Log(level, "Route {0} is rate-limited - delaying for {1}.", request.Route, delay);
                                await Task.Delay(delay).ConfigureAwait(false);
                            }
                        }

                        var response = await _rateLimiter.ApiClient.Requester.ExecuteAsync(request).ConfigureAwait(false);
                        if (_rateLimiter.UpdateBucket(request.Route, response.HttpResponse))
                        {
                            Logger.LogInformation("Route {0} is retrying the last request due to a hit rate-limit.", request.Route);
                            retry = true;
                        }
                        else
                        {
                            request.Complete(response);
                            request.Dispose();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "Route {0} encountered an exception while processing a request.", request.Route);
                        request.Dispose();
                    }
                }
                while (retry);
            }
        }
    }
}
