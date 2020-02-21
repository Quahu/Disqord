using System;
using System.Globalization;
using System.Linq;
using System.Net.Http.Headers;

namespace Disqord.Rest
{
    internal sealed class RateLimit
    {
        public bool IsGlobal { get; }

        public int? Limit { get; }

        public int? Remaining { get; }

        public DateTimeOffset? ResetsAt { get; }

        public TimeSpan ResetsAfter { get; }

        public string Bucket { get; }

        public DateTimeOffset ServerDate { get; }

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

            if (headers.TryGetValues("X-RateLimit-Reset", out values) && double.TryParse(values.First(), NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out var resetsAt))
                ResetsAt = DateTimeOffset.UnixEpoch + TimeSpan.FromSeconds(resetsAt);

            if (headers.TryGetValues("X-RateLimit-Reset-After", out values) && double.TryParse(values.First(), NumberStyles.AllowDecimalPoint, NumberFormatInfo.InvariantInfo, out var resetsAfter))
                ResetsAfter = TimeSpan.FromSeconds(resetsAfter);

            if (headers.TryGetValues("X-RateLimit-Bucket", out values))
                Bucket = values.First();

            if (headers.TryGetValues("Date", out values) && DateTimeOffset.TryParse(values.First(), out var serverDate))
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
