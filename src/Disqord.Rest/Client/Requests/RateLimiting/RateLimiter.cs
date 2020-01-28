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

        public void EnqueueRequest(RestRequest request)
        {
            var bucket = _buckets.GetOrAdd(request.Identifier, (_, x) => new RateLimitBucket(x), this);
            bucket.EnqueueRequest(request);
        }

        public async Task HandleRateLimitedAsync(RateLimit rateLimit)
        {
            // TODO
            Client.Log(rateLimit.IsGlobal
                ? LogMessageSeverity.Error
                : LogMessageSeverity.Warning,
                $"{(rateLimit.IsGlobal ? "Globally rate" : "Rate")} limited - will be delaying for {rateLimit.ResetsAfter}.");
        }

        public static RateLimiter GetOrCreate(RestDiscordApiClient client)
        {
            if (client._token == null)
                return new RateLimiter(client);

            return _rateLimiters.GetOrAdd(client._token, (_, x) => new RateLimiter(x), client);
        }
    }
}
