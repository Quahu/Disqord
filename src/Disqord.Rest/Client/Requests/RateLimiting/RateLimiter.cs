using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Disqord.Logging;

namespace Disqord.Rest
{
    internal sealed class RateLimiter
    {
        public readonly RestDiscordApiClient Client;

        private readonly ConcurrentDictionary<string, RateLimitBucket> _buckets = Extensions.CreateConcurrentDictionary<string, RateLimitBucket>(0);

        private static readonly ConcurrentDictionary<string, RateLimiter> _rateLimiters = Extensions.CreateConcurrentDictionary<string, RateLimiter>(1);

        private RateLimiter(RestDiscordApiClient client)
        {
            Client = client;
        }

        public async Task EnqueueRequestAsync(RestRequest request)
        {
            var bucket = _buckets.GetOrAdd(request.Identifier, (_, x) => new RateLimitBucket(x), this);
            var lastRateLimit = bucket.LastRateLimit;
            if (lastRateLimit != null && lastRateLimit.RetryAfter != null)
            {
                var delay = lastRateLimit.Date + lastRateLimit.RetryAfter.Value - DateTimeOffset.UtcNow;
                if (delay > TimeSpan.Zero)
                {
                    await Task.Delay(delay).ConfigureAwait(false);
                }
            }

            bucket.EnqueueRequest(request);
        }

        public async Task HandleRateLimitedAsync(RateLimit rateLimit)
        {
            // TODO
            Client.Log(rateLimit.IsGlobal
                ? LogMessageSeverity.Error
                : LogMessageSeverity.Warning,
                $"{(rateLimit.IsGlobal ? "Globally rate" : "Rate")} limited - will be delaying for {rateLimit.RetryAfter}.");
        }

        public static RateLimiter GetOrCreate(RestDiscordApiClient client)
        {
            if (client.Token == null)
                return new RateLimiter(client);

            return _rateLimiters.GetOrAdd(client.Token, (_, x) => new RateLimiter(x), client);
        }
    }
}
