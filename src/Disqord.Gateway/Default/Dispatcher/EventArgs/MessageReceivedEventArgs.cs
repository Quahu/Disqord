using System;

namespace Disqord.Gateway
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public IGatewayMessage Message { get; }

        public ITextChannel Channel { get; }

        public MessageReceivedEventArgs(IGatewayMessage message, ITextChannel channel)
        {
            Message = message;
            Channel = channel;
        }
    }
}
