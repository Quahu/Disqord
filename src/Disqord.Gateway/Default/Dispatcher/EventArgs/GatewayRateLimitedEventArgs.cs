using System;
using Disqord.Gateway.Api;
using Disqord.Serialization.Json;

namespace Disqord.Gateway;

public class GatewayRateLimitedEventArgs(ShardId shardId, GatewayPayloadOperation operation, TimeSpan retryAfter, IJsonObject? metadata)
    : EventArgs
{
    /// <summary>
    ///     Gets the ID of the shard that was rate-limited.
    /// </summary>
    public ShardId ShardId { get; } = shardId;

    /// <summary>
    ///     Gets the gateway operation that was rate-limited.
    /// </summary>
    public GatewayPayloadOperation Operation { get; } = operation;

    /// <summary>
    ///     Gets the time after which it is safe to retry the gateway operation.
    /// </summary>
    public TimeSpan RetryAfter { get; } = retryAfter;

    /// <summary>
    ///     Gets the metadata of this rate-limited event.
    /// </summary>
    public IJsonObject? Metadata { get; } = metadata;
}
