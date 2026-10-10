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
    ///     Gets or sets the interval at which idle buckets whose rate-limits have reset are removed.
    ///     Set to <see cref="System.Threading.Timeout.InfiniteTimeSpan"/> to disable the removal.
    ///     Defaults to <c>10</c> minutes.
    /// </summary>
    public virtual TimeSpan BucketCleanupInterval { get; set; } = TimeSpan.FromMinutes(10);
}