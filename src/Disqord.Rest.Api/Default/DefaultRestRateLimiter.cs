using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Disqord.Http;
using Disqord.Logging;
using Disqord.Utilities.Binding;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Disqord.Rest.Api.Default
{
    public sealed class DefaultRestRateLimiter : IRestRateLimiter
    {
        public const string UNLIMITED_BUCKET = "unlimited";

        public ILogger Logger { get; }

        public IRestApiClient ApiClient => _binder.Value;

        public TimeSpan MaximumDelayDuration { get; }

        private readonly Binder<IRestApiClient> _binder;

        private readonly object _lock;
        private readonly Dictionary<Route, string> _hashes;
        private readonly Dictionary<string, Bucket> _buckets;
        private readonly HashSet<Route> _hitRateLimits;

        public DefaultRestRateLimiter(
            IOptions<DefaultRestRateLimiterConfiguration> options,
            ILogger<DefaultRestRateLimiter> logger)
        {
            var configuration = options.Value;
            Logger = logger;
            MaximumDelayDuration = configuration.MaximumDelayDuration;

            _binder = new Binder<IRestApiClient>(this);

            _lock = new object();
            _hashes = new Dictionary<Route, string>();
            _buckets = new Dictionary<string, Bucket>();
            _hitRateLimits = new HashSet<Route>();
        }

        public void Bind(IRestApiClient apiClient)
        {
            _binder.Bind(apiClient);
        }

        public bool IsRateLimited(FormattedRoute route = null)
        {
            // TODO: proper global handling
            if (route == null)
                return false;

            var bucket = GetBucket(route, false);
            if (bucket == null)
                return false;

            return bucket.Remaining == 0;
        }

        public ValueTask EnqueueRequestAsync(IRestRequest request)
        {
            // Asks Discord for accurate, not rounded up rate-limits.
            // TODO: Remove for v8.
            if (ApiClient.Requester.Version != 8)
                request.Options.Headers["X-Ratelimit-Precision"] = "millisecond";

            var bucket = GetBucket(request.Route, true);
            bucket.Enqueue(request);
            return default;
        }

        private Bucket GetBucket(FormattedRoute route, bool create)
        {
            lock (_lock)
            {
                var hash = GetRouteHash(route.BaseRoute);
                var parameters = $"{route.Parameters.GuildId}:{route.Parameters.ChannelId}:{route.Parameters.WebhookId}";
                var bucketId = $"{hash}:{parameters}";
                if (!_buckets.TryGetValue(bucketId, out var bucket) && create)
                {
                    bucket = new Bucket(this, bucketId);
                    _buckets.Add(bucketId, bucket);
                }

                return bucket;
            }
        }

        private string GetRouteHash(Route route)
        {
            lock (_lock)
            {
                return _hashes.GetValueOrDefault(route) ?? $"{UNLIMITED_BUCKET}+{route}";
            }
        }

        private bool UpdateBucket(FormattedRoute route, IHttpResponse response)
        {
            lock (_lock)
            {
                try
                {
                    var now = DateTimeOffset.UtcNow;
                    var bucket = GetBucket(route, true);
                    var wasUnlimited = bucket.IsUnlimited;
                    var headers = new DiscordHeaders(response.Headers);
                    if (headers.Bucket != null)
                    {
                        if (_hashes.TryAdd(route.BaseRoute, headers.Bucket))
                            Logger.LogTrace("Cached bucket hash {0} -> {1}.", route, headers.Bucket);

                        bucket = GetBucket(route, true);
                    }

                    if (headers.IsGlobal)
                    {
                        Logger.LogError("Hit the global rate-limit! Retry-After: {0}ms.", headers.RetryAfter.Value.TotalMilliseconds);
                    }
                    else if ((int) response.Code == 429)
                    {
                        bucket.Remaining = 0;
                        bucket.ResetsAt = now + headers.RetryAfter.Value;
                        var severity = _hitRateLimits.Add(route.BaseRoute)
                            ? LogLevel.Information
                            : LogLevel.Warning;
                        Logger.Log(severity, "Bucket {0} hit a rate-limit. Retry-After: {1}ms.", bucket, headers.RetryAfter.Value.TotalMilliseconds);
                        return true;
                    }

                    if (headers.Bucket == null)
                        return false;

                    bucket.Limit = Math.Max(1, headers.Limit.Value);
                    bucket.Remaining = headers.Remaining.Value;
                    bucket.ResetsAt = now + headers.ResetsAfter;
                    Logger.LogDebug("Updated bucket {0} to ({1}/{2}, {3})", bucket, bucket.Remaining, bucket.Limit, bucket.ResetsAt - now);
                    return false;
                }
                catch (Exception ex)
                {
                    var bucket = GetBucket(route, true);
                    Logger.LogError(ex, "Encountered an exception while updating bucket {0}. Route: {1} Code: {2} Headers:\n{3}",
                        bucket, route, response.Code, string.Join('\n', response.Headers));
                    return false;
                }
            }
        }

        public void Dispose()
        {
            //foreach (var bucket in _buckets.Values)
            //    bucket.Dispose();

            //_buckets.Clear();
        }

        private class Bucket : ILogging
        {
            public ILogger Logger => _rateLimiter.Logger;

            public string Id { get; }

            public bool IsUnlimited => Id.StartsWith("unlimited");

            public int Limit { get; internal set; }

            public int Remaining { get; internal set; }

            public DateTimeOffset ResetsAt { get; internal set; }

            private readonly DefaultRestRateLimiter _rateLimiter;
            private readonly object _lock;
            private readonly LinkedList<IRestRequest> _requests;

            private Task _task;

            public Bucket(DefaultRestRateLimiter rateLimiter, string id)
            {
                Id = id;

                Limit = 1;
                Remaining = 1;

                _rateLimiter = rateLimiter;
                _lock = new object();
                _requests = new LinkedList<IRestRequest>();
            }

            public void Enqueue(IRestRequest request)
            {
                lock (_lock)
                {
                    _requests.AddLast(request);
                    if (_task == null || _task.IsCompleted)
                        _task = Task.Run(RunAsync);
                }
            }

            private async Task RunAsync()
            {
                //Logger.LogTrace("Bucket {0} is running {1} requests.", Id, _requests.Count);
                LinkedListNode<IRestRequest> requestNode;
                while ((requestNode = _requests.First) != null)
                {
                    var request = requestNode.Value;
                    _requests.RemoveFirst();
                    if (Remaining == 0)
                    {
                        var delay = ResetsAt - DateTimeOffset.UtcNow;
                        if (delay > TimeSpan.Zero)
                        {
                            if (_rateLimiter.MaximumDelayDuration != Timeout.InfiniteTimeSpan && delay > _rateLimiter.MaximumDelayDuration)
                            {
                                Logger.LogWarning("Bucket {0} is pre-emptively rate-limiting, throwing as the delay {1} exceeds the maximum delay duration.", Id, delay);
                                // TODO: MaximumDelayDurationExceededException
                                request.Complete(new TimeoutException());
                                continue;
                            }

                            Logger.LogInformation("Bucket {0} is pre-emptively rate-limiting, delaying for {1}.", Id, delay);
                            await Task.Delay(delay).ConfigureAwait(false);
                        }
                    }

                    // If this bucket is unlimited we check if there's a proper bucket created and move the requests accordingly.
                    if (IsUnlimited)
                    {
                        var bucket = _rateLimiter.GetBucket(request.Route, false);
                        if (bucket != this)
                        {
                            Logger.LogDebug("Bucket {0} is moving the request to the limited bucket {1}.", Id, bucket);
                            bucket.Enqueue(request);
                            continue;
                        }
                    }

                    try
                    {
                        await _rateLimiter.ApiClient.Requester.ExecuteAsync(request).ConfigureAwait(false);
                        var response = await request.WaitAsync().ConfigureAwait(false);
                        if (_rateLimiter.UpdateBucket(request.Route, response.HttpResponse))
                        {
                            Logger.LogInformation("Bucket {0} is re-enqueuing the last request due to a hit rate-limit.", Id);
                            _requests.AddFirst(request);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.LogError(ex, "Bucket {0} encountered an exception when executing a request.", Id);
                    }
                }
            }

            public override string ToString()
                => Id;
        }

        // TODO: remake
        private sealed class DiscordHeaders
        {
            public bool IsGlobal { get; }

            public TimeSpan? RetryAfter { get; }

            public int? Limit { get; }

            public int? Remaining { get; }

            public DateTimeOffset? ResetsAt { get; }

            public TimeSpan ResetsAfter { get; }

            public string Bucket { get; }

            public DateTimeOffset ServerDate { get; }

            public DateTimeOffset Date { get; }

            public DiscordHeaders(IDictionary<string, string> headers)
            {
                if (headers == null)
                    throw new ArgumentNullException(nameof(headers));

                if (headers.TryGetValue("X-RateLimit-Global", out var value) && bool.TryParse(value, out var isGlobal))
                    IsGlobal = isGlobal;

                if (headers.TryGetValue("Retry-After", out value) && int.TryParse(value, out var retryAfter))
                    RetryAfter = TimeSpan.FromSeconds(retryAfter);

                if (headers.TryGetValue("X-RateLimit-Limit", out value) && int.TryParse(value, out var limit))
                    Limit = limit;

                if (headers.TryGetValue("X-RateLimit-Remaining", out value) && int.TryParse(value, out var remaining))
                    Remaining = remaining;

                if (headers.TryGetValue("X-RateLimit-Reset", out value) && double.TryParse(value, NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out var resetsAt))
                    ResetsAt = DateTimeOffset.UnixEpoch + TimeSpan.FromSeconds(resetsAt);

                if (headers.TryGetValue("X-RateLimit-Reset-After", out value) && double.TryParse(value, NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out var resetsAfter))
                    ResetsAfter = TimeSpan.FromSeconds(resetsAfter);

                if (headers.TryGetValue("X-RateLimit-Bucket", out value))
                    Bucket = value;

                if (headers.TryGetValue("Date", out value) && DateTimeOffset.TryParse(value, out var serverDate))
                    ServerDate = serverDate;

                Date = DateTimeOffset.UtcNow;
            }

            public override string ToString()
                => $"IsGlobal:    {IsGlobal}\n" +
                   $"Limit:       {Limit}\n" +
                   $"Remaining:   {Remaining}\n" +
                   $"ResetsAt:    {ResetsAt}\n" +
                   $"ResetsAfter: {ResetsAfter}\n" +
                   $"Bucket:      {Bucket}\n" +
                   $"ServerDate:  {ServerDate}\n" +
                   $"Date:        {Date}";
        }
    }
}
