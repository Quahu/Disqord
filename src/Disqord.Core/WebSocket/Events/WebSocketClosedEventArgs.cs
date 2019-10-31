using System;

namespace Disqord.WebSocket
{
    public sealed class WebSocketClosedEventArgs : EventArgs
    {
        public int? Status { get; }

        public string Description { get; }

        public Exception Exception { get; }

        public WebSocketClosedEventArgs(int? status, string description, Exception exception)
        {
            Status = status;
            Description = description;
            Exception = exception;
        }
    }
}
