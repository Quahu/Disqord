namespace Disqord.WebSocket.Default
{
    public class DefaultWebSocketClientFactory : IWebSocketClientFactory
    {
        public IWebSocketClient CreateClient()
            => new DefaultWebSocketClient();
    }
}
