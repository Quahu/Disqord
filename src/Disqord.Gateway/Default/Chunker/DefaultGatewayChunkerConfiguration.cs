using System;

namespace Disqord.Gateway.Default;

/// <summary>
///     Represents the configuration of <see cref="DefaultGatewayChunker"/>.
/// </summary>
public class DefaultGatewayChunkerConfiguration
{
    /// <summary>
    ///     Gets or sets the time after which, if no chunk was received,
    ///     the chunk operations will time out and be retried.
    /// </summary>
    /// <remarks>
    ///     Defaults to <c>10</c> seconds.
    /// </remarks>
    public TimeSpan OperationTimeout { get; set; } = TimeSpan.FromSeconds(10);

    /// <summary>
    ///     Gets or sets the maximum number of times a chunk operation is attempted.
    /// </summary>
    /// <remarks>
    ///     Defaults to <see langword="null"/>, i.e., the operations are retried indefinitely.
    /// </remarks>
    public int? MaxOperationAttempts { get; set; }

    /// <inheritdoc cref="AutomaticMemberChunkingConfiguration"/>
    /// <summary>
    ///     Gets or sets the configuration of automatic member chunking,
    ///     i.e., populating the member cache of guilds over the gateway as they become available.
    /// </summary>
    public AutomaticMemberChunkingConfiguration AutomaticMemberChunking { get; set; } = new();
}
