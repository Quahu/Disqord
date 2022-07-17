using System;
using Disqord.Rest.Api;

namespace Disqord.Rest;

/// <summary>
///     Represents an exception that is thrown when the rate-limit duration
///     exceeds the allowed delay.
/// </summary>
public class MaximumRateLimitDelayExceededException : TimeoutException
{
    /// <summary>
    ///     Gets the requests that exceeded the maximum allowed delay.
    /// </summary>
    public IRestRequest Request { get; }

    /// <summary>
    ///     Gets the delay that exceeded the maximum allowed delay.
    /// </summary>
    public TimeSpan Delay { get; }

    /// <summary>
    ///     Gets whether the delay is from a global rate-limit.
    /// </summary>
    public bool IsGlobalRateLimit { get; }

    public MaximumRateLimitDelayExceededException(IRestRequest request, TimeSpan delay, bool isGlobalRateLimit)
        : base($"The rate-limit delay {delay} exceeded the maximum allowed delay.")
    {
        Request = request;
        Delay = delay;
        IsGlobalRateLimit = isGlobalRateLimit;
    }
}