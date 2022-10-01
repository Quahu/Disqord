using System;

namespace Disqord.Gateway.Default;

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
}
