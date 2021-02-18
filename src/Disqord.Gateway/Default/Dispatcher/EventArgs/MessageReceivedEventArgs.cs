using System;

namespace Disqord.Gateway
{
    public class MessageReceivedEventArgs : EventArgs
    {
        public IMessage Message { get; }

        public MessageReceivedEventArgs(IMessage message)
        {
            Message = message;
        }
    }
}
