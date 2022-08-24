namespace Disqord.Gateway.Api;

/// <summary>
///     Represents the state of a shard.
/// </summary>
public enum ShardState
{
    /// <summary>
    ///     The shard is disconnected.
    /// </summary>
    Disconnected,

    /// <summary>
    ///     The shard is connecting.
    /// </summary>
    Connecting,

    /// <summary>
    ///     The shard is connected and awaiting <see cref="GatewayPayloadOperation.Hello"/>.
    /// </summary>
    Connected,

    /// <summary>
    ///     The shard is connected and identifying.
    /// </summary>
    Identifying,

    /// <summary>
    ///     The shard is connected and ready.
    /// </summary>
    Ready,

    /// <summary>
    ///     The shard is reconnecting due to a reconnect request.
    /// </summary>
    Reconnecting,

    /// <summary>
    ///     The shard is resuming.
    /// </summary>
    Resuming
}
