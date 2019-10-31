using System;
using System.IO;

namespace Disqord.WebSocket
{
    public sealed class WebSocketMessageReceivedEventArgs : EventArgs
    {
        public Stream Stream { get; }

        public WebSocketMessageReceivedEventArgs(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            Stream = stream;
        }
    }
}
