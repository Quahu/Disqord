using System;

namespace Disqord.Rest.Api.Default;

public class DefaultRestRateLimiterConfiguration
{
    /// <summary>
    ///     Gets or sets the maximum rate-limit delay allowed before throwing an exception.
    ///     Defaults to <c>10</c> seconds.
    /// </summary>
    public virtual TimeSpan MaximumDelayDuration { get; set; } = TimeSpan.FromSeconds(10);
}