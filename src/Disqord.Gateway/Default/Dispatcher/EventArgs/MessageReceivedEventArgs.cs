using System;

namespace Disqord.Gateway
{
    public class MessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the message received.
        /// </summary>
        public IGatewayMessage Message { get; }

        /// <summary>
        ///     Gets the cached text channel the message was sent in.
        ///     Returns <see langword="null"/>, if the channel was not cached.
        /// </summary>
        public ITextChannel Channel { get; }

        public MessageReceivedEventArgs(IGatewayMessage message, ITextChannel channel)
        {
            Message = message;
            Channel = channel;
        }
    }
}
