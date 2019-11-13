using System;
using System.Net.WebSockets;

namespace Disqord.WebSocket
{
    internal sealed class WebSocketClosedEventArgs : EventArgs
    {
        public WebSocketCloseStatus? Status { get; }

        public string Description { get; }

        public Exception Exception { get; }

        public WebSocketClosedEventArgs(WebSocketCloseStatus? status, string description, Exception exception)
        {
            Status = status;
            Description = description;
            Exception = exception;
        }
    }
}
