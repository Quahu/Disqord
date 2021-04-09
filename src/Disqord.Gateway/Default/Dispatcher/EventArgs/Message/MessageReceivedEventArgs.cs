using System;

namespace Disqord.Gateway
{
    public class MessageReceivedEventArgs : EventArgs
    {
        /// <summary>
        ///     Gets the ID of the guild in which the message was sent in.
        ///     Returns <see langword="null"/> if the message was sent in a private channel.
        /// </summary>
        public Snowflake? GuildId => Message.GuildId;

        /// <summary>
        ///     Gets the ID of the channel in which the message was sent in.
        /// </summary>
        public Snowflake ChannelId => Message.ChannelId;

        /// <summary>
        ///     Gets the ID of the message that was sent.
        /// </summary>
        public Snowflake MessageId => Message.Id;

        /// <summary>
        ///     Gets the message received.
        /// </summary>
        public IGatewayMessage Message { get; }

        /// <summary>
        ///     Gets the cached text channel in which the message was sent in.
        ///     Returns <see langword="null"/> if the channel was not cached of if it was sent outside of a guild.
        /// </summary>
        public CachedTextChannel Channel { get; }

        /// <summary>
        ///     Gets the cached member that sent the message.
        ///     Returns <see langword="null"/> if the member was not cached or if it was sent outside of a guild.
        /// </summary>
        /// <remarks>
        ///     If this returns <see langword="null"/>, retrieve the author from the <see cref="Message"/> instead.
        /// </remarks>
        public CachedMember Member { get; }

        public MessageReceivedEventArgs(
            IGatewayMessage message,
            CachedTextChannel channel,
            CachedMember member)
        {
            Message = message;
            Channel = channel;
            Member = member;
        }
    }
}
