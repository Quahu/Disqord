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
        ///     Returns <see langword="null"/>, if the channel was not cached of if it was sent outside of a guild.
        /// </summary>
        public CachedTextChannel Channel { get; }

        /// <summary>
        ///     Gets the cached member that sent the message.
        ///     Returns <see langword="null"/>, if the member was not cached or if it was sent outside of a guild.
        /// </summary>
        /// <remarks>
        ///     If this returns <see langword="null"/>, retrieve the author from the <see cref="Message"/> instead.
        /// </remarks>
        public CachedMember Author { get; }

        public MessageReceivedEventArgs(IGatewayMessage message, CachedTextChannel channel, CachedMember author)
        {
            Message = message;
            Channel = channel;
            Author = author;
        }
    }
}
