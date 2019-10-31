using System;
using System.Threading;
using System.Threading.Tasks;
using Qommon.Events;

namespace Disqord.WebSocket
{
    public interface IWebSocketClient : IDisposable
    {
        event AsynchronousEventHandler<WebSocketMessageReceivedEventArgs> MessageReceived;

        event AsynchronousEventHandler<WebSocketClosedEventArgs> Closed;

        Task ConnectAsync(Uri url, CancellationToken token);

        Task SendAsync(WebSocketRequest request);

        Task CloseAsync();
    }
}