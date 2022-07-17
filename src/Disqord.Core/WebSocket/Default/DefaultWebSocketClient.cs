using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace Disqord.WebSocket.Default;

public class DefaultWebSocketClient : IWebSocketClient
{
    public ClientWebSocket Client => _ws;

    public WebSocketState State => (WebSocketState) _ws.State;

    public int? CloseStatus => (int?) _ws.CloseStatus;

    public string? CloseMessage => _ws.CloseStatusDescription;

    private readonly ClientWebSocket _ws;

    public DefaultWebSocketClient(ClientWebSocket ws)
    {
        _ws = ws;
    }

    public async Task ConnectAsync(Uri uri, CancellationToken cancellationToken = default)
    {
        try
        {
            await _ws.ConnectAsync(uri, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new WebSocketClosedException(CloseStatus, CloseMessage, ex);
        }
    }

    public async Task CloseAsync(int closeStatus, string? closeMessage, CancellationToken cancellationToken = default)
    {
        try
        {
            await _ws.CloseAsync((WebSocketCloseStatus) closeStatus, closeMessage, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new WebSocketClosedException(CloseStatus, CloseMessage, ex);
        }
    }

    public async Task CloseOutputAsync(int closeStatus, string? closeMessage, CancellationToken cancellationToken = default)
    {
        try
        {
            await _ws.CloseOutputAsync((WebSocketCloseStatus) closeStatus, closeMessage, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new WebSocketClosedException(CloseStatus, CloseMessage, ex);
        }
    }

    public async ValueTask<WebSocketResult> ReceiveAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        ValueWebSocketReceiveResult result;
        try
        {
            result = await _ws.ReceiveAsync(buffer, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new WebSocketClosedException(CloseStatus, CloseMessage, ex);
        }

        return new WebSocketResult(result.Count, (WebSocketMessageType) result.MessageType, result.EndOfMessage);
    }

    public async ValueTask SendAsync(ReadOnlyMemory<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken = default)
    {
        try
        {
            await _ws.SendAsync(buffer, (System.Net.WebSockets.WebSocketMessageType) messageType, endOfMessage, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new WebSocketClosedException(CloseStatus, CloseMessage, ex);
        }
    }

    public void Dispose()
    {
        _ws.Dispose();
    }
}