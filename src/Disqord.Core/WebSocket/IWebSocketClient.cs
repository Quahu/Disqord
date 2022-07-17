using System;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.WebSocket;

public interface IWebSocketClient : IDisposable
{
    WebSocketState State { get; }

    int? CloseStatus { get; }

    string? CloseMessage { get; }

    Task ConnectAsync(Uri uri, CancellationToken cancellationToken = default);

    Task CloseAsync(int closeStatus, string? closeMessage, CancellationToken cancellationToken = default);

    Task CloseOutputAsync(int closeStatus, string? closeMessage, CancellationToken cancellationToken = default);

    ValueTask<WebSocketResult> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken = default);

    ValueTask SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken = default);
}