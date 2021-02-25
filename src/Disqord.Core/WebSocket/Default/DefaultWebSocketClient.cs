using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.WebSocket.Default
{
    public class DefaultWebSocketClient : IWebSocketClient
    {
        public WebSocketState State => (WebSocketState) _ws.State;

        public int? CloseStatus => (int?) _ws.CloseStatus;

        public string? CloseMessage => _ws.CloseStatusDescription;

        private readonly ClientWebSocket _ws;

        public DefaultWebSocketClient()
        {
            _ws = new ClientWebSocket();
            _ws.Options.KeepAliveInterval = TimeSpan.FromSeconds(10);
        }

        public ValueTask ConnectAsync(Uri uri, CancellationToken cancellationToken = default)
            => new ValueTask(_ws.ConnectAsync(uri, cancellationToken));

        public ValueTask CloseAsync(int closeStatus, string closeMessage, CancellationToken cancellationToken = default)
            => new ValueTask(_ws.CloseAsync((WebSocketCloseStatus) closeStatus, closeMessage, cancellationToken));

        public ValueTask CloseOutputAsync(int closeStatus, string closeMessage, CancellationToken cancellationToken = default)
            => new ValueTask(_ws.CloseOutputAsync((WebSocketCloseStatus) closeStatus, closeMessage, cancellationToken));

        public async ValueTask<WebSocketResult> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
        {
            ValueWebSocketReceiveResult result;
            try
            {
                result = await _ws.ReceiveAsync(buffer, cancellationToken).ConfigureAwait(false);
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new WebSocketClosedException(CloseStatus, CloseMessage, ex);
            }

            return new WebSocketResult(result.Count, (WebSocketMessageType) result.MessageType, result.EndOfMessage);
        }

        public ValueTask SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken = default)
            => _ws.SendAsync(buffer, (System.Net.WebSockets.WebSocketMessageType) messageType, endOfMessage, cancellationToken);

        public void Dispose()
        {
            _ws.Dispose();
        }
    }
}
