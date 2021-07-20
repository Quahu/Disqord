using System;
using System.Collections.Generic;
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
        private const string UnlimitedBucketString = "unlimited";

        /// <inheritdoc/>
        public ILogger Logger { get; }

        /// <inheritdoc/>
        public IRestApiClient ApiClient => _binder.Value;

        /// <summary>
        ///     Gets the maximum delay duration the rate-limiter will delay for
        ///     before throwing.
        /// </summary>
        public TimeSpan MaximumDelayDuration { get; }

        private readonly Binder<IRestApiClient> _binder;

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

            _hashes = new Dictionary<Route, string>();
            _buckets = new Dictionary<string, Bucket>();
            _hitRateLimits = new HashSet<Route>();
        }

        public void Bind(IRestApiClient apiClient)
        {
            _binder.Bind(apiClient);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public ValueTask EnqueueRequestAsync(IRestRequest request)
        {
            if (ApiClient.Requester.Version < 8)
            {
                // Asks Discord for accurate, not rounded up rate-limits on API versions before 8.
                request.Options.Headers["X-Ratelimit-Precision"] = "millisecond";
            }

            var bucket = GetBucket(request.Route, true);
            bucket.Enqueue(request);
            return default;
        }

        private Bucket GetBucket(FormattedRoute route, bool create)
        {
            lock (this)
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
            lock (this)
            {
                return _hashes.GetValueOrDefault(route) ?? $"{UnlimitedBucketString}+{route}";
            }
        }

        private bool UpdateBucket(FormattedRoute route, IHttpResponse response)
        {
            lock (this)
            {
                try
                {
                    var now = DateTimeOffset.UtcNow;
                    var bucket = GetBucket(route, true);
                    var wasUnlimited = bucket.IsUnlimited;
                    var headers = new DefaultRestResponseHeaders(response.Headers);
                    if (headers.Bucket.HasValue)
                    {
                        if (_hashes.TryAdd(route.BaseRoute, headers.Bucket.Value))
                            Logger.LogTrace("Cached bucket hash {0} -> {1}.", route, headers.Bucket.Value);

                        bucket = GetBucket(route, true);
                    }

                    if (headers.IsGlobal.GetValueOrDefault())
                    {
                        Logger.LogError("Hit the global rate-limit! Retry-After: {0}ms.", headers.RetryAfter.Value.TotalMilliseconds);
                    }
                    else if ((int) response.Code == 429)
                    {
                        bucket.Remaining = 0;
                        bucket.ResetsAt = now + headers.RetryAfter.Value;
                        var level = _hitRateLimits.Add(route.BaseRoute)
                            ? LogLevel.Information
                            : LogLevel.Warning;
                        Logger.Log(level, "Bucket {0} hit a rate-limit. Retry-After: {1}ms.", bucket, headers.RetryAfter.Value.TotalMilliseconds);
                        return true;
                    }

                    if (!headers.Bucket.HasValue)
                        return false;

                    bucket.Limit = Math.Max(1, headers.Limit.Value);
                    bucket.Remaining = headers.Remaining.Value;
                    bucket.ResetsAt = now + headers.ResetsAfter.Value;
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

        private class Bucket : ILogging
        {
            public ILogger Logger => _rateLimiter.Logger;

            public string Id { get; }

            public bool IsUnlimited => Id.StartsWith("unlimited");

            public int Limit { get; internal set; }

            public int Remaining { get; internal set; }

            public DateTimeOffset ResetsAt { get; internal set; }

            private readonly DefaultRestRateLimiter _rateLimiter;
            private readonly LinkedList<IRestRequest> _requests;

            private Task _task;

            public Bucket(DefaultRestRateLimiter rateLimiter, string id)
            {
                Id = id;

                Limit = 1;
                Remaining = 1;

                _rateLimiter = rateLimiter;
                _requests = new LinkedList<IRestRequest>();
            }

            public void Enqueue(IRestRequest request)
            {
                lock (_requests)
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
                    lock (_requests)
                    {
                        _requests.RemoveFirst();
                    }

                    if (Remaining == 0)
                    {
                        var delay = ResetsAt - DateTimeOffset.UtcNow;
                        if (delay > TimeSpan.Zero)
                        {
                            if (_rateLimiter.MaximumDelayDuration != Timeout.InfiniteTimeSpan && delay > _rateLimiter.MaximumDelayDuration)
                            {
                                Logger.LogWarning("Bucket {0} is pre-emptively rate-limiting. Throwing as the delay {1} exceeds the maximum delay duration.", Id, delay);
                                request.Complete(new MaximumRateLimitDelayExceededException(delay));
                                continue;
                            }

                            var level = request.Route.BaseRoute == Route.Channel.CreateReaction
                                ? LogLevel.Debug
                                : LogLevel.Information;
                            Logger.Log(level, "Bucket {0} is pre-emptively rate-limiting. Delaying for {1}.", Id, delay);
                            await Task.Delay(delay).ConfigureAwait(false);
                        }
                    }

                    // If this bucket is unlimited we check if there's a proper bucket created and move the requests accordingly.
                    if (IsUnlimited)
                    {
                        var bucket = _rateLimiter.GetBucket(request.Route, false);
                        if (bucket != this)
                        {
                            Logger.LogDebug("Bucket {0} is moving the request to the limited bucket {1}.", Id, bucket.Id);
                            bucket.Enqueue(request);
                            continue;
                        }
                    }

                    try
                    {
                        var response = await _rateLimiter.ApiClient.Requester.ExecuteAsync(request).ConfigureAwait(false);
                        if (_rateLimiter.UpdateBucket(request.Route, response.HttpResponse))
                        {
                            Logger.LogInformation("Bucket {0} is re-enqueuing the last request due to a hit rate-limit.", Id);
                            lock (_requests)
                            {
                                _requests.AddFirst(request);
                            }
                        }
                        else
                        {
                            request.Complete(response);
                            request.Dispose();
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
    }
}
