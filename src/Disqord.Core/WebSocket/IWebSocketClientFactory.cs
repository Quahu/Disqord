namespace Disqord.WebSocket;

public interface IWebSocketClientFactory
{
    IWebSocketClient CreateClient();
}