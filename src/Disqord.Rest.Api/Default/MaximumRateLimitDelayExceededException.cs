using System;

namespace Disqord.Rest.Api.Default
{
    public class MaximumRateLimitDelayExceededException : TimeoutException
    {
        public TimeSpan Delay { get; }

        public MaximumRateLimitDelayExceededException(TimeSpan delay)
            : base($"The rate-limit delay {delay} exceeded the maximum allowed delay.")
        {
            Delay = delay;
        }
    }
}
