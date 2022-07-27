namespace Disqord.Gateway.Api;

public enum GatewayState
{
    /// <summary>
    ///     The gateway is disconnected.
    /// </summary>
    Disconnected,

    /// <summary>
    ///     The gateway is connecting.
    /// </summary>
    Connecting,

    /// <summary>
    ///     The gateway is connected and awaiting <see cref="GatewayPayloadOperation.Hello"/>.
    /// </summary>
    Connected,

    /// <summary>
    ///     The gateway is connected and identifying.
    /// </summary>
    Identifying,

    /// <summary>
    ///     The gateway is connected and ready.
    /// </summary>
    Ready,

    /// <summary>
    ///     The gateway is reconnecting due to a reconnect request.
    /// </summary>
    Reconnecting,

    /// <summary>
    ///     The gateway is resuming.
    /// </summary>
    Resuming
}
