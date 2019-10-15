using System;
using System.Linq;
using System.Net.Http.Headers;

namespace Disqord.Rest
{
    internal sealed class RateLimit
    {
        public bool IsGlobal { get; }

        public int? Limit { get; }

        public int? Remaining { get; }

        public TimeSpan? RetryAfter { get; }

        public DateTimeOffset? ResetsAt { get; }

        public double ResetsAfter { get; }

        public DateTimeOffset? ServerDate { get; }

        public DateTimeOffset Date { get; }

        public RateLimit(HttpResponseHeaders headers)
        {
            if (headers == null)
                throw new ArgumentNullException(nameof(headers));

            if (headers.TryGetValues("X-RateLimit-Global", out var values) && bool.TryParse(values.First(), out var isGlobal))
                IsGlobal = isGlobal;

            if (headers.TryGetValues("X-RateLimit-Limit", out values) && int.TryParse(values.First(), out var limit))
                Limit = limit;

            if (headers.TryGetValues("X-RateLimit-Remaining", out values) && int.TryParse(values.First(), out var remaining))
                Remaining = remaining;

            if (headers.TryGetValues("X-RateLimit-Reset", out values) && long.TryParse(values.First(), out var resetsAt))
                ResetsAt = DateTimeOffset.FromUnixTimeSeconds(resetsAt);

            if (headers.TryGetValues("X-RateLimit-Reset-After", out values) && double.TryParse(values.First(), out var resetsAfter))
                ResetsAfter = resetsAfter;

            if (headers.TryGetValues("Retry-After", out values) && int.TryParse(values.First(), out var retryAfter))
                RetryAfter = TimeSpan.FromMilliseconds(retryAfter);

            if (headers.TryGetValues("Date", out values) && DateTimeOffset.TryParse(values.First(), out var serverDate))
                ServerDate = serverDate;

            Date = DateTimeOffset.UtcNow;
        }

        public override string ToString()
            => $"IsGlobal:    {IsGlobal}\n" +
               $"Limit:       {Limit}\n" +
               $"Remaining:   {Remaining}\n" +
               $"RetryAfter:  {RetryAfter}\n" +
               $"ResetsAt:    {ResetsAt}\n" +
               $"ResetsAfter: {ResetsAfter}\n" +
               $"ServerDate:  {ServerDate}\n" +
               $"Date:        {Date}";
    }
}
