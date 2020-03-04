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

        public async Task HandleRateLimitedAsync(RestRequest request, RateLimit rateLimit)
        {
            // TODO: proper rate-limit handling! (semaphore for global rates? / throw?)
            //       (also move to Discord's buckets)
            Client.Log(rateLimit.IsGlobal
                ? LogMessageSeverity.Error
                : LogMessageSeverity.Warning,
                $"{(rateLimit.IsGlobal ? "Globally rate" : "Rate")} limited - will be delaying for {rateLimit.ResetsAfter}{(rateLimit.IsGlobal ? "" : " and resending the request")}.");
            await Task.Delay(rateLimit.ResetsAfter).ConfigureAwait(false);

            if (!rateLimit.IsGlobal)
            {
                request.Initialise(Client.Serializer);
                await Client.HandleRequestAsync(request).ConfigureAwait(false);
            }
        }

        public static RateLimiter GetOrCreate(RestDiscordApiClient client)
        {
            if (client._token == null)
                return new RateLimiter(client);

            return _rateLimiters.GetOrAdd(client._token, (_, x) => new RateLimiter(x), client);
        }
    }
}
