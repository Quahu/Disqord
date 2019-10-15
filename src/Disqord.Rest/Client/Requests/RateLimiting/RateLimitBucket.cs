using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;

namespace Disqord.Rest
{
    internal sealed class RateLimitBucket
    {
        public RateLimit LastRateLimit { get; private set; }

        private Task _runTask;

        private readonly ConcurrentQueue<RestRequest> _requests = new ConcurrentQueue<RestRequest>();

        private readonly RateLimiter _rateLimiter;

        private readonly object _lock = new object();

        public RateLimitBucket(RateLimiter rateLimiter)
        {
            _rateLimiter = rateLimiter;
        }

        public void EnqueueRequest(RestRequest request)
        {
            _requests.Enqueue(request);
            lock (_lock)
            {
                if (_runTask == null || _runTask.IsCompleted)
                    _runTask = Task.Run(RunAsync);
            }
        }

        private async Task RunAsync()
        {
            while (_requests.TryDequeue(out var request))
            {
                using (request)
                {
                    if (LastRateLimit?.Remaining == 0)
                    {
                        TimeSpan delay;
                        if (request.RateLimitOverride != null)
                        {
                            delay = TimeSpan.FromMilliseconds(request.RateLimitOverride.Value);
                        }
                        else
                        {
                            //delay = _rateLimit.ResetsAt.Value - _rateLimit.Date;
                            delay = TimeSpan.FromSeconds(LastRateLimit.ResetsAfter);
                        }

                        if (delay > TimeSpan.Zero)
                        {
                            await Task.Delay(delay).ConfigureAwait(false);
                        }
                    }

                    try
                    {
                        LastRateLimit = await _rateLimiter.Client.HandleRequestAsync(request).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        request.SetException(ex);
                    }
                }
            }
        }

        public static string GenerateIdentifier(HttpMethod method, bool usesMethod, string route, ulong guildId, ulong channelId, ulong webhookId)
            => usesMethod
                ? $"{method} {route}: ({guildId};{channelId};{webhookId})"
                : $"{route}: ({guildId};{channelId};{webhookId})";
    }
}
