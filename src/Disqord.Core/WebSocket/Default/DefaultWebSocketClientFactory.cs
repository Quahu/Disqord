using System;
using System.Net.WebSockets;
using Microsoft.Extensions.Options;

namespace Disqord.WebSocket.Default;

public class DefaultWebSocketClientFactory : IWebSocketClientFactory
{
    private readonly Action<ClientWebSocketOptions>? _clientConfiguration;

    public DefaultWebSocketClientFactory(
        IOptions<DefaultWebSocketClientFactoryConfiguration> options)
    {
        var configuration = options.Value;
        _clientConfiguration = configuration.ClientConfiguration;
    }

    public virtual IWebSocketClient CreateClient()
    {
        var ws = new ClientWebSocket();
        ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(10);

        _clientConfiguration?.Invoke(ws.Options);

        return new DefaultWebSocketClient(ws);
    }
}
