using System;
using System.IO;

namespace Disqord.WebSocket
{
    internal sealed class WebSocketMessageReceivedEventArgs : EventArgs
    {
        public Stream Stream { get; }

        public WebSocketMessageReceivedEventArgs(Stream stream)
        {
            Stream = stream;
        }
    }
}
