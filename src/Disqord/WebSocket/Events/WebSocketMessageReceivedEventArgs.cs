using System;
using System.IO;

namespace Disqord.WebSocket
{
    public sealed class WebSocketMessageReceivedEventArgs : EventArgs
    {
        public Stream Stream { get; }

        public WebSocketMessageReceivedEventArgs(Stream stream)
        {
            Stream = stream;
        }
    }
}
