using System.Net;
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

            var severity = rateLimit.IsGlobal
                ? LogMessageSeverity.Error
                : LogMessageSeverity.Warning;
            var message = $"{(rateLimit.IsGlobal ? "Globally rate" : "Rate")} limited for {rateLimit.ResetsAfter}";
            if (request.Options.MaximumRateLimitDuration != default && rateLimit.ResetsAfter > request.Options.MaximumRateLimitDuration)
            {
                request.SetException(new DiscordHttpException((HttpStatusCode) 429, null, "Rate-limit hit. Throwing due to Options.ThrowOnRateLimits."));
                Client.Log(severity, $"{message} - will be throwing due to Options.ThrowOnRateLimits.");
                return;
            }

            Client.Log(severity, $"{message} - will be delaying.");
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
