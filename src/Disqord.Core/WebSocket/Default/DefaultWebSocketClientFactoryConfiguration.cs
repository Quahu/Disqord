using System;
using System.Net.WebSockets;

namespace Disqord.WebSocket.Default;

public class DefaultWebSocketClientFactoryConfiguration
{
    /// <summary>
    ///     Gets or sets the action that configures the <see cref="ClientWebSocketOptions"/>
    ///     created by the <see cref="DefaultWebSocketClientFactory"/>.
    /// </summary>
    /// <remarks>
    ///     This should only be used for, for example, setting the proxy.
    /// </remarks>
    public Action<ClientWebSocketOptions>? ClientConfiguration { get; set; }
}
