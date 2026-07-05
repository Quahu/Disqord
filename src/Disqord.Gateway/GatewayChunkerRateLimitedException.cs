using System;
using Disqord.Serialization.Json;

namespace Disqord.Gateway;

public class GatewayChunkerRateLimitedException(TimeSpan retryAfter, IJsonObject? metadata)
    : Exception("The chunk operation was rate-limited. Ensure you are not chunking the same guild too often.")
{
    public TimeSpan RetryAfter { get; } = retryAfter;

    public IJsonObject? Metadata { get; } = metadata;
}
