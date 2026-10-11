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
    ///     Gets the rate-limit delay the request was about to wait.
    /// </summary>
    public TimeSpan Delay { get; }

    /// <summary>
    ///     Gets the combined rate-limit delay that exceeded the maximum total delay,
    ///     or <see cref="Delay"/> if <see cref="Delay"/> alone exceeded the maximum delay.
    /// </summary>
    public TimeSpan TotalDelay { get; }

    /// <summary>
    ///     Gets whether the combined rate-limit delay exceeded the
    ///     maximum total delay, rather than <see cref="Delay"/> exceeding the maximum delay.
    /// </summary>
    public bool IsTotalDelayExceeded { get; }

    /// <summary>
    ///     Gets whether the delay is from a global rate-limit.
    /// </summary>
    public bool IsGlobalRateLimit { get; }

    public MaximumRateLimitDelayExceededException(IRestRequest request, TimeSpan delay, bool isGlobalRateLimit)
        : base($"The rate-limit delay {delay} exceeded the maximum allowed delay.")
    {
        Request = request;
        Delay = delay;
        TotalDelay = delay;
        IsGlobalRateLimit = isGlobalRateLimit;
    }

    public MaximumRateLimitDelayExceededException(IRestRequest request, TimeSpan delay, TimeSpan totalDelay, bool isGlobalRateLimit)
        : base($"The total rate-limit delay {totalDelay} exceeded the maximum allowed total delay.")
    {
        Request = request;
        Delay = delay;
        TotalDelay = totalDelay;
        IsTotalDelayExceeded = true;
        IsGlobalRateLimit = isGlobalRateLimit;
    }
}