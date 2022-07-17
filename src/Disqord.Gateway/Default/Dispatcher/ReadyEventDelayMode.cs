using Disqord.Gateway.Default;

namespace Disqord;

/// <summary>
///     Defines how the dispatcher should behave when invoking <see cref="DefaultGatewayDispatcher.ReadyEvent"/>.
/// </summary>
public enum ReadyEventDelayMode : byte
{
    /// <summary>
    ///     The event is not delayed in any way.
    ///     It is dispatched as soon as the bot successfully identifies with the gateway.
    /// </summary>
    None = 0,

    /// <summary>
    ///     The event is delayed until all guilds are received from the gateway.
    ///     This applies to both available and unavailable guilds.
    /// </summary>
    Guilds = 1,

    ///// <summary>
    /////     The event is delayed until all guilds are chunked.
    /////     For more information see <see cref="IGatewayChunker"/>.
    ///// </summary>
    //Members = 2
}
