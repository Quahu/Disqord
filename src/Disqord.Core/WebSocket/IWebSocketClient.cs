using System;
using System.Threading;
using System.Threading.Tasks;
using Disqord.WebSocket.Default;

namespace Disqord.WebSocket
{
    public interface IWebSocketClient : IDisposable
    {
        WebSocketState State { get; }

        int? CloseStatus { get; }

        string CloseMessage { get; }

        ValueTask ConnectAsync(Uri uri, CancellationToken cancellationToken = default);

        ValueTask CloseAsync(int closeStatus, string closeMessage, CancellationToken cancellationToken = default);

        ValueTask CloseOutputAsync(int closeStatus, string closeMessage, CancellationToken cancellationToken = default);

        ValueTask<WebSocketResult> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken = default);

        ValueTask SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken = default);
    }
}
