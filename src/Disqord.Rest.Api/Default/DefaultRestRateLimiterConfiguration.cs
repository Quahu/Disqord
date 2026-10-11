using System;

namespace Disqord.Rest.Api.Default;

public class DefaultRestRateLimiterConfiguration
{
    /// <summary>
    ///     Gets or sets the maximum rate-limit delay allowed before throwing an exception.
    ///     Defaults to <c>10</c> seconds.
    /// </summary>
    public virtual TimeSpan MaximumDelayDuration { get; set; } = TimeSpan.FromSeconds(10);

    /// <summary>
    ///     Gets or sets the maximum combined rate-limit delay of a request allowed before throwing an exception.
    ///     Defaults to <see cref="System.Threading.Timeout.InfiniteTimeSpan"/>, which does not limit it.
    /// </summary>
    public virtual TimeSpan MaximumTotalDelayDuration { get; set; } = System.Threading.Timeout.InfiniteTimeSpan;

    /// <summary>
    ///     Gets or sets the interval at which idle buckets whose rate-limits have reset are removed.
    ///     Set to <see cref="System.Threading.Timeout.InfiniteTimeSpan"/> to disable the removal.
    ///     Defaults to <c>10</c> minutes.
    /// </summary>
    public virtual TimeSpan BucketCleanupInterval { get; set; } = TimeSpan.FromMinutes(10);

    /// <summary>
    ///     Gets or sets whether a bucket sends as many requests at once as its rate-limit allows, which may then reach Discord out of order.
    ///     Defaults to <see langword="false"/>.
    /// </summary>
    public virtual bool SendsSameBucketRequestsConcurrently { get; set; }

    /// <summary>
    ///     Gets or sets the maximum amount of requests sent per second, not counting interaction responses and follow-ups.
    ///     Defaults to <c>50</c>; <see langword="null"/> disables the limit.
    /// </summary>
    public virtual int? GlobalRequestsPerSecond { get; set; } = 50;
}