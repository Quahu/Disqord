using System;

namespace Disqord.Rest
{
    public class MaximumRateLimitDelayExceededException : TimeoutException
    {
        /// <summary>
        ///     Gets the delay that exceeded the maximum allowed delay.
        /// </summary>
        public TimeSpan Delay { get; }

        /// <summary>
        ///     Gets whether the delay is from a global rate-limit.
        /// </summary>
        public bool IsGlobalRateLimit { get; }

        public MaximumRateLimitDelayExceededException(TimeSpan delay, bool isGlobalRateLimit)
            : base($"The rate-limit delay {delay} exceeded the maximum allowed delay.")
        {
            Delay = delay;
            IsGlobalRateLimit = isGlobalRateLimit;
        }
    }
}
