using System;

namespace Disqord.Rest.Api.Default
{
    public class DefaultRestRateLimiterConfiguration
    {
        public virtual TimeSpan MaximumDelayDuration { get; set; } = TimeSpan.FromSeconds(10);
    }
}