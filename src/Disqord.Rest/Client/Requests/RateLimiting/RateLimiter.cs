using System;
using System.Threading.Tasks;
using Disqord.Collections;
using Disqord.Logging;

namespace Disqord.Rest
{
    internal sealed class RateLimiter
    {
        public readonly RestDiscordApiClient Client;

        private readonly LockedDictionary<string, RateLimitBucket> _buckets = new LockedDictionary<string, RateLimitBucket>();

        private static readonly LockedDictionary<string, RateLimiter> _rateLimiters = new LockedDictionary<string, RateLimiter>(1);

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
            if (client._token == null)
                return new RateLimiter(client);

            return _rateLimiters.GetOrAdd(client._token, (_, x) => new RateLimiter(x), client);
        }
    }
}
